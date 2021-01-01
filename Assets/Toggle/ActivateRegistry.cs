using System.Collections.Generic;
using UnityEngine;

public class ActivateRegistry : MonoBehaviour {
    private List<Activate> cps = new List<Activate>();

    public void RegisterToggle(Activate cpt) {
        if (!cps.Contains(cpt)) {
            cps.Add(cpt);
        }
    }

    public void UnregisterToggle(Activate cpt) {
        cps.Remove(cpt);
    }

    // UI表现
    // 如果传递null, 那么相当于全部!toSwicth,可以妙用
    public void SwitchTo(Activate tt, bool toSwicth) {
        for (int i = 0, length = cps.Count; i < length; i++) {
            cps[i].ShowHideBySetActive(!toSwicth);
        }

        if (tt != null) {
            tt.ShowHideBySetActive(!toSwicth);
        }
    }
}
