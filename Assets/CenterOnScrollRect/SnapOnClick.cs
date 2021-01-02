using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// 参考NGUI CenterOnClick的实现
public class SnapOnClick : MonoBehaviour
{
    [SerializeField] private Button _button;
    public Button button {
        get {
            if (_button == null) {
                _button = GetComponent<Button>();
            }

            return _button;
        }
    }

    [SerializeField] private SnapOnChild snapOn;
    public SnapOnChild SnapOn {
        get {
            if (snapOn == null) {
                snapOn = GetComponentInParent<SnapOnChild>();
            }

            return snapOn;
        }
    }

    private void Awake() {
        if (button != null) {
            button.onClick.AddListener(OnBtnClicked);
        }
    }

    private void OnBtnClicked() {
        Snap();
    }

    public void Snap() {
        SnapOn.TrySnapOn(transform);
    }
}
