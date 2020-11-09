using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory { 

    public static BaseProjectileController CreateProjectile(ProjectileModel model) {
        var projectile = UnityEngine.Object.Instantiate(model.Prefab);
        projectile.SetupModel(model);
        return projectile;
    }

    public static BaseVisualEffect CreateOnHitEffect(ProjectileModel model) {
        return UnityEngine.Object.Instantiate(model.OnHitEffectPrefab);
    }
}
