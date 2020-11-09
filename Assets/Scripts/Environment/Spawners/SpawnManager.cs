using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private Dictionary<int, BaseSpawnPlace> MeteorSpawnPlaces;
    private Dictionary<MeteorModel, ObjectPool<BaseMeteor>> MeteorPools;

    [SerializeField] private Transform MeteorContainer;

    public BaseSpawnPlace PlayerSpawnPlace;

    private MeteorEnumerator Enumerator;
    public IEnumerable Meteors {
        get { return Enumerator; }
    }

    private void Awake() {
        RescanSpawnPlaces();
        MeteorPools = new Dictionary<MeteorModel, ObjectPool<BaseMeteor>>();
        Enumerator = new MeteorEnumerator(MeteorPools);
    }

    private void RescanSpawnPlaces() {
        MeteorSpawnPlaces = new Dictionary<int, BaseSpawnPlace>();
        PlayerSpawnPlace = null;

        var placesObjects = FindObjectsOfType<BaseSpawnPlace>();
        if (placesObjects.Length == 0)
            throw new UnityException("Not spawners found");

        foreach (var place in placesObjects) {
            if (place.gameObject.tag == "Player")
                PlayerSpawnPlace = place;
            else {
                MeteorSpawnPlaces[place.Index] = place;
            }
        }

        if (PlayerSpawnPlace == null)
            throw new UnityException("Player spawn place not found");
    }

    public void LaunchWave(LevelWave wave) {
        StartCoroutine(SpawnMeteorsCoroutine(wave));
    }

    private IEnumerator SpawnMeteorsCoroutine(LevelWave wave) {

        yield return new WaitForSeconds(wave.StartDelay);

        int Count = wave.Count;

        while (Count != 0) {
            int nextSpawnerIndex = wave.MeteorSpawners[UnityEngine.Random.Range(0, wave.MeteorSpawners.Count)];
            Vector2 nextPosition = MeteorSpawnPlaces[nextSpawnerIndex].NextPosition();

            MeteorModel nextModel = wave.MeteorTypes[UnityEngine.Random.Range(0, wave.MeteorTypes.Count)];

            BaseMeteor meteor = GetFreeMeteor(nextModel);

            meteor.Launch(nextPosition, (Vector2) LevelController.Current.Spaceship.transform.position - nextPosition);

            yield return new WaitForSeconds(1f / wave.Rate.RandomValue);
            if (Count > 0) Count--;
        }
    }

    private BaseMeteor GetFreeMeteor(MeteorModel model) {
        if (!MeteorPools.ContainsKey(model)) {
            MeteorPools[model] = new ObjectPool<BaseMeteor>(() => MeteorFactory.Create(model), container: MeteorContainer);
        }
        return MeteorPools[model].GetFree();
    }

    public void Reset() {
        StopAllCoroutines();
        foreach (BaseMeteor meteor in Meteors) {
            meteor.ExcludeFromGame();
        }
    }

    class MeteorEnumerator : IEnumerable {

        Dictionary<MeteorModel, ObjectPool<BaseMeteor>> mp;
        public MeteorEnumerator(Dictionary<MeteorModel, ObjectPool<BaseMeteor>> mp) { this.mp = mp; }

        public IEnumerator GetEnumerator() {
            foreach (var key in mp.Keys) {
                foreach (var value in mp[key]) {
                    if (!value.IsFreeToReuse)
                        yield return value;
                }
            }
        }
    }
}
