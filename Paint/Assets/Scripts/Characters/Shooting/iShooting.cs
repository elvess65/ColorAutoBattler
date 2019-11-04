using Paint.Character.Weapon;
using UnityEngine;

namespace Paint.Characters.Shooting
{
    public interface iShooting 
    {
        event System.Action OnRotation;
        event System.Action OnAim;
        event System.Action OnShoot;
        event System.Action OnCooldown;
        event System.Action OnFinish;

        bool IsShooting { get; }
        Vector2 ShootDir { get; set; }
        WeaponTypes WeaponType { get; set; }
        int Damage { get; set; }
        float AngleToRotateBeforeAim { get; set; }

        void StartShoot(Vector2 sDir);
        void SetWeaponType(WeaponTypes type);
        void ProcessShooting();

        bool IsLookingAtShootDir();
    }
}
