using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMeteor : MonoBehaviour, IModelBased<MeteorModel>, IObjectPoolItem, IPhysEntity
{

    [SerializeField]
    protected MeteorModel model;

    public abstract bool IsFreeToReuse { get; }

    public abstract Rigidbody2D Rigidbody { get; protected set; }

    public abstract Collider2D Collider { get; protected set; }

    public virtual void SetupModel(MeteorModel model) {
        this.model = model;
    }

    public abstract void Launch(Vector2 position, Vector2 direction);

    public abstract void Hit(float damage);

    public abstract void OnCreate(object options);

    public abstract void ExcludeFromGame();
}
