using Paint.General;
using UnityEngine;

namespace Paint.InputSystem
{
    /// <summary>
    /// Отслеживание ввода
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public System.Action<bool> OnInputStateChange;
        public System.Action<Vector2> OnMove;

        private BaseInputManager m_CurrentInput;
        private bool m_InputIsEnabledState = false;

        public bool InputIsEnabled
        {
            get { return m_InputIsEnabledState; }
            set
            {
                if (m_InputIsEnabledState != value)
                {
                    m_InputIsEnabledState = value;

                    OnInputStateChange?.Invoke(m_InputIsEnabledState);
                }
            }
        }


        public void Init()
        {
            m_CurrentInput = new KeyboardInputManager();
            m_CurrentInput.OnMove += CallMoveEvent;
        }

        void CallMoveEvent(Vector2 mDir) => OnMove?.Invoke(mDir);

        void Update()
        {
            if (GameManager.Instance.IsActive && m_InputIsEnabledState)
                m_CurrentInput.UpdateInput();
        }
    }


    public abstract class BaseInputManager
    {
        public System.Action<Vector2> OnMove;

        public abstract void UpdateInput();
    }
}
