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
    /// Башня, которая может поворачиватся и атаковать, но не может перемещатся
    /// </summary>
    public class TurrentCharacter : Character
    {
        public Transform RotationObject;
        public ParticleSystem ShootEffect;

        private const float m_ROTATION_SPEED = 15;

        private const float m_AIM_TIME = 0.0f;
        private const float m_SHOOT_TIME = 0.1f;
        private const float m_COOLDOWN_TIME = 0.5f;
        private const float m_TIME_BETWEEN_SHOOTS_MIN = 2;
        private const float m_TIME_BETWEEN_SHOOTS_MAX = 4;
        private const float m_DISTANCE_TO_ATTACK = 5f;
        private const int m_DAMAGE = 5;

        private float m_ShootTime = 0;


        public override void Init((WeaponTypes type, int health)[] healthData)
        {
            m_MoveBehaviour = new Movement_RotationOnly(RotationObject, m_ROTATION_SPEED);
            m_ShootBehaviour = new Shooting_StandartShooting(m_AIM_TIME, m_SHOOT_TIME, m_COOLDOWN_TIME, m_DAMAGE, RotationObject);
            m_HealthBehaviour = new Health_StandartCharacter(healthData, HealthBarSpawnPoint, transform);
            m_ShieldBehaviour = new Shield_None();

            SelectWeaponType((WeaponTypes)Random.Range(0, (int)WeaponTypes.Max));

            m_ShootTime = Time.time + GetRandomTimeBetweenShoots();

            base.Init(healthData);
        }


        protected override void HandleShootEvent_Shoot()
        {
            base.HandleShootEvent_Shoot();

            Projectiles.Projectile projectile = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.ProjectilePrefab);
            projectile.transform.position = ProjectileSpawnPoint.position;
            projectile.Launch(m_ShootBehaviour.WeaponType, m_ShootBehaviour.ShootDir, m_ShootBehaviour.Damage);

            ShootEffect.Play();
        }

        protected override void Update()
        {
            Vector3 dir2Player = GameManager.Instance.PlayerCharacter.transform.position - transform.position;
            Vector3 dir2PlayerNormalized = dir2Player.normalized;

            if (!m_ShootBehaviour.IsShooting)
                m_TargetRotAngle = Mathf.Atan2(dir2PlayerNormalized.x, dir2PlayerNormalized.z) * Mathf.Rad2Deg;

            if (dir2Player.magnitude <= m_DISTANCE_TO_ATTACK)
            {
                if (Time.time >= m_ShootTime)
                {
                    m_ShootTime = Time.time + GetRandomTimeBetweenShoots();
                    Shoot(new Vector2(dir2PlayerNormalized.x, dir2PlayerNormalized.z));
                }
            }

            base.Update();
        }


        float GetRandomTimeBetweenShoots() => Random.Range(m_TIME_BETWEEN_SHOOTS_MIN, m_TIME_BETWEEN_SHOOTS_MAX);
    }
}
