using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

    #region

    /// <summary>
    /// this page's type
    /// </summary>
    public enum UIType
    {
        Normal,
        Fixed,
        PopUp,
        None,      //独立的窗口
    }

    /// <summary>
    /// how to show this page.
    /// </summary>
    public enum UIMode
    {
        /// <summary>
        /// 什么也不做
        /// </summary>
        DoNothing,

        /// <summary>
        /// 闭其他界面
        /// </summary>
        HideOther,

        /// <summary>
        /// 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        /// </summary>
        NeedBack,

        /// <summary>
        /// 关闭TopBar,关闭其他界面,不加入backSequence队列
        /// </summary>
        NoNeedBack,
    }

    /// <summary>
    /// the background collider mode
    /// </summary>
    public enum UICollider
    {
        /// <summary>
        /// 显示该界面不包含碰撞背景
        /// </summary>
        None,

        /// <summary>
        /// 碰撞透明背景
        /// </summary>
        NoColor,

        /// <summary>
        /// 碰撞非透明背景
        /// </summary>
        Translucent,
    }

    #endregion

    public class UIBase : MonoBehaviour
    {
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
        private object data;

        public Action action;

        public Action clickMaskAction;

        [HideInInspector]
        public Page page = Page.None;
        public UIType type = UIType.Normal;
        public UIMode mode = UIMode.DoNothing;
        public UICollider collder = UICollider.None;

        public bool isAsyncUI = false;
        protected bool isActived = false;

        public virtual void Init() { }
        public virtual void Refresh() { }
        public virtual void Active()
        {
            this.gameObject.SetActive(true);
            isActived = true;
        }
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            isActived = false;
        }

        internal bool CheckIfNeedBack()
        {
            if (type == UIType.Fixed || type == UIType.PopUp || type == UIType.None) return false;
            else if (mode == UIMode.NoNeedBack || mode == UIMode.DoNothing) return false;
            return true;
        }
        public void AnchorUIGameObject()
        {
            if (UIRoot.Instance == null) return;

            Vector3 anchorPos = Vector3.zero;
            Vector2 sizeDel = Vector2.zero;
            Vector3 scale = Vector3.one;
            if (this.GetComponent<RectTransform>() != null)
            {
                anchorPos = this.GetComponent<RectTransform>().anchoredPosition;
                sizeDel = this.GetComponent<RectTransform>().sizeDelta;
                scale = this.GetComponent<RectTransform>().localScale;
            }
            else
            {
                anchorPos = this.transform.localPosition;
                scale = this.transform.localScale;
            }

            switch (type)
            {
                case UIType.Normal: this.transform.SetParent(UIRoot.Instance.normalRoot); break;
                case UIType.Fixed: this.transform.SetParent(UIRoot.Instance.fixedRoot); break;
                case UIType.PopUp: this.transform.SetParent(UIRoot.Instance.popupRoot); break;
            }

            if (this.GetComponent<RectTransform>() != null)
            {
                this.GetComponent<RectTransform>().anchoredPosition = anchorPos;
                this.GetComponent<RectTransform>().sizeDelta = sizeDel;
                this.GetComponent<RectTransform>().localScale = scale;
            }
            else
            {
                this.transform.localPosition = anchorPos;
                this.transform.localScale = scale;
            }
        }
        public bool isActive()
        {
            bool ret = gameObject != null && gameObject.activeSelf;
            return ret || isActived;
        }
        public void AddMask()
        {
            Image mask = this.GetComponent<Image>();
            Button maskButton = this.GetComponent<Button>();
            if (mask == null)
            {
                mask = this.gameObject.AddComponent<Image>();
                maskButton = this.gameObject.AddComponent<Button>();
                maskButton.onClick.AddListener(() =>
                {
                    if (clickMaskAction != null)
                    {
                        clickMaskAction();
                    }
                });
            }

            switch (collder)
            {
                case UICollider.None:
                    mask.raycastTarget = false;
                    break;

                case UICollider.NoColor:
                    mask.raycastTarget = true;
                    mask.color = new Color(0, 0, 0, 0);
                    break;

                case UICollider.Translucent:
                    mask.raycastTarget = true;
                    mask.color = new Color(0, 0, 0, 100 / 255f);
                    break;

                default:
                    break;
            }
        }

        public void ClosePage()
        {
            switch (mode)
            {
                case UIMode.DoNothing:
                    Hide();
                    break;
                case UIMode.HideOther:
                    UIRoot.ClosePage();
                    break;
                case UIMode.NeedBack:
                    break;
                case UIMode.NoNeedBack:
                    break;
                default:
                    break;
            }
        }
    }
