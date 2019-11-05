using System;
using Paint.Character.Weapon;

namespace Paint.Characters.Shield
{
    /// <summary>
    /// отсутствие щита
    /// </summary>
    public class Shield_None : iShield
    {
        public bool IsShieldActivated => false; 
        public WeaponTypes WeaponType => WeaponTypes.Max;

        public event Action OnShieldActivated;
        public event Action OnShieldDeactivated;

        public void ActivateShield()
        { }

        public void ProcessShield()
        { }

        public void SetWeaponType(WeaponTypes wType)
        { }
    }
}
