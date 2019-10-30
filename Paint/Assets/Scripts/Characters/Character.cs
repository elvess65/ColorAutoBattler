using Paint.Characters.Animation;
using Paint.Characters.Movement;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Общий класс для всех персонажей
    /// </summary>
    [RequireComponent(typeof(StandartCharacterAnimationController))]
    public abstract class Character : MonoBehaviour
    {
        protected Vector3 m_TargetMoveDir = Vector3.zero;
        protected float m_TargetRotAngle;

        protected iMovement m_MoveController;
        private StandartCharacterAnimationController m_AnimationController;

        //iShootBehaviour
        //iHealthBehaviour


        public virtual void Init()
        {
            m_AnimationController = GetComponent<StandartCharacterAnimationController>();
            m_AnimationController.Init();
        }

        public void GetMoveDiretion(Vector2 mDir)
        {
            //Кэш направления передвижения
            m_TargetMoveDir = new Vector3(mDir.x, 0, mDir.y);

            //Если игрок совершил перемещение
            if (m_TargetMoveDir != Vector3.zero)
            {
                //Кеш вращение в направлении движения
                m_TargetRotAngle = Mathf.Atan2(m_TargetMoveDir.x, m_TargetMoveDir.z) * Mathf.Rad2Deg;
            }
        }


        protected virtual void Update()
        {
            HandleMovement();
        }


        void HandleMovement()
        {
            m_MoveController.Move(m_TargetMoveDir);
            m_MoveController.Rotate(m_TargetRotAngle);

            if (m_TargetMoveDir.sqrMagnitude > 0)
                m_AnimationController.PlayMoveAnimation();
            else
                m_AnimationController.PlayStayAnimation();
        }
    }
}
