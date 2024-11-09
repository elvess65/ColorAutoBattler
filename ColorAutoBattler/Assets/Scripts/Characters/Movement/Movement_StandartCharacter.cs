using UnityEngine;

namespace Paint.Characters.Movement
{
    /// <summary>
    /// Передвижение при помощи CharacterController
    /// </summary>
    public class Movement_StandartCharacter : iMovement
    {
        private Transform m_Target;
        private float m_MoveSpeed;
        private float m_RotationSpeed;


        public Movement_StandartCharacter(Transform target, float moveSpeed, float rotationSpeed)
        {
            m_Target = target;
            m_MoveSpeed = moveSpeed;
            m_RotationSpeed = rotationSpeed;
        }


        public virtual void Move(Vector3 mDir) => m_Target.Translate(mDir * m_MoveSpeed * Time.deltaTime, Space.World);

        public virtual void Rotate(float angle)
        {
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.up);
            m_Target.rotation = Quaternion.Lerp(m_Target.rotation, targetRot, Time.deltaTime * m_RotationSpeed);
        }
    }
}
