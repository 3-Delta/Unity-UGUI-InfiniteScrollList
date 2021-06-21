using System;
using System.Collections.Generic;
using UnityEngine;

// 一般工作在框架层
public class COWComponent<T> : MonoBehaviour where T : Component {
    private readonly List<T> components = new List<T>();

    public T proto;
    public Transform parent;

    public int Count {
        get { return this.components.Count; }
    }

    public int RealCount { get; private set; }

    public T this[int index] {
        get {
            if (0 <= index && index < this.Count) {
                return this.components[index];
            }

            return null;
        }
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

            this.components.Add(t);
        }

        return this;
    }

    private COWComponent<T> TryRefresh(int targetCount, Action<T, int /* index */> onRrefresh) {
        this.RealCount = targetCount;
        int componentCount = this.Count;
        for (int i = 0; i < componentCount; ++i) {
            T cur = this.components[i];
            if (i < targetCount) {
                cur.gameObject.SetActive(true);
                onRrefresh?.Invoke(cur, i);
            }
            else {
                cur.gameObject.SetActive(false);
            }
        }

        return this;
    }

    public COWComponent<T> TryBuildOrRefresh(int targetCount, Action<T, int /* index */> onInit, Action<T, int /* index */> onRrfresh) {
        this.TryBuild(targetCount, onInit);
        return this.TryRefresh(targetCount, onRrfresh);
    }

    private void Awake() {
        proto.gameObject.SetActive(false);
        if (parent == null) {
            parent = transform;
        }
    }

    // private void OnDestroy() {
    //     this.components.Clear();
    // }
}

public class COWTransform : COWComponent<Transform> { }
