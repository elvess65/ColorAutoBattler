using System;
using Paint.Character.Weapon;
using UnityEngine;

namespace Paint.Characters.Shooting
{
    /// <summary>
    /// Отстутствие возможности стрелять
    /// </summary>
    public class Shooting_None : iShooting
    {
        public bool IsShooting => false;

        public Vector2 ShootDir { get => Vector2.zero; set { } }
        public float AngleToRotateBeforeAim { get => 0; set { } }

        public WeaponTypes WeaponType { get; set; }
        public int Damage { get; set; }

        public event Action OnRotation;
        public event Action OnAim;
        public event Action OnShoot;
        public event Action OnCooldown;
        public event Action OnFinish;

        public bool IsLookingAtShootDir() => false;

        public void ProcessShooting()
        { }

        public void SetWeaponType(WeaponTypes type)
        { }

        public void StartShoot(Vector2 sDir)
        { }
    }
}
