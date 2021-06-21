using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 左右箭头切换
public class PageSwitcher : MonoBehaviour {
    public enum ETravelMode {
        Circle, // 循环
        CircleRightOnly, // 右侧循环
        CircleLeftOnly, // 右侧循环
        NoCircle, // 两侧边界模式
    }

    public ETravelMode mode = ETravelMode.Circle;
    public Button leftArrow;
    public Button rightArrow;

    [SerializeField, Range(1, 50)] private int countPerPage = 1;

    // index:startIndex:rangeCount
    public Action<int, int, int> onSwitch;

    public int DataCount { get; private set; }

    public int PageCount {
        get { return Mathf.CeilToInt(1f * DataCount / countPerPage); }
    }

    public int CurrentPageIndex { get; private set; } = 0;

    protected void Awake() {
        if (leftArrow != null) {
            void OnBtnLeftClicked() {
                --CurrentPageIndex;
                Switch();
            }

            leftArrow.onClick.AddListener(OnBtnLeftClicked);
        }

        if (rightArrow != null) {
            void OnBtnRightClicked() {
                ++CurrentPageIndex;
                Switch();
            }

            rightArrow.onClick.AddListener(OnBtnRightClicked);
        }
    }

    // 设置个数
    public PageSwitcher SetCount(uint count) {
        DataCount = (int) count;
        return this;
    }

    // 设置currentIndex
    public bool SwitchTo(ref int currentIndex) {
        if (0 <= currentIndex && currentIndex < PageCount) {
            this.CurrentPageIndex = currentIndex;
            this.Switch();
            return true;
        }
        else {
            this.CurrentPageIndex = currentIndex = -1;
            return false;
        }
    }

    private void Switch() {
        CurrentPageIndex = (CurrentPageIndex + PageCount) % PageCount;

        if (mode == ETravelMode.NoCircle) {
            leftArrow.gameObject.SetActive(CurrentPageIndex != 0);
            rightArrow.gameObject.SetActive(CurrentPageIndex != PageCount - 1);
        }
        else if (mode == ETravelMode.CircleRightOnly) {
            leftArrow.gameObject.SetActive(CurrentPageIndex != 0);
        }
        else if (mode == ETravelMode.CircleLeftOnly) {
            rightArrow.gameObject.SetActive(CurrentPageIndex != PageCount - 1);
        }

        int startIndex = CurrentPageIndex * countPerPage;
        int rangeCount = Math.Min(countPerPage, DataCount - startIndex);
        onSwitch?.Invoke(CurrentPageIndex, startIndex, rangeCount);
    }
}
