using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

public class UIRoot : MonoBehaviour
{
    private static UIRoot m_Instance = null;
    public static UIRoot Instance
    {
        get
        {
            if (m_Instance == null)
            {
                InitRoot();
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    public Transform root;
    public Transform fixedRoot;
    public Transform normalRoot;
    public Transform popupRoot;
    public Camera uiCamera;

    public static string uiPrefabRootPath = "Prefabs/UIs/";

    private static List<UIBase> currentPageNodes;
    private static Dictionary<string, UIBase> allPages;

    private static void InitRoot()
    {
        GameObject go = new GameObject("UIRoot");
        go.layer = LayerMask.NameToLayer("UI");
        m_Instance = go.AddComponent<UIRoot>();
        go.AddComponent<RectTransform>();

        Canvas can = go.AddComponent<Canvas>();
        can.renderMode = RenderMode.ScreenSpaceCamera;
        can.pixelPerfect = true;

        go.AddComponent<GraphicRaycaster>();

        m_Instance.root = go.transform;

        GameObject camObj = new GameObject("UICamera");
        camObj.layer = LayerMask.NameToLayer("UI");
        camObj.transform.parent = go.transform;
        camObj.transform.localPosition = new Vector3(0, 0, -100f);
        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
        cam.orthographic = true;
        cam.farClipPlane = 200f;
        can.worldCamera = cam;
        cam.cullingMask = 1 << 5;
        cam.nearClipPlane = -50f;
        cam.farClipPlane = 50f;

        m_Instance.uiCamera = cam;

        //add audio listener
        camObj.AddComponent<AudioListener>();
        camObj.AddComponent<GUILayer>();

        CanvasScaler cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(320f, 568f);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        GameObject subRoot;

        subRoot = CreateSubCanvasForRoot(go.transform, 0);
        subRoot.name = "NormalRoot";
        m_Instance.normalRoot = subRoot.transform;
        m_Instance.normalRoot.transform.localScale = Vector3.one;

        subRoot = CreateSubCanvasForRoot(go.transform, 250);
        subRoot.name = "FixedRoot";
        m_Instance.fixedRoot = subRoot.transform;
        m_Instance.fixedRoot.transform.localScale = Vector3.one;

        subRoot = CreateSubCanvasForRoot(go.transform, 500);
        subRoot.name = "PopupRoot";
        m_Instance.popupRoot = subRoot.transform;
        m_Instance.popupRoot.transform.localScale = Vector3.one;

        //add Event System
        GameObject esObj = GameObject.Find("EventSystem");
        if (esObj != null)
        {
            GameObject.DestroyImmediate(esObj);
        }

        GameObject eventObj = new GameObject("EventSystem");
        eventObj.layer = LayerMask.NameToLayer("UI");
        eventObj.transform.SetParent(go.transform);
        eventObj.AddComponent<EventSystem>();
        eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }
    private static GameObject CreateSubCanvasForRoot(Transform root, int sort)
    {
        GameObject go = new GameObject("canvas");
        go.transform.parent = root;
        go.layer = LayerMask.NameToLayer("UI");

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        return go;
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

    public static void ShowPage(Page page, Action action = null)
    {
        ShowPage(page, null, action);
    }
    public static void ShowPage(Page page, object data, Action action = null)
    {
        string path = GetPagePath(page);
        UIBase uiBase = null;

        if (allPages != null && allPages.ContainsKey(path))
        {
            uiBase = allPages[path];

            if (data != null)
            {
                uiBase.Data = data;
            }

            if (action != null)
            {
                uiBase.action = action;
            }

            uiBase.Refresh();
            uiBase.Active();
            PopNode(uiBase);
        }
        else
        {
            LoadPage(path, page, data, action);
        }
    }
    public static void ClosePage()
    {
        if (currentPageNodes == null || currentPageNodes.Count <= 1) return;

        UIBase closePage = currentPageNodes[currentPageNodes.Count - 1];
        currentPageNodes.RemoveAt(currentPageNodes.Count - 1);

        if (currentPageNodes.Count > 0)
        {
            UIBase openPage = currentPageNodes[currentPageNodes.Count - 1];
            if (openPage.isAsyncUI)
            {

            }
            else
            {
                ShowPage(openPage.page);
                closePage.Hide();
            }
        }
    }
    public static void HidePage(Page page)
    {
        string key = GetPagePath(page);
        UIBase ui = null;
        if (allPages.TryGetValue(key, out ui))
        {
            ui.Hide();
        }
    }

    private static void LoadPage(string path, Page page, object data, Action action = null)
    {
        GameObject go = ResourceManager.Instance.LoadAsset<GameObject>(path);
        if (go == null)
        {
            Debug.LogError("[UI]can not load prefab " + path);
            return;
        }

        go = GameObject.Instantiate(go);
        UIBase uiBase = go.GetComponent<UIBase>();
        if (uiBase == null)
        {
            Debug.LogError("[UI]can not find script");
            return;
        }

        if (allPages == null)
        {
            allPages = new Dictionary<string, UIBase>();
        }

        uiBase.page = page;
        uiBase.AnchorUIGameObject();
        uiBase.AddMask();
        uiBase.Init();

        allPages[path] = uiBase;
        ShowPage(page, data, action);
    }
    private static bool CheckIfNeedBack(UIBase page)
    {
        return page != null && page.CheckIfNeedBack();
    }
    private static void PopNode(UIBase page)
    {
        if (currentPageNodes == null)
        {
            currentPageNodes = new List<UIBase>();
        }

        if (page == null)
        {
            Debug.LogError("[UI] page popup is null.");
            return;
        }

        if (CheckIfNeedBack(page) == false)
            return;

        bool _isFound = false;
        for (int i = 0; i < currentPageNodes.Count; i++)
        {
            if (currentPageNodes[i].Equals(page))
            {
                currentPageNodes.RemoveAt(i);
                currentPageNodes.Add(page);
                _isFound = true;
                break;
            }
        }

        if (!_isFound)
            currentPageNodes.Add(page);

        HideOldNodes();
    }
    private static void HideOldNodes()
    {
        if (currentPageNodes.Count < 0) return;
        UIBase topPage = currentPageNodes[currentPageNodes.Count - 1];
        if (topPage.mode == UIMode.HideOther)
        {
            for (int i = currentPageNodes.Count - 2; i >= 0; i--)
            {
                if (currentPageNodes[i].isActive())
                    currentPageNodes[i].Hide();
            }
        }
    }
    public static void ClearNodes()
    {
        currentPageNodes.Clear();
    }

    private static string GetPagePath(Page page)
    {
        string path = string.Empty;

        switch (page)
        {
            case Page.GameMain: path = "GameMain"; break;
            case Page.Login: path = "Login"; break;
            case Page.Register: path = "Register"; break;
            case Page.SelectRole: path = "SelectRole"; break;
            case Page.CreateRole: path = "CreateRole"; break;

            default: Debug.LogError("未知类型:" + page); break;
        }
        return uiPrefabRootPath + path;
    }
}

public enum Page
{
    None,
    GameMain,
    Login,
    Register,
    SelectRole,
    CreateRole,
}