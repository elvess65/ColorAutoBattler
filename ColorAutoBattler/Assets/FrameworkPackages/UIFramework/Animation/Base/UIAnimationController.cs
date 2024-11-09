using System.Collections;
using UnityEngine;

namespace FrameworkPackage.UI.Animations
{
    /// <summary>
    /// Базовый класс для всех контроллеров анимации
    /// </summary>
    public abstract class UIAnimationController_Base : MonoBehaviour
    {
        public event System.Action OnShowFinished;
        public event System.Action OnHideFinished;

        [Header("Animation")]
        [Tooltip("Задержка перед началом анимации")]
        public float DelayBeforeShow = 0;
        [Tooltip("Время анимации")]
        public float TimeToTargetPosition = 1.0f;
        [Tooltip("Объект анимируеться туда сюда")]
        public bool PingPong = false;

        public abstract void PlayAnimation(bool playShowAnimation);

        public abstract void Init();

        protected void CallShowFinished()
        {
            if (OnShowFinished != null)
                OnShowFinished();
        }

        protected void CallHideFinished()
        {
            if (OnHideFinished != null)
                OnHideFinished();
        }
    }

    /// <summary>
    /// Базовый класс для контроля над UI анимациями
    /// </summary>
    public abstract class UIAnimationController<T> : UIAnimationController_Base
    {
        [Space(10)]
        public T StartPosition;
        public AnimationCurve CurveShow;
        public AnimationCurve CurveHide;
        public bool AutoInit = true;

        protected T m_TargetPosition;
        protected float m_TotalDistance = 0;

        private Coroutine m_Coroitune;
        private bool m_WasInitialized = false;

        protected virtual void Awake()
        {
            if (AutoInit)
                Init();
        }

        public override void PlayAnimation(bool playShowAnimation)
        {
            if (!m_WasInitialized)
                return;

            if (m_Coroitune != null)
                StopCoroutine(m_Coroitune);

            if (playShowAnimation)
                m_Coroitune = StartCoroutine(Show());
            else
                m_Coroitune = StartCoroutine(Hide());
        }

        public override void Init()
        {
            if (m_WasInitialized)
                return;

            SetTargetPosition();

            m_TotalDistance = GetTotalDistance();

            ApplyPosition(StartPosition);

            m_WasInitialized = true;
        }

        IEnumerator Show()
        {
            if (DelayBeforeShow > 0)
                yield return new WaitForSeconds(DelayBeforeShow);

            ShowStarted();
            float speed = 1.0f / TimeToTargetPosition;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                ApplyPosition(GetShowValue(t));
                yield return null;
            }

            ApplyPosition(m_TargetPosition);
            ShowFinished();
        }

        protected virtual void ShowStarted()
        {
        }

        protected virtual void ShowFinished()
        {
            m_Coroitune = null;

            CallShowFinished();

            if (PingPong)
            {
                DelayBeforeShow = 0;
                PlayAnimation(false);
            }
        }


        IEnumerator Hide()
        {
            HideStarted();
            float speed = 1.0f / TimeToTargetPosition;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
            {
                ApplyPosition(GetHideValue(t));
                yield return null;
            }

            ApplyPosition(StartPosition);
            HideFinished();
        }

        protected virtual void HideStarted()
        {
        }

        protected virtual void HideFinished()
        {
            m_Coroitune = null;

            CallHideFinished();

            if (PingPong)
            {
                DelayBeforeShow = 0;
                PlayAnimation(true);
            }
        }


        [ContextMenu("Show")]
        public void ContextMenuShow()
        {
            StartCoroutine(Show());
        }

        [ContextMenu("Hide")]
        public void ContextMenuHide()
        {
            StartCoroutine(Hide());
        }


        protected abstract float GetTotalDistance();

        protected abstract T GetShowValue(float t);

        protected abstract T GetHideValue(float t);

        protected abstract void SetTargetPosition();

        protected abstract void ApplyPosition(T position);
    }
}
