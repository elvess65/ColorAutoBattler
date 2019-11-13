using Paint.Character.Health;
using Paint.Character.Weapon;
using Paint.Characters.Movement;
using Paint.Characters.Shield;
using Paint.Characters.Shooting;
using UnityEngine;

namespace Paint.Characters
{
    public class UnitCharacter : Character
    {
        private const float m_MOVE_SPEED = 3;
        private const float m_ROTATION_SPEED = 15;

        private const float m_AIM_TIME = 0.3f;
        private const float m_SHOOT_TIME = 0.1f;
        private const float m_COOLDOWN_TIME = 0.2f;
        private const int m_DAMAGE = 1;

        public override void Init((WeaponTypes type, int health)[] healthData)
        {
            base.Init(healthData);

            m_MoveBehaviour = new Movement_StandartCharacter(transform, m_MOVE_SPEED, m_ROTATION_SPEED);
            m_ShootBehaviour = new Shooting_StandartShooting(m_AIM_TIME, m_SHOOT_TIME, m_COOLDOWN_TIME, m_DAMAGE, transform);
            m_HealthBehaviour = new Health_StandartCharacter(healthData, HealthBarSpawnPoint, transform);
            m_ShieldBehaviour = new Shield_None();
        }
    }
}
