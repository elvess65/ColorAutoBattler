using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Базовый класс для всех интерактивных UI объектов (окна, вкладки, элементы)
    /// </summary>
    public class UIObject : MonoBehaviour
    {
        protected bool m_IsInitialized = false;

        protected virtual void Init()
        {
            m_IsInitialized = true;
        }
    }
}
