using System;
using UnityEngine;
using UnityEngine.UI;

// 参考NGUI CenterOnClick的实现
public class CenterOnClick : MonoBehaviour
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

    [SerializeField] private CenterOnScrollRect _centerOn;
    public CenterOnScrollRect centerOn {
        get {
            if (_centerOn == null) {
                _centerOn = GetComponentInParent<CenterOnScrollRect>();
            }
            return _centerOn;
        }
    }

    private void Awake() {
        if (button != null) {
            button.onClick.AddListener(OnBtnClicked);
        }
    }

    private void OnBtnClicked() {
        SetCenter();
    }

    public void SetCenter() {
        centerOn.CenterOn(transform);
    }
}
