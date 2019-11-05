using System;
using Paint.Character.Weapon;
using UnityEngine;

namespace Paint.Characters.Shield
{
    /// <summary>
    /// Щит обычного персонажа
    /// </summary>
    public class Shield_StandartShield : iShield
    {
        public event Action OnShieldActivated;
        public event Action OnShieldDeactivated;

        public bool IsShieldActivated => m_ShieldIsActivated;
        public WeaponTypes WeaponType => m_WeaponType;

        private float m_Duration;
        private float m_DeactivationTime;
        private bool m_ShieldIsActivated = false;
        private WeaponTypes m_WeaponType;


        public Shield_StandartShield(float duration)
        {
            m_Duration = duration;
        }

        public void ActivateShield()
        {
            m_DeactivationTime = Time.time + m_Duration;
            m_ShieldIsActivated = true;

            OnShieldActivated?.Invoke();
        }

        public void ProcessShield()
        {
            if (IsShieldActivated && Time.time >= m_DeactivationTime)
                DeactivateShield();
        }

        public void SetWeaponType(WeaponTypes wType) => m_WeaponType = wType;


        void DeactivateShield()
        {
            m_ShieldIsActivated = false;
            OnShieldDeactivated?.Invoke();
        }
    }
}
