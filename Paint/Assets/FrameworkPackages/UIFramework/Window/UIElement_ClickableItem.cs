using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// UI элемент, который можно использовать как кнопку
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class UIElement_ClickableItem : MonoBehaviour
    {
        public System.Action<RectTransform> OnItemClick;
        public RectTransform ItemRectTransform { get; private set; }

        protected Button m_Button;

        public void EnableButton(bool state)
        {
            m_Button.enabled = state;
        }

        protected void Init()
        {
            ItemRectTransform = GetComponent<RectTransform>();

            //Подписаться на нажатие 
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(Button_PressHandler);
        }

        protected virtual void Button_PressHandler()
        {
            OnItemClick?.Invoke(ItemRectTransform);
        }
    }
}
