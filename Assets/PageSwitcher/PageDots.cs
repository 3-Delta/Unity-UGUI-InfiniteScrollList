using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageDots : MonoBehaviour {
    public COWTransform cow;

    public Action<Transform, int> onSelected;

    private void Awake() {
        if (cow == null) {
            if (!TryGetComponent(out cow)) {
                cow = gameObject.AddComponent<COWTransform>();
            }
        }
    }

    public PageDots TryBuildOrRefresh(int targetCount, Action<Transform, int /* index */> onInit, Action<Transform, int /* index */> onRrfresh, ref int currentIndex) {
        cow.TryBuildOrRefresh(targetCount, onInit, onRrfresh);
        SwitchTo(ref currentIndex);
        return this;
    }

    public bool SwitchTo(ref int currentIndex) {
        if (0 <= currentIndex && currentIndex < cow.RealCount) {
            onSelected?.Invoke(cow[currentIndex], currentIndex);
            return true;
        }
        else {
            currentIndex = -1;
            return false;
        }
    }
}
