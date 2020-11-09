using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class SimpleMeteor : BaseMeteor
{
    public override Collider2D Collider { get; protected set; }
    public override Rigidbody2D Rigidbody { get; protected set; }
    protected SpriteRenderer Renderer;

    public BaseVisualEffect ExplosionEffect;

    public float MaxHealth { get; protected set; }
    public float Health { get; protected set; }

    public override bool IsFreeToReuse {
        get { return !gameObject.activeSelf; }
    }

    private void Awake() {
        Renderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();

        if (model != null)
            SetupModel(model);

        ExplosionEffect.gameObject.SetActive(false);
    }

    public override void SetupModel(MeteorModel model) {
        base.SetupModel(model);

        Renderer.sprite = model.Image;
        Renderer.color = model.Tint;

        // Reset collider to sync it with new sprite
        if (Collider != null)
            Destroy(Collider);
        Collider = gameObject.AddComponent<PolygonCollider2D>();

        MaxHealth = model.Health;
        Rigidbody.mass = model.Mass.RandomValue;
    }

    public override void Launch(Vector2 position, Vector2 direction) {
        gameObject.SetActive(true);

        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
        Rigidbody.velocity = direction.normalized * model.Speed.RandomValue;

        Health = MaxHealth;
    }

    public override void Hit(float damage) {
        Health = Mathf.Max(Health - damage, 0);
        if (Health <= 0) {
            Explode();
            ExcludeFromGame();
        }
    }

    public virtual void Explode() {
        ExplosionEffect.gameObject.SetActive(true);
        ExplosionEffect.PlayAt(transform.position);
    }

    public override void ExcludeFromGame() {
        gameObject.SetActive(false);
        Rigidbody.velocity = Vector2.zero;
    }

    public override void OnCreate(object options) {}
}
