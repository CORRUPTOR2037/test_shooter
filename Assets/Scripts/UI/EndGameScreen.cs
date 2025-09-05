using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{

    [SerializeField] private Text text;
    [SerializeField] private Button restartButton, returnButton;
    [SerializeField] private Transform screen;

    bool gameEnded = false;

    private void Start()
    {
        MessageBroker.Default.Receive<LevelMessage>()
            .Subscribe(msg => ReactOnGameEvent(msg.Type)).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Subscribe(_ => Pause()).AddTo(this);


        restartButton.onClick.AddListener(() =>
        {
            screen.gameObject.SetActive(false);
            MessageBroker.Default.Publish<LevelMessage>(new LevelMessage(LevelMessage.MessageType.CallToRestart));
        });

        returnButton.onClick.AddListener(() =>
        {
            SceneChanger.OpenScene(Scene.MainMenu);
        });

        screen.gameObject.SetActive(false);
    }

    private void ReactOnGameEvent(LevelMessage.MessageType type)
    {
        switch (type)
        {
            case LevelMessage.MessageType.GameLose:
                {
                    gameEnded = true;
                    screen.gameObject.SetActive(true);
                    restartButton.gameObject.SetActive(true);
                    returnButton.gameObject.SetActive(true);

                    text.text = "You lost.";
                }
                break;

            case LevelMessage.MessageType.GameWin:
                {
                    gameEnded = true;
                    screen.gameObject.SetActive(true);
                    restartButton.gameObject.SetActive(false);
                    returnButton.gameObject.SetActive(true);

                    text.text = "You won!";
                }
                break;
        }
    }

    private void Pause()
    {
        if (gameEnded) return;

        screen.gameObject.SetActive(!screen.gameObject.activeSelf);
        restartButton.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);

        text.text = "Pause";
    }
    

}
