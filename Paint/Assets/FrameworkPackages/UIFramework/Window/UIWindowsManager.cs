using System.Collections.Generic;
using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    public partial class UIWindowsManager : MonoBehaviour
    {
        [Header("Parents")]
        public RectTransform FadeParent;
        public RectTransform WindowParent;
        [Header("Windows")]
        public UIWindow_Base UIWindow_ScreenFade;

        private UIWindow_Base m_ScreenFade;
        private Stack<UIWindow_Base> m_WindowQueue;

        void Awake() => m_WindowQueue = new Stack<UIWindow_Base>();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_WindowQueue.Count > 0)
                    m_WindowQueue.Peek().HideByEscape();
            }
        }


        public UIWindow_Base ShowWindow(UIWindow_Base source, bool show = true)
        {
            if (m_WindowQueue.Count == 0 )
                ShowScreenFade();

            UIWindow_Base wnd = CreateWindow(source, WindowParent);
            m_WindowQueue.Push(wnd);

            wnd.OnUIBeginHiding += WindowCloseHandler;

            if (show)
                wnd.Show();

            return wnd;
        }

        public UIWindow_Base ShowWindowWithoutFade(UIWindow_Base source)
        {
            UIWindow_Base wnd = CreateWindow(source, WindowParent);
            wnd.Show();

            return wnd;
        }

        public void ShowScreenFade()
        {
            if (UIWindow_ScreenFade != null)
            {
                m_ScreenFade = CreateWindow(UIWindow_ScreenFade, FadeParent);
                m_ScreenFade.Show();
            }
        }

        public void HideScreenFade()
        {
            if (m_ScreenFade != null)
                m_ScreenFade.Hide();
        }

        public void HideAllWindows()
        {
            while (m_WindowQueue.Count > 0)
                m_WindowQueue.Peek().Hide();
        }


        UIWindow_Base CreateWindow(UIWindow_Base source, RectTransform parent)
        {
            UIWindow_Base wnd = Instantiate(source);
            RectTransform rTransform = wnd.GetComponent<RectTransform>();
            rTransform.SetParent(parent, false);

            return wnd;
        }

        void WindowCloseHandler()
        {
            if (m_WindowQueue.Count > 0)
            {
                m_WindowQueue.Pop();

                if (m_WindowQueue.Count == 0)
                    HideScreenFade();
            }
        }
    }
}
