using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {

    [SerializeField]
    private Text text;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button returnButton;

    private void Start() {
        MessageBroker.Default.Receive<LevelMessage>()
            .Subscribe(msg => ReactOnGameEvent(msg.Type));

        restartButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
            MessageBroker.Default.Publish<LevelMessage>(new LevelMessage(LevelMessage.MessageType.CallToRestart));
        });

        returnButton.onClick.AddListener(() => {
            SceneChanger.OpenScene(Scene.SelectLevel);
        });

        gameObject.SetActive(false);
    }

    private void ReactOnGameEvent(LevelMessage.MessageType type) {
        switch (type) {
            case LevelMessage.MessageType.GameLose: {
                gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                returnButton.gameObject.SetActive(true);

                text.text = "You lost.";
            } break;

            case LevelMessage.MessageType.GameWin: {
                gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                returnButton.gameObject.SetActive(true);

                text.text = "You won!";
            }
            break;
        }
    }
}
