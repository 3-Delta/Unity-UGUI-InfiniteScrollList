using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 单纯的以scrollrect的content为基准做centerOn
public class CenterOnScrollRect : MonoBehaviour
{
    public RectTransform cellProto;
    public bool enableWhenScrolling = true;
    public Vector2 MinScale = new Vector2(0.7f, 0.7f);
    public Vector2 Shrinkage = new Vector2(0.005f, 0.005f);
    [Header("低于该速度，才会开始centerOn流程")] public float StopSpeed = 60f;
    [Header("居中点")] public RectTransform center;

    [SerializeField] private ScrollRect _scrollRect;
    public ScrollRect scrollRect {
        get {
            if (_scrollRect == null) {
                _scrollRect = GetComponent<ScrollRect>();
            }
            return _scrollRect;
        }
    }

    public Button btnLeft;
    public Button btnRight;

    public Transform Focused { get; protected set; } = null;
    public Transform Nearest { get; protected set; } = null;
    
    public Action<Transform> onCenter;
    protected IList<Transform> contentChildren = new List<Transform>();
    
    protected float[] distReposition;
    protected float[] distance;

    protected virtual bool ReadyCenterOn {
        get {
            return true;
        }
    }
    protected virtual bool ReadyStop {
        get {
            return false;
        }
    }

    public int NearestIndex {
        get {
            return contentChildren.IndexOf(Nearest);
        }
    }

    protected virtual void Awake() {
        if (btnLeft != null) {
            btnLeft.onClick.AddListener(OnBtnLeftClicked);
        }

        if (btnRight != null) {
            btnRight.onClick.AddListener(OnBtnRightClicked);
        }
        
        scrollRect.onValueChanged.AddListener(OnScrollRectNormalizedPositionChange);
    }
    
    private void OnScrollRectNormalizedPositionChange(Vector2 normalizedPosition) {
        enabled = enableWhenScrolling;
    }

    protected virtual void Start() {
        CollectChildren();

        int length = contentChildren.Count;
        distance = new float[length];
        distReposition = new float[length];
        Focused = null;
        Nearest = null;
    }

    public void CollectChildren() {
        contentChildren.Clear();
        for (int i = 0, length = scrollRect.content.childCount; i < length; ++i) {
            Transform child = scrollRect.content.GetChild(i);
            contentChildren.Add(child);
        }
    }

    protected virtual void OnBtnLeftClicked() { }

    protected virtual void OnBtnRightClicked() { }

    public void CenterOnNearest() {
        CenterOn(Nearest);
    }

    // 吸附到特定的item
    public void CenterOn(Transform target) {
        if (target == null) {
            return;
        }

        // 设置聚焦点
        Focused = target;
        
        // 将center转换为 以content为基准的情形下的一个localPosition
        Vector3 relativeLocalPositionTarget = scrollRect.content.InverseTransformPoint(target.position);
        Vector3 relativeLocalPositionCenter = scrollRect.content.InverseTransformPoint(center.position);
        // 都在content下, 和center和item的位置差值
        Vector3 localOffset = relativeLocalPositionTarget - relativeLocalPositionCenter;
        localOffset.z = 0f;
        
        SpringTo.Begin(scrollRect.content, scrollRect.content.transform.position - localOffset, () => {
            onCenter?.Invoke(Focused);
        });
    }
}
