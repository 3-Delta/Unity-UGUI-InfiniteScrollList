using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfiniteListItem : InfiniteListItem {
    public Button button;

    protected override void Awake() {
        base.Awake();
        if (button == null) {
            button = GetComponent<Button>();
        }

        if (button != null) {
            button.onClick.AddListener(OnBtnClicked);
        }
    }

    private void OnBtnClicked() {
        if (toggle != null && toggle.Collector != null) {
            toggle.Collector.SwitchTo(toggle, true);
        }

        if (infiniteList != null) {
            infiniteList.OnItemSelected?.Invoke(groupIndex, index, this);
        }
    }
}