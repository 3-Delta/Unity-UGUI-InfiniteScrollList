using System;
using System.Collections.Generic;
using UnityEngine;

// 一般工作在框架层
public class COWComponent<T> : MonoBehaviour where T : Component {
    private readonly List<T> list = new List<T>();

    public T proto;
    public Transform parent;

    public int Count {
        get { return this.list.Count; }
    }

    public int RealCount { get; private set; }

    public T this[int index] {
        get {
            if (0 <= index && index < this.Count) {
                return this.list[index];
            }

            return default;
        }
    }

    public void ForEach(Action<T> onForeach) {
        this.list.ForEach(onForeach);
    }

    private COWComponent<T> TryBuild(int targetCount, Action<T, int /* index */> onInit) {
        while (targetCount > this.Count) {
            GameObject clone = GameObject.Instantiate<GameObject>(proto.gameObject, parent);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localEulerAngles = Vector3.zero;
            clone.transform.localScale = Vector3.one;
            clone.SetActive(false);

            T t = clone.GetComponent<T>();
            onInit?.Invoke(t, this.Count - 1);

            this.list.Add(t);
        }

        return this;
    }

    private COWComponent<T> TryRefresh(int targetCount, Action<T, int /* index */> onRefresh) {
        this.RealCount = targetCount;
        int count = this.Count;
        for (int i = 0; i < count; ++i) {
            T cur = this.list[i];
            if (i < targetCount) {
                cur.gameObject.SetActive(true);
                onRefresh?.Invoke(cur, i);
            }
            else {
                cur.gameObject.SetActive(false);
            }
        }

        return this;
    }

    public COWComponent<T> TryBuildOrRefresh(int targetCount, Action<T, int /* index */> onInit,
        Action<T, int /* index */> onRefresh) {
        this.TryBuild(targetCount, onInit);
        return this.TryRefresh(targetCount, onRefresh);
    }

    private void Awake() {
        proto.gameObject.SetActive(false);
        if (parent == null) {
            parent = transform;
        }
    }
}

public class COWTransform : COWComponent<Transform> {
}