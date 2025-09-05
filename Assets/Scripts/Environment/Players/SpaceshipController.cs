using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class SpaceshipController : MonoBehaviour, IModelBased<SpaceshipModel>, IPhysEntity
{

    private SpaceshipModel Model;

    public Rigidbody2D Rigidbody { get; protected set; }
    public Collider2D Collider { get; protected set; }
    private SpriteRenderer Renderer;

    private SpaceshipGun[] Guns = null;
    private SpaceshipEngine[] Engines = null;
    private ObjectPool<BaseProjectileController> Projectiles;

    private ISpaceshipControllerState CurrentState;

    private ReactiveProperty<int> _Health = new ReactiveProperty<int>(0);
    public int Health {
        get { return _Health.Value; }
        set {
            _Health.Value = Mathf.Max(value, 0);
            Publish(SpaceshipMessage.MessageType.HealthChanged);
            if (_Health.Value <= 0)
                Publish(SpaceshipMessage.MessageType.Died);
        }
    }

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();

        MessageBroker.Default.Receive<SpaceshipMessage>().Subscribe(msg => {
            CurrentState = CurrentState.UpdateState(msg.Type);
            Debug.Log("State updated to " + CurrentState);
        });
        if (Model != null)
            SetupModel(Model);
    }

    IDisposable MoveDisposable = null, ShootDisposable = null;

    void RestartUpdates()
    {
        if (MoveDisposable != null) MoveDisposable.Dispose();
        if (ShootDisposable != null) ShootDisposable.Dispose();

        Observable.EveryUpdate()
            .Where(_ => Input.anyKey && CurrentState.CanMove)
            .Subscribe(_ =>
            {
                Move();
                CheckProjectilesChange();
            }).AddTo(this);

        Observable.EveryUpdate()
            .Subscribe(_ => UpdateEngines()).AddTo(this);

        ShootDisposable = Observable.Interval(System.TimeSpan.FromSeconds(Model.ShootRate))
            .Where(_ => Input.GetButton("Fire1") && CurrentState.CanShoot)
            .Subscribe(_ => Shoot()).AddTo(this);
    }


    public void SetupModel(SpaceshipModel model)
    {
        this.Model = model;

        Rigidbody.drag = model.LinearDrag;
        Renderer.sprite = model.Image;

        if (Engines != null) foreach (var engine in Engines) Destroy(engine.gameObject);
        Engines = new SpaceshipEngine[Model.EnginePoints.Length];

        int i = 0;
        foreach (var enginePoint in Model.EnginePoints) {
            Engines[i] = SpaceshipFactory.CreateEngine(model);
            Engines[i].transform.localPosition = new Vector3(
                enginePoint.x, enginePoint.y, Engines[i].transform.localPosition.z
            );
            Engines[i].transform.parent = transform;
            i++;
        }

        if (Guns != null) foreach (var gun in Guns) Destroy(gun.gameObject);
        Guns = new SpaceshipGun[Model.GunPoints.Length];

        i = 0;
        foreach (var gunPoint in Model.GunPoints) {
            Guns[i] = SpaceshipFactory.CreateGun(model);
            Guns[i].Setup(model, Projectiles);
            Guns[i].transform.localPosition = new Vector3(
                gunPoint.x, gunPoint.y, Guns[i].transform.localPosition.z
            );
            Guns[i].transform.parent = transform;
            i++;
        }

        SetProjectilesOnGun(GameConfig.Current.Projectiles);

        if (Collider != null) Destroy(Collider);
        Collider = gameObject.AddComponent<PolygonCollider2D>();

        RestartUpdates();

        Reset();
    }

    public void Reset()
    {
        _Health.Value = Model.HealthPoints;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = 0;

        CurrentState = new DefaultSpaceshipControllerState();
    }

    private void SetProjectilesOnGun(ProjectileModel projectiles)
    {
        Projectiles = new ObjectPool<BaseProjectileController>(
            () => ProjectileFactory.CreateProjectile(projectiles),
            container: LevelController.Current.ProjectilesContainer);
        for (int i = 0; i < Guns.Length; i++)
            Guns[i].Setup(Model, Projectiles);
    }

    private void Move()
    {
        Vector2 direction = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (direction.magnitude > 1) direction.Normalize();

        Rigidbody.AddForce(direction * Model.Speed * Time.deltaTime);

        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        float angle = Vector2.SignedAngle(Vector2.up, new Vector2(
            direction.x - transform.position.x,
            direction.y - transform.position.y
        ));
        transform.localEulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, Time.deltaTime * Model.RotationSpeed));
    }

    private void CheckProjectilesChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetProjectilesOnGun(GameConfig.Current.ProjectilesByIndex(0));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetProjectilesOnGun(GameConfig.Current.ProjectilesByIndex(1));
    }

    private void UpdateEngines()
    {
        foreach (var engine in Engines) engine.SetForce(Rigidbody.velocity.magnitude);
    }

    private void Shoot() {
        foreach (var gun in Guns)
            gun.Shoot();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        BaseMeteor meteor = collision.gameObject.GetComponent<BaseMeteor>();

        if (meteor != null && CurrentState.CanBeHit) {
            Publish(SpaceshipMessage.MessageType.Hit);
            Health -= 1;
        }
    }

    private void Publish(SpaceshipMessage.MessageType type) {
        MessageBroker.Default.Publish<SpaceshipMessage>(new SpaceshipMessage(type));
    }
}
