using Paint.Character.Weapon;
using Paint.General;
using UnityEngine;

namespace Paint.Character.Health.UI
{
    public class UIHealthBarController_Unit : UIFollowingObject
    {
        public RectTransform SegmentParent;

        private UIHealthBarSegment m_Segment;
        private UIAttackDefenceBar m_UIAttackDefenceBar;


        public void Init(Transform parent, int healthAmount, WeaponTypes attackType, WeaponTypes resistType)
        {
            Init(parent);

            //Create segment
            m_Segment = GameManager.Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UIHealthBarSegmentPrefab) as UIHealthBarSegment;

            //Position segment
            RectTransform segmentRectTransform = m_Segment.GetComponent<RectTransform>();
            segmentRectTransform.SetParent(SegmentParent);
            segmentRectTransform.anchoredPosition = Vector3.zero;
            segmentRectTransform.localPosition = Vector3.zero;
            segmentRectTransform.localEulerAngles = Vector3.zero;
            segmentRectTransform.localScale = Vector3.one * 3;

            //Init segment
            m_Segment.Init(Weapon.WeaponTypes.Max, healthAmount);

            //Attack defence bar
            m_UIAttackDefenceBar = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UIAttackDefenceBarPrefab);
            m_UIAttackDefenceBar.transform.SetParent(SegmentParent);
            m_UIAttackDefenceBar.transform.localScale = Vector3.one;
            m_UIAttackDefenceBar.transform.localPosition = new Vector3(0, 0, -350);
            m_UIAttackDefenceBar.transform.localRotation = Quaternion.Euler(new Vector3(-45, 0, 0));

            m_UIAttackDefenceBar.Init(attackType, resistType);
        }

        public void UpdateUI(WeaponTypes type, int currentHealth) => m_Segment.UpdateUI(currentHealth);
    }
}
