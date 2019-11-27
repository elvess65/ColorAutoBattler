using Paint.Character.Health;
using Paint.Character.Weapon;
using Paint.Characters.Animation;
using Paint.Characters.Movement;
using Paint.Characters.Shield;
using Paint.Characters.Shooting;
using Paint.General;
using UnityEngine;

namespace Paint.Characters
{
    public class UnitCharacter : Character
    {
        public Renderer R;

        private UnitCharacter m_Target;

        private const float m_MOVE_SPEED = 1.3f;
        private const float m_ROTATION_SPEED = 15;

        private const float m_AIM_TIME = 0.5f;
        private const float m_SHOOT_TIME = 0.2f;
        private const float m_COOLDOWN_TIME = 0.5f;
        private int m_DAMAGE = 5;
        private float m_ATTACK_DISTANCE = 5;

        public int m_PlayerID;
        public int ID;
        public bool HasTarget => m_Target != null;

        public void Init(int playerID, int id, int healthAmount, Color teamColor, CharacterTypes cType, WeaponTypes attackType, WeaponTypes resistType)
        {
            R.material.color = teamColor;

            switch(cType)
            {
                case CharacterTypes.Melee:
                    m_ATTACK_DISTANCE = 1;
                    m_DAMAGE = 3;
                    break;
            }

            m_PlayerID = playerID;
            ID = id;

            m_MoveBehaviour = new Movement_StandartCharacter(transform, m_MOVE_SPEED, m_ROTATION_SPEED);
            m_ShootBehaviour = new Shooting_StandartShooting(m_AIM_TIME, m_SHOOT_TIME, m_COOLDOWN_TIME, m_DAMAGE, transform);
            m_HealthBehaviour = new Health_UnitCharacter(healthAmount, attackType, resistType, HealthBarSpawnPoint, transform);
            m_ShieldBehaviour = new Shield_None();

            SelectWeaponType(attackType);

            Init(null);

            switch (cType)
            {
                case CharacterTypes.Melee:
                    GetComponent<Animation_StandartCharacter>().SetArsenal(GetComponent<Animation_StandartCharacter>().ArsenalList[1].name);
                    break;
            }
        }

        public void AttackTarget()
        {
            if (m_Target == null || m_Target.IsDestroyed)
            {
                SetMoveDiretion(Vector2.zero);
                m_Target = null;

                return;
            }

            Vector3 dirToEnemy = m_Target.transform.position - transform.position;
            Vector2 dirToEnemyNormalized = new Vector2(dirToEnemy.x, dirToEnemy.z).normalized;

            if (dirToEnemy.magnitude > m_ATTACK_DISTANCE)
                SetMoveDiretion(dirToEnemyNormalized);
            else
            {
                SetMoveDiretion(Vector2.zero);
                Shoot(dirToEnemyNormalized);
            }
        }

        public void SetTarget(UnitCharacter target) => m_Target = target;


        protected override void HandleShootEvent_Shoot()
        {
            base.HandleShootEvent_Shoot();

            Projectiles.Projectile projectile = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.ProjectilePrefab);
            projectile.transform.position = ProjectileSpawnPoint.position;
            projectile.Launch(m_ShootBehaviour.WeaponType, m_ShootBehaviour.ShootDir, m_ShootBehaviour.Damage);
        }

        protected override void HandleDamageEvent_Destroy()
        {
            base.HandleDamageEvent_Destroy();

            m_HealthBehaviour.GetUISegmentParent().gameObject.SetActive(false);
        }
    }

    public enum CharacterTypes { Melee, Range, Fly, Max }
}
