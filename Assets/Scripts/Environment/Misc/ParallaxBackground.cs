using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float followStrength = 0.1f;
    [SerializeField] float zPos = 0;

    Vector3 _pos;

    void Update()
    {
        if (LevelController.Current.Spaceship) {
            _pos.x = -LevelController.Current.Spaceship.transform.position.x * followStrength;
            _pos.y = -LevelController.Current.Spaceship.transform.position.y * followStrength;
            _pos.z = zPos;
            this.transform.localPosition = _pos;
        }
    }
}
