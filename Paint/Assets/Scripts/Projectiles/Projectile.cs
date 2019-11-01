using Paint.Character.Weapon;
using Paint.General;
using UnityEngine;

namespace Paint.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public WeaponTypes Type => WeaponTypes.Red;
        public int Damage => 2;
        
        private Vector3 m_MoveDir;
        private float m_Speed = 5;
        private float m_LifeTime = 1;
        private float m_CurLifeTime = 0;

        private bool isActive = false;


        public void Launch(Vector2 dir)
        {
            m_MoveDir = new Vector3(dir.x, 0, dir.y);
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
