using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysEntity
{
    Rigidbody2D Rigidbody { get; }

    Collider2D Collider { get; }
}
