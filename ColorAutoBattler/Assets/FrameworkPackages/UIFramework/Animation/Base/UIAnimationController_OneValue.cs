namespace FrameworkPackage.UI.Animations
{
    public abstract class UIAnimationController_OneValue : UIAnimationController<float>
    {
        protected override float GetTotalDistance()
        {
            return m_TargetPosition - StartPosition;
        }

        protected override float GetShowValue(float t)
        {
            return StartPosition + CurveShow.Evaluate(t) * m_TotalDistance;
        }

        protected override float GetHideValue(float t)
        {
            return m_TargetPosition - CurveHide.Evaluate(t) * m_TotalDistance;
        }
    }
}
