using UnityEngine;
using UnityEngine.UI;

namespace Paint.Character.Health.UI
{
    public class UIAttackDefenceBar : MonoBehaviour
    {
        public Image Image_Attack;
        public Image Image_Defence;

        public Sprite[] Sprites;

        public void Init(Weapon.WeaponTypes attackType, Weapon.WeaponTypes resistType)
        {
            Image_Attack.sprite = Sprites[(int)attackType];
            Image_Defence.sprite = Sprites[(int)resistType];
        }
    }
}
