using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meteor", menuName = "MeteorModel")]
public class MeteorModel : ScriptableObject
{
    public Sprite Image;

    public Color Tint = Color.white;

    public FloatRange Scale = new FloatRange(1, 1);

    public float Health = 3;

    public FloatRange Mass = new FloatRange(1, 1);

    public FloatRange Speed = new FloatRange(1, 1);

}
