using System;
using UnityEngine;

// 参考NGUI的springPanel
public class SpringTo : MonoBehaviour {
    public Action onFinished;

    [SerializeField] private RectTransform _toSpring;

    public RectTransform toSpring {
        get {
            if (_toSpring == null) {
                _toSpring = transform as RectTransform;
            }

            return _toSpring;
        }
    }

    [Header("目标世界位置")] public Vector3 toWorldPosition;
    [Header("强度")] public float strength = 10f;

    private void LateUpdate() {
        ToPosition();
    }

    protected virtual void ToPosition() {
        float deltaTime = Time.deltaTime;
        bool trigger = false;
        Vector3 before = toSpring.position;
        Vector3 after = SpringLerp(before, toWorldPosition, strength, deltaTime);
        float distance = (after - toWorldPosition).sqrMagnitude;
        if (distance < 0.01f) {
            // 到达最终的 目标位置
            after = toWorldPosition;

            enabled = false;
            trigger = true;
        }

        // 设置位置
        toSpring.position = after;
        if (trigger) {
            onFinished?.Invoke();
        }
    }

    public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime) {
        return Vector3.Lerp(from, to, SpringLerp(strength, deltaTime));
    }

    public static float SpringLerp(float strength, float deltaTime) {
        if (deltaTime > 1f) {
            deltaTime = 1f;
        }

        int ms = Mathf.RoundToInt(deltaTime * 1000f);
        deltaTime = 0.001f * strength;
        float cumulative = 0f;
        for (int i = 0; i < ms; ++i) {
            cumulative = Mathf.Lerp(cumulative, 1f, deltaTime);
        }

        return cumulative;
    }

    public static SpringTo Begin(Transform target, Vector3 targetWorldPosition, Action onFinished) {
        if (!target.TryGetComponent<SpringTo>(out SpringTo springTo)) {
            springTo = target.gameObject.AddComponent<SpringTo>();
        }

        springTo.toWorldPosition = targetWorldPosition;
        springTo.onFinished = onFinished;
        springTo.enabled = true;
        return springTo;
    }
}