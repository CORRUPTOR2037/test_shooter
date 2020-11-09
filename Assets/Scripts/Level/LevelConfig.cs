using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level", menuName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public float Distance = 100;

    public List<LevelWave> Waves;

    public int Index {
        get {
            return EntitiesData.Storage.LevelConfigs.IndexOf(this);
        }
    }

    public bool Opened {
        get {
            return Index <= GameConfig.Current.LastLevelCompleted + 1;
        }
    }

    public bool Completed {
        get {
            return Index <= GameConfig.Current.LastLevelCompleted;
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor {


    public override void OnInspectorGUI() {

        base.DrawDefaultInspector();

        LevelConfig config = (LevelConfig) target;

        if (GUILayout.Button("Randomize")){

            config.Distance = Random.Range(50, 500);

            config.Waves = new List<LevelWave>();

            for (int i = 0, l = Random.Range(1, 5); i < l; i++) {
                LevelWave wave = new LevelWave();
                config.Waves.Add(wave);

                for (int j = 0; j < LevelController.MAX_SPAWNERS_ON_LEVEL; j++)
                    if (Random.value > 0.5f) wave.MeteorSpawners.Add(j);
                if (wave.MeteorSpawners.Count == 0) wave.MeteorSpawners.Add(0);

                for (int j = 0; j < EntitiesData.Storage.MeteorModels.Count; j++)
                    if (Random.value > 0.5f) wave.MeteorTypes.Add(EntitiesData.Storage.MeteorModels[j]);
                if (wave.MeteorTypes.Count == 0) wave.MeteorTypes.Add(EntitiesData.Storage.MeteorModels[0]);

                if (Random.value > 0.5f)
                    wave.Count = -1;
                else
                    wave.Count = Random.Range(50, 300);

                wave.StartDelay = Random.Range(0, 50);
            }

            Resave();
        }
    }

    private void Resave() {
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif