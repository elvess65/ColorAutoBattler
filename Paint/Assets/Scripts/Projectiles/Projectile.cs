using Paint.Character.Weapon;
using Paint.General;
using UnityEngine;

namespace Paint.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public MeshRenderer ProjectileRebderer;

        public WeaponTypes Type { get; private set; }
        public int Damage { get; private set; }

        private Vector3 m_MoveDir;
        private float m_Speed = 5;
        private float m_LifeTime = 3;
        private float m_CurLifeTime = 0;

        private bool isActive = false;


        public void Launch(WeaponTypes type, Vector2 dir, int damage)
        {
            Type = type;
            Damage = damage;
            m_MoveDir = new Vector3(dir.x, 0, dir.y);

            Color color = Color.gray;
            switch(type)
            {
                case WeaponTypes.Frost:
                    color = Color.blue;
                    break;
                case WeaponTypes.Sun:
                    color = Color.yellow;
                    break;
                case WeaponTypes.Earth:
                    color = Color.green;
                    break;
                case WeaponTypes.Water:
                    color = Color.cyan;
                    break;
            }

            ProjectileRebderer.material.color = color;

            isActive = true;
        }

        public void Collide() => Destroy(gameObject);


        void Update()
        {
            if (GameManager.Instance.IsActive && isActive)
            {
                transform.Translate(m_MoveDir * m_Speed * Time.deltaTime, Space.World);

                m_CurLifeTime += Time.deltaTime;
                if (m_CurLifeTime >= m_LifeTime)
                    Destroy(gameObject);
            }
        }
    }
}
