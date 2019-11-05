using Paint.Character.Weapon;

namespace Paint.Characters.Shield
{
    public interface iShield
    {
        event System.Action OnShieldActivated;
        event System.Action OnShieldDeactivated;

        bool IsShieldActivated { get; }
        WeaponTypes WeaponType { get; }

        void ActivateShield();
        void ProcessShield();
        void SetWeaponType(WeaponTypes wType);
    }
}
