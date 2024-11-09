using UnityEngine;

namespace Paint.Characters.Movement
{
    /// <summary>
    /// Интерфейс для всех типов передвижений
    /// </summary>
    public interface iMovement
    {
        void Move(Vector3 mDir);
        void Rotate(float angle);
    }
}
