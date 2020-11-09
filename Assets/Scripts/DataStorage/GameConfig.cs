
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameConfig {

    [NonSerialized]
    private static GameConfig _Current = null;
    public static GameConfig Current {
        get {
            if (_Current == null) _Current = Load();
            return _Current;
        }
    }

    [NonSerialized]
    public LevelConfig LevelConfig;

    [SerializeField]
    private int SpaceshipIndex = 0;
    public SpaceshipModel Spaceship {
        get {
            return EntitiesData.Storage.SpaceshipsModels[SpaceshipIndex];
        }
        set {
            int index = EntitiesData.Storage.SpaceshipsModels.IndexOf(value);
            if (index < 0)
                throw new KeyNotFoundException("Spaceship model should be contained in Entities Data to be stored");

            SpaceshipIndex = index;
        }
    }

    [SerializeField]
    private int ProjectilesIndex = 0;
    public ProjectileModel Projectiles {
        get {
            return EntitiesData.Storage.ProjectileModels[ProjectilesIndex];
        }
        set {
            int index = EntitiesData.Storage.ProjectileModels.IndexOf(value);
            if (index < 0)
                throw new KeyNotFoundException("Projectile type model should be contained in Entities Data to be stored");

            ProjectilesIndex = index;
        }
    }

    [SerializeField]
    public int LastLevelCompleted = -1;

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
        bf.Serialize(file, _Current != null ? _Current : new GameConfig());
        file.Close();
    }

    public static GameConfig Load() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        try {
            file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            GameConfig cfg = (GameConfig)bf.Deserialize(file);
            file.Close();
            return cfg;
        } catch (Exception e) {
            return new GameConfig();
        }
    }

    public static void MarkLevelAsCompleted(LevelConfig level) {
        if (level.Completed) return;
        Current.LastLevelCompleted = Mathf.Max(Current.LastLevelCompleted, level.Index);
        Current.Save();
    }
}