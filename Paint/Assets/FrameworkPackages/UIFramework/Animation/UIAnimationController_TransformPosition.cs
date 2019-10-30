using UnityEngine;

namespace FrameworkPackage.UI.Animations
{
    /// <summary>
    /// Контролирует анимацию расположения MaskableGraphic(картинка, текст)
    /// </summary>
    public class UIAnimationController_TransformPosition : UIAnimationController_Vector
    {
        [Header("Link")]
        public Transform Target;

        protected override void SetTargetPosition()
        {
            //Получить текущую позицию кнопки
            m_TargetPosition = Target.position;
        }

        protected override void ApplyPosition(Vector3 position)
        {
            //Задать позицию кнопки
            Target.position = position;
        }
    }
}
