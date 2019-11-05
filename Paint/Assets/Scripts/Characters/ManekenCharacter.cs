using Paint.Character.Health;
using Paint.Character.Weapon;
using Paint.Characters.Movement;
using Paint.Characters.Shield;
using Paint.Characters.Shooting;
using Paint.General;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Манекен, который не может атаковать и перемещатся
    /// </summary>
    public class ManekenCharacter : Character
    {
        private const float m_ROTATION_SPEED = 15;

        public override void Init((WeaponTypes type, int health)[] healthData)
        {
            m_MoveBehaviour = new Movement_RotationOnly(transform, m_ROTATION_SPEED);
            m_ShootBehaviour = new Shooting_None();
            m_HealthBehaviour = new Health_StandartCharacter(healthData, HealthBarSpawnPoint, transform);
            m_ShieldBehaviour = new Shield_None();

            base.Init(healthData);
        }

        protected override void Update()
        {
            Vector3 dir2Player = (GameManager.Instance.PlayerCharacter.transform.position - transform.position).normalized;
            m_TargetRotAngle = Mathf.Atan2(dir2Player.x, dir2Player.z) * Mathf.Rad2Deg;

            base.Update();
        }
    }
}
  