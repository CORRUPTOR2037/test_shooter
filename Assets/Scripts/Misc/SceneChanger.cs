using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene {
    Game,
    SelectLevel,
    MainMenu,
    SetupShip

}
public class SceneChanger : MonoBehaviour {

    public static void OpenScene(Scene scene) {
        SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }

    public void CallOpenScene(string text) {
        SceneManager.LoadScene(text, LoadSceneMode.Single);
    }


    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
