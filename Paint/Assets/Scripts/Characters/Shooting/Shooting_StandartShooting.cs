using System;
using UnityEngine;

namespace Paint.Characters.Shooting
{
    public class Shooting_StandartShooting : iShooting
    {
        public event Action OnRotation;
        public event Action OnAim;
        public event Action OnShoot;
        public event Action OnCooldown;
        public event Action OnFinish;

        public bool IsShooting => m_ShootPhaseTracker.CurrentPhase != ShootPhase.Phases.None;
        public float AngleToRotateBeforeAim { get; set; }
        public Vector2 ShootDir { get; set; }

        private float[] m_PhaseTimes;
        private Transform m_CharacterTransform;
        private ShootPhase m_ShootPhaseTracker;

        private const float m_DELTA_ANGLE_TO_DIR = 1f;


        public Shooting_StandartShooting(float aimingTime, float shootTime, float cooldownTime, Transform characterTransform)
        {
            m_CharacterTransform = characterTransform;
            m_PhaseTimes = new float[] { aimingTime, shootTime, cooldownTime };
            m_ShootPhaseTracker = new ShootPhase();
        }


        public void StartShoot(Vector2 sDir)
        {
            ShootDir = sDir;

            GoToNextPhase();
        }

        public void ProcessShooting()
        {
            if (IsShooting && m_ShootPhaseTracker.NextPhaseTime > 0 && Time.time >= m_ShootPhaseTracker.NextPhaseTime)
                GoToNextPhase();
        }

        public bool IsLookingAtShootDir()
        {
            if (m_ShootPhaseTracker.CurrentPhase != ShootPhase.Phases.Rotating)
                return true;

            bool isLookingAtShootDir = GetAnglePlayerAndDir(ShootDir) <= m_DELTA_ANGLE_TO_DIR;

            if (isLookingAtShootDir)
                GoToNextPhase();

            return isLookingAtShootDir;
        }


        void GoToNextPhase()
        {
            m_ShootPhaseTracker.CurrentPhase++;

            if (m_ShootPhaseTracker.CurrentPhase > ShootPhase.Phases.Cooldown)
            {
                OnFinish?.Invoke();

                m_ShootPhaseTracker.NextPhaseTime = 0;
                m_ShootPhaseTracker.CurrentPhase = ShootPhase.Phases.None;
            }
            else
            {
                //Не считать время фазы, если текущая фаза - вращение
                if (m_ShootPhaseTracker.CurrentPhase != ShootPhase.Phases.Rotating)
                    m_ShootPhaseTracker.NextPhaseTime = Time.time + m_PhaseTimes[(int)m_ShootPhaseTracker.CurrentPhase - 2];

                switch (m_ShootPhaseTracker.CurrentPhase)
                {
                    case ShootPhase.Phases.Rotating:

                        if (!IsLookingAtShootDir())
                        {
                            AngleToRotateBeforeAim = Mathf.Atan2(ShootDir.x, ShootDir.y) * Mathf.Rad2Deg;
                            OnRotation?.Invoke();
                        }

                        break;

                    case ShootPhase.Phases.Aiming:
                        OnAim?.Invoke();

                        break;

                    case ShootPhase.Phases.Shooting:
                        OnShoot?.Invoke();
                        break;

                    case ShootPhase.Phases.Cooldown:
                        OnCooldown?.Invoke();
                        break;
                }
            }
        }

        float GetAnglePlayerAndDir(Vector2 dir) => Vector2.Angle(new Vector2(m_CharacterTransform.forward.x, m_CharacterTransform.forward.z), dir);


        struct ShootPhase
        {
            public float NextPhaseTime;
            public Phases CurrentPhase;

            public enum Phases { None, Rotating, Aiming, Shooting, Cooldown }
        }
    }
}
