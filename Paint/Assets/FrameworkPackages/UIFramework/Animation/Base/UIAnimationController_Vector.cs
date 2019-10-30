using UnityEngine;

namespace FrameworkPackage.UI.Animations
{
    public abstract class UIAnimationController_Vector : UIAnimationController<Vector3>
    {
        protected override float GetTotalDistance()
        {
            return Vector3.Distance(m_TargetPosition, StartPosition);
        }

        protected override Vector3 GetShowValue(float t)
        {
            return StartPosition + CurveShow.Evaluate(t) * (m_TargetPosition - StartPosition);
        }

        protected override Vector3 GetHideValue(float t)
        {
            return m_TargetPosition - CurveHide.Evaluate(t) * (m_TargetPosition - StartPosition);
        }
    }
}
