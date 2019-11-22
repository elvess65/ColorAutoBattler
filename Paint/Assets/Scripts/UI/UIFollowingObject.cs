using Paint.Controllers3D;
using UnityEngine;

namespace Paint.Character.Health.UI
{
    [RequireComponent(typeof(FollowTransformController))]
    public abstract class UIFollowingObject : MonoBehaviour
    {
        private FollowTransformController m_FollowController;

        public void Init(Transform parent)
        {
            //Follow
            if (m_FollowController == null)
                m_FollowController = GetComponent<FollowTransformController>();

            m_FollowController.Init(parent);
        }
    }
}
