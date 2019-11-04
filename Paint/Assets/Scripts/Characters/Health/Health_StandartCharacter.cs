using Paint.Character.Health.UI;
using Paint.Character.Weapon;
using Paint.General;
using System.Collections.Generic;
using UnityEngine;

namespace Paint.Character.Health
{
    /// <summary>
    /// Контроллер ХП персонажа
    /// </summary>
    public class Health_StandartCharacter : iHealth
    {
        public event System.Action<WeaponTypes, int> OnTakeDamage;
        public event System.Action OnDestroy;
        public event System.Action<WeaponTypes> OnWrongType;

        private UIHealthBarController m_UIHealthBarController;
        private Dictionary<WeaponTypes, HealthSegment> m_HealthData;

        public bool IsDestroyed
        {
            get
            {
                int emptySegments = 0;
                foreach (HealthSegment s in m_HealthData.Values)
                {
                    if (s.IsEmpty)
                        emptySegments++;
                }

                return emptySegments == m_HealthData.Count;
            }
        }


        public Health_StandartCharacter((WeaponTypes type, int health)[] healthData, Transform healthBarSpawnPoint, Transform followTarget)
        {
            //Инициализировать данные
            m_HealthData = new Dictionary<WeaponTypes, HealthSegment>();
            for (int i = 0; i < healthData.Length; i++)
            {
                if (!m_HealthData.ContainsKey(healthData[i].type))
                {
                    HealthSegment hSegment = new HealthSegment(healthData[i].type, healthData[i].health);
                    hSegment.Init();

                    m_HealthData.Add(hSegment.Type, hSegment);
                }
            }

            //Инициализировать UI
            m_UIHealthBarController = GameManager.Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UIHealthBarPrefab) as UIHealthBarController;
            m_UIHealthBarController.transform.position = healthBarSpawnPoint.position;
            m_UIHealthBarController.Init(followTarget, m_HealthData);
        }

        /// <summary>
        /// Нанести урон персонажу
        /// </summary>
        public void TakeDamage(WeaponTypes type, int damage)
        {
            HealthSegment healthSegment = GetSegmentForTakeDamage(type);
            if (healthSegment != null)
            {
                //Нанести  урон
                healthSegment.TakeDamage(damage);

                //Обновить UI
                m_UIHealthBarController.UpdateUI(type, healthSegment.CurHealth);

                //Персонаж уничтожен
                if (IsDestroyed)
                    OnDestroy?.Invoke();
                else //Нанесен урон
                    OnTakeDamage?.Invoke(healthSegment.Type, healthSegment.CurHealth);
            }
            else //Урон не подходящей способностью
                OnWrongType?.Invoke(type);
        }


        /// <summary>
        /// Получить сегмент хп, которому наноситься урон
        /// </summary>
        HealthSegment GetSegmentForTakeDamage(WeaponTypes type)
        {
            if (m_HealthData.ContainsKey(type))
                return m_HealthData[type];

            return null;
        }

   
        /// <summary>
        /// Представление одного сегмента хп
        /// </summary>
        public class HealthSegment
        {
            public WeaponTypes Type { get; private set; }
            public int Health { get; private set; }
            public int CurHealth { get; private set; }
            public bool IsEmpty => CurHealth <= 0;

            public HealthSegment(WeaponTypes type, int health)
            {
                Type = type;
                Health = health;
                CurHealth = Health;
            }

            public void Init() => CurHealth = Health;

            public void TakeDamage(int damage)
            {
                CurHealth -= damage;

                if (CurHealth < 0)
                    CurHealth = 0;
            }
        }
    }
}
