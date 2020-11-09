using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class SimpleProjectileController : BaseProjectileController
{

    protected bool IsLaunched = false;

    protected SpriteRenderer Renderer;
    protected Rigidbody2D Rigidbody;
    protected Collider2D Collider;
    protected BaseVisualEffect OnHitEffect;

    private IDisposable DeactivationTimer;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();

        IsFreeToReuse = true;

        if (model != null)
            SetupModel(model);
    }

    public override void OnCreate(object options) { }

    public override void SetupModel(ProjectileModel model) {
        this.model = model;

        Renderer.sprite = model.Image;

        if (Collider == null)
            Destroy(Collider);
        Collider = gameObject.AddComponent<BoxCollider2D>();

        if (OnHitEffect != null) Destroy(OnHitEffect.gameObject);

        OnHitEffect = ProjectileFactory.CreateOnHitEffect(model);
        OnHitEffect.transform.parent = transform;
        OnHitEffect.gameObject.SetActive(false);
    }

    public override void Launch(Vector2 position, Vector2 direction) {
        transform.position = position;
        transform.up = direction;

        IsFreeToReuse = false;
        IsLaunched = true;
        gameObject.SetActive(true);

        Rigidbody.velocity = direction.normalized * model.Speed;

        DeactivationTimer = Observable.Timer(System.TimeSpan.FromSeconds(model.Lifetime))
            .Subscribe(_ => Deactivate());
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        Deactivate();

        OnHitEffect.gameObject.SetActive(true);
        OnHitEffect.PlayAt(transform.position + transform.up * 0.2f);

        BaseMeteor meteorController = collision.gameObject.GetComponent<BaseMeteor>();
        if (meteorController != null) {
            meteorController.Hit(model.Damage.RandomValue);
        }
    }

    protected void Deactivate() {
        gameObject.SetActive(false);
        IsLaunched = false;
        IsFreeToReuse = true;
        DeactivationTimer.Dispose();
    }
}
