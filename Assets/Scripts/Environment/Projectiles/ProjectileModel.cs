using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ProjectileModel")]
public class ProjectileModel : ScriptableObject
{

    public BaseProjectileController Prefab;

    public Sprite Image;

    public float Speed = 1;

    public FloatRange Damage = new FloatRange(1, 1);

    public float Lifetime = 5;

    public BaseVisualEffect OnHitEffectPrefab;

}
