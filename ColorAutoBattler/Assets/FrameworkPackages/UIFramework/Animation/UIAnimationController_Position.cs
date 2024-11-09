using UnityEngine;

namespace FrameworkPackage.UI.Animations
{
    /// <summary>
    /// Контролирует анимацию расположения MaskableGraphic(картинка, текст)
    /// </summary>
    public class UIAnimationController_Position : UIAnimationController_Vector
    {
        [Header("Link")]
        public RectTransform Target;

        protected override void SetTargetPosition()
        {
            //Получить текущую позицию кнопки
            m_TargetPosition = Target.anchoredPosition;
        }

        protected override void ApplyPosition(Vector3 position)
        {
            //Задать позицию кнопки
            Target.anchoredPosition = position;
        }
    }
}
