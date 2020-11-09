using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawnPlace : MonoBehaviour
{
    [SerializeField] int _Index;
    public int Index => _Index;

    public abstract Vector2 NextPosition();

}
