using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawnPlace : BaseSpawnPlace
{
    public override Vector2 NextPosition() { return transform.position; }

    void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}