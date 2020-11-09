using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipGun : MonoBehaviour
{
    private SpaceshipModel model;
    private ObjectPool<BaseProjectileController> projectiles;

    public void Setup(SpaceshipModel model, ObjectPool<BaseProjectileController> projectilesPool) {
        this.model = model;
        this.projectiles = projectilesPool;
    }

    public void Shoot() {
        BaseProjectileController nextProjectile = projectiles.GetFree();
        nextProjectile.Launch(this.transform.position, this.transform.up);
    }
}
