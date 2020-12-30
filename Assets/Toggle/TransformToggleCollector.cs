using System.Collections.Generic;
using UnityEngine;

public class TransformToggleCollector : MonoBehaviour {
    private List<TransformToggle> cps = new List<TransformToggle>();

    public void RegisterToggle(TransformToggle cpt) {
        if (!cps.Contains(cpt)) {
            cps.Add(cpt);
        }
    }

    public void UnregisterToggle(TransformToggle cpt) {
        cps.Remove(cpt);
    }

    // UI表现
    // 如果传递null, 那么相当于全部!toSwicth,可以妙用
    public void SwitchTo(TransformToggle tt, bool toSwicth) {
        for (int i = 0, length = cps.Count; i < length; i++) {
            cps[i].ShowHideBySetActive(!toSwicth);
        }

        if (tt != null) {
            tt.ShowHideBySetActive(!toSwicth);
        }
    }
}