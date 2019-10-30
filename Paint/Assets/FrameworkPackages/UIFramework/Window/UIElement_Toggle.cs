using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Реализация Toggle, которая позволяет производить производить переключение при условии
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UIElement_Toggle : MonoBehaviour
    {
        public System.Action<bool> OnTryChangeValue;

        [Header("Images")]
        public Image Image_Checkbox;
        [Header("Buttons")]
        public Button Button_Main;
        [Header("Settings")]
        public bool IsToggled = false;
        public Animator AnimationController;

        private bool m_Value = false;

        private const string m_ANIMATION_KEY_SUCCESS = "Success";
        private const string m_ANIMATION_KEY_ERROR = "Error";

        public void Init()
        {
            m_Value = IsToggled;
            SetValue(m_Value);

            Button_Main.onClick.AddListener(Button_Main_PressHandler);
        }

        public void SetValue(bool isToggled)
        {
            m_Value = isToggled;
            UpdateVisual();
        }

        public void PlaySuccessAnimation()
        {
            if (AnimationController != null)
                AnimationController.SetTrigger(m_ANIMATION_KEY_SUCCESS);
        }

        public void PlayErrorAnimation()
        {
            if (AnimationController != null)
                AnimationController.SetTrigger(m_ANIMATION_KEY_ERROR);
        }


        void Button_Main_PressHandler()
        {
            OnTryChangeValue?.Invoke(!m_Value);
        }

        void UpdateVisual()
        {
            Image_Checkbox.gameObject.SetActive(m_Value);
        }
    }
}
