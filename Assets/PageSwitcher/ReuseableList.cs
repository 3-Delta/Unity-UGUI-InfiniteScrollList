using System;
using System.Collections.Generic;
using UnityEngine;

public interface IReuseable {
    void Reset();
}

// 一般工作在框架层
public class ReuseableList<T> : IReuseable where T : IReuseable, new() {
    private readonly List<T> list = new List<T>();

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

    private ReuseableList<T> TryBuild(int targetCount, Action<T, int /* index */> onInit) {
        while (targetCount > this.Count) {
            T vd = new T();
            vd.Reset();
            onInit?.Invoke(vd, this.Count - 1);
            this.list.Add(vd);
        }

        return this;
    }

    private ReuseableList<T> TryRefresh(int targetCount, Action<T, int /* index */, bool /* toRefresh*/> onRrefresh) {
        this.RealCount = targetCount;
        int count = this.Count;
        for (int i = 0; i < count; ++i) {
            T cur = this.list[i];
            onRrefresh?.Invoke(cur, i, i < targetCount);
        }

        return this;
    }

    public ReuseableList<T> TryBuildOrRefresh(int targetCount, Action<T, int /* index */> onInit,
        Action<T, int /* index */, bool /* toRefresh*/> onRefresh) {
        this.TryBuild(targetCount, onInit);
        return this.TryRefresh(targetCount, onRefresh);
    }

    public void Reset() {
        for (int i = list.Count - 1; i >= 0; i--) {
            this.list[i].Reset();
        }

        this.list.Clear();
    }
}