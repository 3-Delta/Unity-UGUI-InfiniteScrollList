using System.Collections.Generic;

using UnityEngine;

public class CP_TransformToggleCollector : MonoBehaviour {
    private List<CP_TransformToggle> cps = new List<CP_TransformToggle>();

    public void RegisterToggle(CP_TransformToggle cpt) {
        if (!cps.Contains(cpt)) {
            cps.Add(cpt);
        }
    }

    public void UnregisterToggle(CP_TransformToggle cpt) {
        cps.Remove(cpt);
    }

    // UI表现
    public void SwitchTo(CP_TransformToggle cpt, bool toSwicth) {
        for (int i = 0, length = cps.Count; i < length; i++) {
            cps[i].ShowHideBySetActive(!toSwicth);
        }
        cpt.ShowHideBySetActive(toSwicth);
    }
}