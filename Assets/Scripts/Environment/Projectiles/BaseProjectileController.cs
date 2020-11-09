using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectileController : MonoBehaviour, IModelBased<ProjectileModel>, IObjectPoolItem {

    [SerializeField]
    protected ProjectileModel model;

    public bool IsFreeToReuse {
        get; protected set;
    }

    public abstract void OnCreate(object options);

    public abstract void SetupModel(ProjectileModel model);

    public abstract void Launch(Vector2 position, Vector2 direction);

}
