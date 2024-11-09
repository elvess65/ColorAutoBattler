using Paint.General;
using UnityEngine;

namespace Paint.CameraSystem
{
    /// <summary>
    /// Управление камерой
    /// </summary>
    [RequireComponent(typeof(CameraBehaviour_OffsetAlign))]
    [RequireComponent(typeof(CameraBehaviour_FollowTarget))]
    public class CameraController : MonoBehaviour
    {
        public System.Action OnCameraFinishedAligning;

        public Vector3 InitCameraOffset = new Vector3(0, 12, -10);

        private CameraBehaviour_OffsetAlign m_Behaviour_OffsetAlign;
        private CameraBehaviour_FollowTarget m_Behaviour_FollowTarget;

        private Transform m_Target;
        private Vector3 m_CachedOffset;


        public void Init()
        {
            m_Behaviour_OffsetAlign = GetComponent<CameraBehaviour_OffsetAlign>();
            m_Behaviour_FollowTarget = GetComponent<CameraBehaviour_FollowTarget>();
        }


        /// <summary>
        /// Задать объект для следования
        /// </summary>
        public void SetTarget(Transform target)
        {
            m_Target = target;

            //Выровнять камеру согласно отступу
            m_Behaviour_OffsetAlign.OnFinished += StartFollowingTarget;
            m_Behaviour_OffsetAlign.AlignToOffset(m_Target, InitCameraOffset);
        }

        /// <summary>
        /// Начать следовать за целью, закешировав отступ
        /// </summary>
        void StartFollowingTarget()
        {
            m_Behaviour_OffsetAlign.OnFinished -= StartFollowingTarget;
            m_Behaviour_FollowTarget.Follow(m_Target, CacheOffset());

            OnCameraFinishedAligning?.Invoke();
        }


        void LateUpdate()
        {
            if (!GameManager.Instance.IsActive && m_Target == null)
                return;

            m_Behaviour_OffsetAlign.UpdateBehaviour();
            m_Behaviour_FollowTarget.UpdateBehaviour();
        }


        //Tools
        Vector3 CacheOffset()
        {
            m_CachedOffset = transform.position - m_Target.transform.position;
            return m_CachedOffset;
        }
    }
}
