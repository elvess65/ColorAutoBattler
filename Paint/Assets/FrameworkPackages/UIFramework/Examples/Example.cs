using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows.Example
{
    public class Example : MonoBehaviour
    {
        [Header("Buttons")]
        public Button Button_Window;
        [Header("Windows")]
        public UIWindow_Base Window_Example_Prefab;

        private UIWindowsManager m_WindowManager;
        private UIWindow_Base m_Window;

        private void Start()
        {
            m_WindowManager = GetComponent<UIWindowsManager>();

            Button_Window.onClick.AddListener(Button_Windows_PressHandler);
        }

        void Button_Windows_PressHandler()
        {
            if (m_Window == null)
            {
                m_Window = m_WindowManager.ShowWindow(Window_Example_Prefab);
            }
            else
            {
                m_Window.Hide();
                m_Window = null;
            }
        }
    }
}
