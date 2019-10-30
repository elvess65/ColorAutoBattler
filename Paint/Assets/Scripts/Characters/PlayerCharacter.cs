using Paint.Characters.Movement;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Персонаж, которые пребывает под контролем игрока
    /// </summary>
    public class PlayerCharacter : Character
    {
        private Vector3 m_TargetMoveDir = Vector3.zero;
        private float m_TargetRotAngle;


        public override void Init()
        {
            base.Init();

            m_MoveController = new Movement_StandartCharacter(transform, 3, 5);
        }

        public override void Move(Vector2 mDir)
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


        protected override void Update()
        {
            base.Update();

            m_MoveController.Move(m_TargetMoveDir);
            m_MoveController.Rotate(m_TargetRotAngle);
        }
    }
}
