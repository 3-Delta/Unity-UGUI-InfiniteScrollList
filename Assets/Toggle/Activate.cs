using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activate : MonoBehaviour {
    public List<Transform> actives = new List<Transform>();
    public List<Transform> deactives = new List<Transform>();

    [SerializeField] private ActivateRegistry _collector;
    public ActivateRegistry Collector {
        get {
            if (_collector == null) {
                _collector = GetComponentInParent<ActivateRegistry>();
            }

            return _collector;
        }
    }
    
    protected void OnEnable() {
        if (Collector != null) {
            Collector.RegisterToggle(this);
        }
    }

    protected void OnDisable() {
        if (Collector != null) {
            Collector.UnregisterToggle(this);
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
