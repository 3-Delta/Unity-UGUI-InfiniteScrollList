using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_TransformToggle : MonoBehaviour {
    public List<Transform> actives = new List<Transform>();
    public List<Transform> deactives = new List<Transform>();

    private CP_TransformToggleCollector _collector;
    public CP_TransformToggleCollector Collector {
        get {
            if (_collector == null) {
                _collector = GetComponentInParent<CP_TransformToggleCollector>();
            }

            return _collector;
        }
    }
    
    protected void OnEnable() {
        if (Collector != null) {
            _collector.RegisterToggle(this);
        }
    }

    protected void OnDisable() {
        if (Collector != null) {
            _collector.UnregisterToggle(this);
        }
    }

    public void ShowHideBySetActive(bool active) {
        foreach (Transform t in actives) {
            t.gameObject.SetActive(active);
        }

        foreach (Transform t in deactives) {
            t.gameObject.SetActive(!active);
        }
    }
}