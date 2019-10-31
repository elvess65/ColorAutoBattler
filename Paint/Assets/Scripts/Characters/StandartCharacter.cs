using Paint.Characters.Movement;
using Paint.Characters.Shooting;

namespace Paint.Characters
{
    /// <summary>
    /// Обычный персонаж
    /// </summary>
    public class StandartCharacter : Character
    {
        private const float m_MOVE_SPEED = 3;
        private const float m_ROTATION_SPEED = 15;

        private const float m_AIM_TIME = 0.3f;
        private const float m_SHOOT_TIME = 0.1f;
        private const float m_COOLDOWN_TIME = 0.2f;

        public override void Init()
        {
            m_MoveBehaviour = new Movement_StandartCharacter(transform, m_MOVE_SPEED, m_ROTATION_SPEED);
            m_ShootBehaviour = new Shooting_StandartShooting(m_AIM_TIME, m_SHOOT_TIME, m_COOLDOWN_TIME, transform);

            base.Init();
        }
    }
}
