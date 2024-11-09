namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Вкладка окна
    /// </summary>
    public abstract class UITabContent : UIObject
    {
        public virtual void InitTab()
        {
            if (!m_IsInitialized)
                Init();
        }

        /// <summary>
        /// Очистка данных при вкладки при закритии окна
        /// </summary>
        public virtual void DisposeOnWindowClose()
        { }

        /// <summary>
        /// Отключить вкладку, при активации другой вкладки
        /// </summary>
        public virtual void DeactivateTabOnSelectOther()
        { }

        /// <summary>
        /// Обновить состояние вкладки
        /// </summary>
        protected virtual void UpdateTabState()
        { }
    }
}
