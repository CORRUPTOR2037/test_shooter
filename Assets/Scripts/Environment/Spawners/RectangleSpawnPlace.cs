using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSpawnPlace : BaseSpawnPlace {

    public Vector2 SpawnRectSize;

    public override Vector2 NextPosition() {
        return new Vector2(
            transform.position.x + Random.Range(0, SpawnRectSize.x * 2) - SpawnRectSize.x,
            transform.position.y + Random.Range(0, SpawnRectSize.y * 2) - SpawnRectSize.y
        );
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, SpawnRectSize);
    }
}
