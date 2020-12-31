using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformToggle : MonoBehaviour {
    public bool playOnEnable = false;
    public List<Transform> actives = new List<Transform>();
    public List<Transform> deactives = new List<Transform>();

    private TransformToggleCollector _collector;
    public TransformToggleCollector Collector {
        get {
            if (_collector == null) {
                _collector = GetComponentInParent<TransformToggleCollector>();
            }

            return _collector;
        }
    }
    
    protected void OnEnable() {
        if (Collector != null) {
            _collector.RegisterToggle(this);
        }

        if (playOnEnable) {
            ShowHideBySetActive(true);
        }
    }

    protected void OnDisable() {
        if (Collector != null) {
            _collector.UnregisterToggle(this);
        }
        
        if (playOnEnable) {
            ShowHideBySetActive(false);
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