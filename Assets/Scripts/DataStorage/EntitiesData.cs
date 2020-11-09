using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesData : MonoBehaviour
{
    private static EntitiesData _Storage;
    public static EntitiesData Storage {
        get {
            if (_Storage == null)
                _Storage = Resources.Load<EntitiesData>("EntitiesData");
            return _Storage;
        }
    }

    public List<LevelConfig> LevelConfigs;

    public SpaceshipController SpaceshipBase;
    public SpaceshipGun SpaceshipGun;
    public SpaceshipEngine SpaceshipEngine;
    public List<SpaceshipModel> SpaceshipsModels;

    public List<ProjectileModel> ProjectileModels;

    public BaseMeteor MeteorBase;
    public List<MeteorModel> MeteorModels;
}
