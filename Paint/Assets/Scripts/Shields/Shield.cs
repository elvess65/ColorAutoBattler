using Paint.Character.Weapon;
using UnityEngine;

namespace Paint.Shields
{
    public class Shield : MonoBehaviour
    {
        public MeshRenderer ShieldRebderer;

        public void Init(WeaponTypes type)
        {
            Color color = Color.gray;
            switch (type)
            {
                case WeaponTypes.Frost:
                    color = Color.red;
                    break;
                case WeaponTypes.Sun:
                    color = Color.green;
                    break;
                case WeaponTypes.Earth:
                    color = Color.blue;
                    break;
                case WeaponTypes.Water:
                    color = Color.yellow;
                    break;
            }

            color = new Color(color.r, color.g, color.b, 0.25f);

            ShieldRebderer.material.color = color;
        }
    }
}
