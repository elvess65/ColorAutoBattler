using Paint.Characters.Movement;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Обычный персонаж
    /// </summary>
    public class StandartCharacter : Character
    {
        private const float m_MOVE_SPEED = 3;
        private const float m_ROTATION_SPEED = 7;

        public override void Init()
        {
            base.Init();

            m_MoveController = new Movement_StandartCharacter(transform, m_MOVE_SPEED, m_ROTATION_SPEED);
        }
    }
}
