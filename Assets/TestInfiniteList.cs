using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TestInfiniteList : MonoBehaviour {
    public RectTransform rt;
    public Vector2 offMin;
    public Vector2 offMax;
    public Vector3 anchorPosition;

    public VerticalInfiniteList vertical;
    public HorizontalInfiniteList horizontal;

    public VerticalCenterOnScrollRect verticalCenterOn;

    // vd是热更层的一个逻辑，这里模拟热更和框架的交互
    public class Vd {
        public Text nameText;
        public Image icon;

        public GameObject gameObject;

        public Vd Init(GameObject gameObject) {
            this.gameObject = gameObject;

            nameText = gameObject.GetComponentInChildren<Text>();
            icon = gameObject.GetComponentInChildren<Image>();
            return this;
        }

        public void Refresh(uint id) {
            nameText.text = id.ToString();
        }
        public void SetSelected() {
            Debug.LogError("SetSelected");
        }
    }

    private Dictionary<GameObject, Vd> vds = new Dictionary<GameObject, Vd>();
    private Dictionary<GameObject, Vd> vdss = new Dictionary<GameObject, Vd>();

    private void Awake() {
        rt = GetComponent<RectTransform>();

        // 测试热更新代码 获取 框架层代理
        if (vertical != null) {
            vertical.OnItemCreated = (item) => {
                if (!vds.TryGetValue(item.gameObject, out Vd vd)) {
                    vd = new Vd().Init(item.gameObject);
                    vds.Add(item.gameObject, vd);
                }
            };
            vertical.OnItemRefreshed = (groupIndex, index, id, item) => {
                if (vds.TryGetValue(item.gameObject, out Vd vd)) {
                    vd.Refresh(id);
                }
            };
            vertical.OnItemSelected = (groupIndex, index, id, item) => {
                if (vds.TryGetValue(item.gameObject, out Vd vd)) {
                    vd.SetSelected();
                }
            };
        }
        if (horizontal != null) {
            horizontal.OnItemCreated = (item) => {
                if (!vdss.TryGetValue(item.gameObject, out Vd vd)) {
                    vd = new Vd().Init(item.gameObject);
                    vdss.Add(item.gameObject, vd);
                }
            };
            horizontal.OnItemRefreshed = (groupIndex, index, id, item) => {
                if (vdss.TryGetValue(item.gameObject, out Vd vd)) {
                    vd.Refresh(id);
                }
            };
            horizontal.OnItemSelected = (groupIndex, index, id, item) => {
                if (vdss.TryGetValue(item.gameObject, out Vd vd)) {
                    vd.SetSelected();
                }
            };
        }
    }
    private void Update() {
        TimerMgr.OnUpdate();

        offMin = rt.offsetMin;
        offMax = rt.offsetMax;
        anchorPosition = rt.anchoredPosition3D;

        if (Input.GetKeyDown(KeyCode.A)) {
            rt.offsetMin = new Vector2(0, -100f);
            rt.offsetMax = new Vector2(0, 100f);
        }
        else if (Input.GetKeyDown(KeyCode.B)) {
            // 测试
            List<uint> ls = new List<uint>();
            for (int i = 0; i < 58; ++i) {
                ls.Add((uint)i);
            }
            vertical?.SetIdList(ls);
            horizontal?.SetIdList(ls);
        }
        else if (Input.GetKeyDown(KeyCode.C)) {
            // 测试
            List<uint> ls = new List<uint>();
            for (int i = 0; i < 5; ++i) {
                ls.Add((uint)i);
            }
            vertical?.SetIdList(ls);
            horizontal?.SetIdList(ls);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            // 测试
            List<uint> ls = new List<uint>();
            for (int i = 0; i < 120; ++i) {
                ls.Add((uint)i);
            }
            vertical?.SetIdList(ls);
            horizontal?.SetIdList(ls);
        }

        else if (Input.GetKeyDown(KeyCode.E)) {
            vertical?.MoveToById(5);
            horizontal?.MoveToById(5);
        }
        else if (Input.GetKeyDown(KeyCode.F)) {
            vertical?.MoveToById(57);
            horizontal?.MoveToById(57);
        }
        else if (Input.GetKeyDown(KeyCode.G)) {
            vertical?.MoveToById(200);
            horizontal?.MoveToById(200);
        }
    }
}
