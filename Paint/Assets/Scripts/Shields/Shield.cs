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
                case WeaponTypes.Red:
                    color = Color.red;
                    break;
                case WeaponTypes.Green:
                    color = Color.green;
                    break;
                case WeaponTypes.Blue:
                    color = Color.blue;
                    break;
                case WeaponTypes.Yellow:
                    color = Color.yellow;
                    break;
            }

            ShieldRebderer.material.color = color;
        }
    }
}
