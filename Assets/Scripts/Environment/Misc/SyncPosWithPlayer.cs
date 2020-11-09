using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPosWithPlayer : MonoBehaviour
{
    public bool X = true;
    public bool Y = true;
    public bool Z = true;

    private void Update()
    {
        if (LevelController.Current.Spaceship && (X || Y || Z))
            this.transform.position = new Vector3(
                X ? LevelController.Current.Spaceship.transform.position.x : this.transform.position.x,
                Y ? LevelController.Current.Spaceship.transform.position.y : this.transform.position.y,
                Z ? LevelController.Current.Spaceship.transform.position.z : this.transform.position.z
            );
    }
}
