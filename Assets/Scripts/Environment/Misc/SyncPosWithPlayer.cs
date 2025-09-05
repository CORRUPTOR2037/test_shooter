using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SyncPosWithPlayer : MonoBehaviour
{
    public bool X = true;
    public bool Y = true;
    public bool Z = true;
    public bool smooth = true;

    private void Start()
    {
        MessageBroker.Default.Receive<LevelMessage>()
            .Where(msg => msg.Type == LevelMessage.MessageType.CallToRestart)
            .Subscribe(_ => Reset()).AddTo(this);
    }

    Vector3 newPos;
    private void FixedUpdate()
    {
        if (LevelController.Current.Spaceship && (X || Y || Z))
        {
            newPos = new Vector3(
                X ? LevelController.Current.Spaceship.transform.position.x : this.transform.position.x,
                Y ? LevelController.Current.Spaceship.transform.position.y : this.transform.position.y,
                Z ? LevelController.Current.Spaceship.transform.position.z : this.transform.position.z
            );
            if (smooth)
                transform.position = Vector3.MoveTowards(transform.position, newPos, Vector3.Distance(transform.position, newPos) * Time.deltaTime);
            else
                transform.position = newPos;
        }
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
    }
}
