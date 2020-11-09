using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenLevelButton : MonoBehaviour
{
    [SerializeField]
    private LevelConfig levelConfig;

    private void Awake() {
        if (levelConfig == null) {
            enabled = false;
            return;
        }

        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadGame);
        button.interactable = levelConfig.Opened;
    }

    public void LoadGame() {
        GameConfig.Current.LevelConfig = levelConfig;
        SceneChanger.OpenScene(Scene.Game);
    }
}
