using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolItem {

    bool IsFreeToReuse { get; }

    void OnCreate(object options);

}

[System.Serializable]
public class ObjectPool<T> : List<T> where T : MonoBehaviour, IObjectPoolItem {

    private object initOptions;
    private System.Func<T> sourcePrefabCreator;
    private LinkedList<int> prefabsUsageOrder = new LinkedList<int>();
    private Transform parentContainer;

    public bool FixedSize { get; set; }

    public int MaxCount { get; set; }

    private ObjectPool (int count, Transform container, object options) {
        this.initOptions = options;

        this.parentContainer = container;
        if (container == null || container.gameObject == null) this.parentContainer = null;

        if (count > 0) {
            FixedSize = true;
            MaxCount = count;
        } else {
            FixedSize = false;
        }
    }

    public ObjectPool (T sourcePrefab, int count = -1, Transform container = null, object options = null) : this(count, container, options) {
        if (sourcePrefab == null)
            throw new System.Exception("Source prefab provided to ObjectPool is null");

        this.sourcePrefabCreator = () => Object.Instantiate(sourcePrefab);
    }

    public ObjectPool(System.Func<T> sourcePrefabCreator, int count = -1, Transform container = null, object options = null) : this(count, container, options) {

        if (sourcePrefabCreator == null)
            throw new System.Exception("Source prefab creator provided to ObjectPool is null");

        this.sourcePrefabCreator = sourcePrefabCreator;
    }

    public void Preload() {
        Spawn();
        for (int i = 1; i < MaxCount; i++) {
            Spawn();
        }
    }

    protected T Spawn() {
        T obj = sourcePrefabCreator();
        obj.gameObject.SetActive(false);
        obj.OnCreate(initOptions);

        obj.transform.parent = parentContainer;
        Add(obj);

        return obj;
    }

    public void Resize(int value) {
        if (Count == value) return;

        if (Count > value) {
            for (int i = value; i < Count; i++) {
                this[i].gameObject.SetActive(false);
            }
            return;
        }

        if (Count < value) {
            for (int i = Count; i < value; i++) {
                Spawn();
            }
        }
    }

    public T GetFree() {
        T obj;
        for (int i = 0; i < Count; i++) {
            obj = this[i];
            if (!obj.IsFreeToReuse) continue;
            prefabsUsageOrder.Remove(i);
            prefabsUsageOrder.AddFirst(i);
            return obj;
        }

        if (!FixedSize || (FixedSize && Count < MaxCount)) {
            obj = Spawn();
            prefabsUsageOrder.AddFirst(Count - 1);
            return obj;
        }

        int index = prefabsUsageOrder.Last.Value;
        obj = this[index];
        prefabsUsageOrder.RemoveLast();
        prefabsUsageOrder.AddFirst(index);
        return obj;
    }

    public void Destroy() {
        foreach (T prefab in this) {
            if (prefab && prefab.gameObject)
                Object.Destroy(prefab.gameObject);
        }
    }

    ~ObjectPool() {
        Destroy();
    }
}
