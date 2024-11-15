﻿using Paint.Character.Health;
using Paint.Character.Weapon;
using Paint.Characters.Animation;
using Paint.Characters.Movement;
using Paint.Characters.Shield;
using Paint.Characters.Shooting;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Общий класс для всех персонажей
    /// </summary>
    [RequireComponent(typeof(iAnimation))]
    public abstract class Character : MonoBehaviour
    {
        public System.Action OnDestroy;

        public Transform ProjectileSpawnPoint;
        public Transform HealthBarSpawnPoint;

        protected Vector3 m_TargetMoveDir = Vector3.zero;
        protected float m_TargetRotAngle;

        protected iMovement m_MoveBehaviour;
        protected iShooting m_ShootBehaviour;
        protected iAnimation m_AnimationBehaviour;
        protected iHealth m_HealthBehaviour;
        protected iShield m_ShieldBehaviour;
        protected Collider m_CollisionCollider;

        public bool IsDestroyed { get; private set; }
        public bool IsMoving => m_TargetMoveDir.sqrMagnitude > 0;
        
        private bool CanMove => !m_ShootBehaviour.IsShooting && !m_ShieldBehaviour.IsShieldActivated;           //Перемещение - не стреляет и не под щитом
        private bool CanShoot => !m_ShootBehaviour.IsShooting && !m_ShieldBehaviour.IsShieldActivated;          //Выстрел - не стреляет и не под щитом
        private bool CanActivateShield => !m_ShieldBehaviour.IsShieldActivated && !m_ShootBehaviour.IsShooting; //Щит -  - не стреляет и не под щитом


        public virtual void Init((WeaponTypes type, int health)[] healthData)
        {
            IsDestroyed = false;

            if (m_CollisionCollider == null)
                m_CollisionCollider = GetComponent<Collider>();

            m_CollisionCollider.enabled = true;

            m_AnimationBehaviour = GetComponent<iAnimation>();
            m_AnimationBehaviour.Init();

            m_ShootBehaviour.OnRotation += HandleShootEvent_Rotation;
            m_ShootBehaviour.OnAim += HandleShootEvent_Aim;
            m_ShootBehaviour.OnShoot += HandleShootEvent_Shoot;
            m_ShootBehaviour.OnCooldown += HandleShootEvent_Cooldown;
            m_ShootBehaviour.OnFinish += HandleShootEvent_Finish;

            m_HealthBehaviour.OnTakeDamage += HandleDamageEvent_TakeDamage;
            m_HealthBehaviour.OnDestroy += HandleDamageEvent_Destroy;
            m_HealthBehaviour.OnWrongType += HandleDamageEvent_WrongType;

            m_ShieldBehaviour.OnShieldActivated += HandleShieldActivated;
            m_ShieldBehaviour.OnShieldDeactivated += HandleShieldDeactivated;
        }


        public void SetMoveDiretion(Vector2 mDir)
        {
            if (m_ShootBehaviour.IsShooting)
                return;

            //Кэш направления передвижения
            m_TargetMoveDir = new Vector3(mDir.x, 0, mDir.y);

            //Если игрок совершил перемещение
            if (m_TargetMoveDir != Vector3.zero)
            {
                //Кеш вращения в направлении движения
                m_TargetRotAngle = Mathf.Atan2(m_TargetMoveDir.x, m_TargetMoveDir.z) * Mathf.Rad2Deg;
            }
        }

        public void Shoot(Vector2 sDir)
        {
            if (!CanShoot)
                return;

            m_ShootBehaviour.StartShoot(sDir);

            //Кеш вращения в направлении движения0-
            m_TargetRotAngle = Mathf.Atan2(sDir.x, sDir.y) * Mathf.Rad2Deg;
        }

        public void ShieldActivate()
        {
            if (!CanActivateShield)
                return;

            m_ShieldBehaviour.ActivateShield();
        }

        public void SelectWeaponType(WeaponTypes type)
        {
            if (!m_ShootBehaviour.IsShooting)
            {
                m_ShootBehaviour.SetWeaponType(type);
                m_ShieldBehaviour.SetWeaponType(type);
            }
        }

        public void TakeDamage(WeaponTypes type, int damage)
        {
            if (!ShieldIsValid(type))
                m_HealthBehaviour.TakeDamage(type, damage);
        }


        protected virtual void HandleShootEvent_Rotation() { m_AnimationBehaviour.PlayAimAnimation(); }

        protected virtual void HandleShootEvent_Aim() { m_AnimationBehaviour.PlayAimAnimation(); }

        protected virtual void HandleShootEvent_Shoot() { m_AnimationBehaviour.PlayShootAnimation(); }

        protected virtual void HandleShootEvent_Cooldown() { m_AnimationBehaviour.PlayCooldownAnimation(); }

        protected virtual void HandleShootEvent_Finish() { m_AnimationBehaviour.PlayFinishShootAnimation(); }


        protected virtual void HandleDamageEvent_TakeDamage(WeaponTypes type, int damage) { m_AnimationBehaviour.PlayDamageAnimation(); }

        protected virtual void HandleDamageEvent_Destroy()
        {
            m_AnimationBehaviour.PlayDestroyAnimation();

            m_CollisionCollider.enabled = false;
            IsDestroyed = true;

            OnDestroy?.Invoke();
        }

        protected virtual void HandleDamageEvent_WrongType(WeaponTypes type) { }


        protected virtual void HandleShieldActivated(WeaponTypes type)
        {
            m_AnimationBehaviour.PlayStayAnimation();
            m_AnimationBehaviour.PlayShieldActivatedAnimation();
        }

        protected virtual void HandleShieldDeactivated() => m_AnimationBehaviour.PlayShieldDeactivatedAnimation();


        protected virtual void Update()
        {
            if (IsDestroyed)
                return;

            if (CanMove)
                ProcessMovement();
            else 
            {
                if (m_ShootBehaviour.IsShooting)
                    ProcessShooting();
                else if (m_ShieldBehaviour.IsShieldActivated)
                    ProcessShield();
            }
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            Projectiles.Projectile projectile = collider.GetComponent<Projectiles.Projectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.Type, projectile.Damage);
                projectile.Collide();
            }
        }


        void ProcessMovement()
        {
            m_MoveBehaviour.Move(m_TargetMoveDir);
            m_MoveBehaviour.Rotate(m_TargetRotAngle);

            if (IsMoving)
                m_AnimationBehaviour.PlayMoveAnimation();
            else
                m_AnimationBehaviour.PlayStayAnimation();
        }

        void ProcessShooting()
        {
            m_ShootBehaviour.ProcessShooting();

            if (!m_ShootBehaviour.IsLookingAtShootDir())
                m_MoveBehaviour.Rotate(m_ShootBehaviour.AngleToRotateBeforeAim);
        }

        void ProcessShield() => m_ShieldBehaviour.ProcessShield();


        bool ShieldIsValid(WeaponTypes type) => m_ShieldBehaviour.IsShieldActivated && m_ShieldBehaviour.WeaponType == type;
    }
}
