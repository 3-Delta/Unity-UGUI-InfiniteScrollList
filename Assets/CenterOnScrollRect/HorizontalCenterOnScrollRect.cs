using System;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalCenterOnScrollRect : CenterOnScrollRect {
    protected override bool ReadyCenterOn {
        get {
            return scrollRect.velocity.x <= StopSpeed;
        }
    }
    protected override bool ReadyStop {
        get {
            return Mathf.Abs(scrollRect.velocity.x) == 0f;
        }
    }
}
