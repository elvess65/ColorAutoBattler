﻿using Paint.Character.Health;
using Paint.Character.Weapon;
using Paint.Characters.Animation;
using Paint.Characters.Movement;
using Paint.Characters.Shooting;
using Paint.General;
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

        public bool IsMoving => m_TargetMoveDir.sqrMagnitude > 0;
        public bool IsDestroyed { get; private set; }


        public virtual void Init()
        {
            IsDestroyed = false;

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
            if (m_ShootBehaviour.IsShooting)
                return;

            m_ShootBehaviour.StartShoot(sDir);

            //Кеш вращения в направлении движения0-
            m_TargetRotAngle = Mathf.Atan2(sDir.x, sDir.y) * Mathf.Rad2Deg;
        }

        public void TakeDamage(WeaponTypes type, int damage) => m_HealthBehaviour.TakeDamage(type, damage);


        void HandleShootEvent_Rotation() { m_AnimationBehaviour.PlayAimAnimation(); }

        void HandleShootEvent_Aim() { m_AnimationBehaviour.PlayAimAnimation(); }

        void HandleShootEvent_Shoot()
        {
            m_AnimationBehaviour.PlayShootAnimation();

            Projectiles.Projectile projectile = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.ProjectilePrefab);
            projectile.transform.position = ProjectileSpawnPoint.position;
            projectile.Launch(m_ShootBehaviour.ShootDir);
        }

        void HandleShootEvent_Cooldown() { m_AnimationBehaviour.PlayCooldownAnimation(); }

        void HandleShootEvent_Finish() { m_AnimationBehaviour.PlayFinishShootAnimation(); }


        void HandleDamageEvent_TakeDamage(WeaponTypes type, int damage) { m_AnimationBehaviour.PlayDamageAnimation(); }

        void HandleDamageEvent_Destroy()
        {
            m_AnimationBehaviour.PlayDestroyAnimation();
            IsDestroyed = true;

            OnDestroy?.Invoke();
        }

        void HandleDamageEvent_WrongType(WeaponTypes type) { Debug.Log("WrongType"); }



        protected virtual void Update()
        {
            if (IsDestroyed)
                return;

            //Можно перемещатся только если не стреляешь
            if (!m_ShootBehaviour.IsShooting)
                ProcessMovement();
            else 
                ProcessShooting();
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
    }
}
