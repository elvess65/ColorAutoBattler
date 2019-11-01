using FrameworkPackage.Utils;
using Paint.Character.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Paint.Character.Health.UI
{
    /// <summary>
    /// Контроллер объекта, который отображает количество ХП определенного типа
    /// </summary>
    public class UIHealthBarSegment : MonoBehaviour
    {
        public Image Image_FG;

        [Header("Animation")]
        public float AnimationTime = 0.5f;

        private int m_MaxHealth;
        private InterpolationData<float> m_LerpData;


        public void Init(WeaponTypes type, int health)
        {
            m_LerpData = new InterpolationData<float>(AnimationTime);
            m_MaxHealth = health;

            switch (type)
            {
                case WeaponTypes.Blue:
                    Image_FG.color = Color.blue;
                    break;
                case WeaponTypes.Green:
                    Image_FG.color = Color.green;
                    break;
                case WeaponTypes.Red:
                    Image_FG.color = Color.red;
                    break;
                default:
                    Image_FG.color = Color.white;
                    break;
            }
        }

		public void UpdateUI(int currentHealth)
		{
            float progress = (float)currentHealth / m_MaxHealth;
            m_LerpData.From = Image_FG.fillAmount;
            m_LerpData.To = progress;
            m_LerpData.Start();
        }
        

        void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                Image_FG.fillAmount = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();
                    Image_FG.fillAmount = m_LerpData.To;
                }
            }
        }
    }
}
