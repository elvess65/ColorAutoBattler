using Paint.Character.Weapon;
using Paint.General;
using System.Collections.Generic;
using UnityEngine;
using static Paint.Character.Health.Health_StandartCharacter;

namespace Paint.Character.Health.UI
{
    /// <summary>
    /// Контроллер объекта, который отображает количество ХП у персонажа
    /// </summary>
    public class UIHealthBarController : UIFollowingObject
    {
        public RectTransform SegmentParent;

        private float m_InitSize = 3;
        private float m_StepDelta = 0.5f; 

        private Dictionary<WeaponTypes, UIHealthBarSegment> m_Segments;


        public void Init(Transform parent, Dictionary<WeaponTypes, HealthSegment> healthData)
        {
            Init(parent);

            //Healthbar
            int segments = 0;
            m_Segments = new Dictionary<WeaponTypes, UIHealthBarSegment>();
            foreach (HealthSegment segmentData in healthData.Values)
            {
                //Create segment
                UIHealthBarSegment segment = GameManager.Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UIHealthBarSegmentPrefab) as UIHealthBarSegment;

                //Position segment
                RectTransform segmentRectTransform = segment.GetComponent<RectTransform>();
                segmentRectTransform.SetParent(SegmentParent);
                segmentRectTransform.anchoredPosition = Vector3.zero;
                segmentRectTransform.localPosition = Vector3.zero;
                segmentRectTransform.localEulerAngles = Vector3.zero;

                //Scale segment
                float scale = m_InitSize - m_StepDelta * segments;
                segmentRectTransform.localScale = new Vector3(scale, scale, scale);

                //Init segment
                segment.Init(segmentData.Type, segmentData.Health);

                //Other
                m_Segments.Add(segmentData.Type, segment);
                segments++;
            }
        }

		public void UpdateUI(WeaponTypes type, int currentHealth)
		{
            UIHealthBarSegment segment = GetSegmentForTakeDamage(type);
            if (segment != null)
                segment.UpdateUI(currentHealth);
		}


        /// <summary>
        /// Получить сегмент хп, которому наноситься урон
        /// </summary>
        UIHealthBarSegment GetSegmentForTakeDamage(WeaponTypes type)
        {
            if (m_Segments.ContainsKey(type))
                return m_Segments[type];

            return null;
        }
    }
}
