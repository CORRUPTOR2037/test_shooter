using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Spaceship", menuName = "Spaceship Model")]
public class SpaceshipModel : ScriptableObject
{

    public Sprite Image;

    public float Speed;

    public float RotationSpeed = 100;

    public float ShootRate;

    public float LinearDrag;

    public int HealthPoints;

    public Vector2[] GunPoints;

    public Vector2[] EnginePoints;

}
