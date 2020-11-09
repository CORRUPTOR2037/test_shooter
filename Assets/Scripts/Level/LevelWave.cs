using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class LevelWave {

    public List<int> MeteorSpawners = new List<int>();

    public int Count = 10;

    public float StartDelay = 0;

    public FloatRange Rate = new FloatRange(3, 5);

    public List<MeteorModel> MeteorTypes = new List<MeteorModel>();

}