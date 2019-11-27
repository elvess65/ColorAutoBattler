using System;
using Paint.Character.Health.UI;
using Paint.Character.Weapon;
using Paint.General;
using UnityEngine;

namespace Paint.Character.Health
{
    public class Health_UnitCharacter : iHealth
    {
        public bool IsDestroyed => m_HealthSegment.IsEmpty;

        public event Action<WeaponTypes, int> OnTakeDamage;
        public event Action OnDestroy;
        public event Action<WeaponTypes> OnWrongType;

        private UIHealthBarController_Unit m_UIHealthBarController;

        private Health_StandartCharacter.HealthSegment m_HealthSegment;


        public Health_UnitCharacter(int healthAmount, WeaponTypes attackType, WeaponTypes resistType, Transform healthBarSpawnPoint, Transform followTarget)
        {
            //Инициализировать UI
            m_UIHealthBarController = GameManager.Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UIHealthBarUnitPrefab) as UIHealthBarController_Unit;
            m_UIHealthBarController.transform.position = healthBarSpawnPoint.position;
            m_UIHealthBarController.Init(followTarget, healthAmount, attackType, resistType);

            m_HealthSegment = new Health_StandartCharacter.HealthSegment(resistType, healthAmount);
        }


        public void TakeDamage(WeaponTypes type, int damage)
        {
            Debug.Log("Unit is taking damage " + type + " " + damage);

            //Нанести  урон
            if (m_HealthSegment.Type == type)
                damage = damage / 2;

            m_HealthSegment.TakeDamage(damage);

            //Обновить UI
            m_UIHealthBarController.UpdateUI(type, m_HealthSegment.CurHealth);

            //Персонаж уничтожен
            if (IsDestroyed)
                OnDestroy?.Invoke(); 
            else //Нанесен урон
                OnTakeDamage?.Invoke(m_HealthSegment.Type, m_HealthSegment.CurHealth);
        }

        public Transform GetUISegmentParent() => m_UIHealthBarController.SegmentParent;
    }
}
