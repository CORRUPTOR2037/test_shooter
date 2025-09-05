using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LevelController : MonoBehaviour, IModelBased<LevelConfig>
{

    public const int MAX_SPAWNERS_ON_LEVEL = 3;

    public static LevelController Current;

    protected LevelConfig Config;

    protected MeteorSpawnManager spawner;
    protected EndGameTrigger endGameTrigger;
    public Transform ProjectilesContainer, EffectsContainer, MeteorsContainer;

    public SpaceshipController Spaceship { get; protected set; }

    public float Distance => Config.Distance;

    public IEnumerable Meteors => spawner.Meteors;

    public void SetupModel(LevelConfig model)
    {
        this.Config = model;
        endGameTrigger.transform.position = new Vector3(0, Distance, 0);
    }

    protected virtual void Awake()
    {
        spawner = FindObjectOfType<MeteorSpawnManager>();
        endGameTrigger = FindObjectOfType<EndGameTrigger>();
    }

    protected virtual void Start() {

        Current = this;

        if (GameConfig.Current.LevelConfig == null)
            GameConfig.Current.LevelConfig = EntitiesData.Storage.LevelConfigs[0];

        SetupModel(GameConfig.Current.LevelConfig);

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => {
                foreach (BaseMeteor meteor in spawner.Meteors) {
                    if (Vector3.Distance(Spaceship.transform.position, meteor.transform.position) > 40)
                        meteor.ExcludeFromGame();
                }
            }).AddTo(this);

        MessageBroker.Default.Receive<SpaceshipMessage>()
            .Where(msg => msg.Type == SpaceshipMessage.MessageType.Died)
            .Subscribe(_ => EndGame());

        MessageBroker.Default.Receive<LevelMessage>()
            .Where(msg => msg.Type == LevelMessage.MessageType.GameWin)
            .Subscribe(_ => OnCompleted());

        MessageBroker.Default.Receive<LevelMessage>()
            .Where(msg => msg.Type == LevelMessage.MessageType.CallToRestart)
            .Subscribe(_ => RestartGame());

        Spaceship = SpaceshipFactory.CreateSpaceship(GameConfig.Current.Spaceship);

        RestartGame();
    }

    public void EndGame()
    {
        MessageBroker.Default.Publish<LevelMessage>(new LevelMessage(LevelMessage.MessageType.GameLose));
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Spaceship.transform.position = spawner.PlayerSpawnPlace.NextPosition();
        Spaceship.transform.eulerAngles = Vector3.zero;

        Spaceship.Reset();
        MessageBroker.Default.Publish<SpaceshipMessage>(new SpaceshipMessage(SpaceshipMessage.MessageType.Created));

        spawner.Reset();
        foreach (LevelWave wave in Config.Waves) {
            spawner.LaunchWave(wave);
        }

        endGameTrigger.gameObject.SetActive(true);
        endGameTrigger.transform.position = new Vector3(0, Distance, 0);

        Time.timeScale = 1;
    }

    private void OnCompleted() {
        GameConfig.MarkLevelAsCompleted(Config);
    }

    protected virtual void OnDestroy() {
        Current = null;
    }
}
