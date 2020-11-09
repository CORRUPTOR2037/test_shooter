using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<SpaceshipController>() != null) {
            MessageBroker.Default.Publish<LevelMessage>(new LevelMessage(LevelMessage.MessageType.GameWin));
            gameObject.SetActive(false);
        }
    }
}
