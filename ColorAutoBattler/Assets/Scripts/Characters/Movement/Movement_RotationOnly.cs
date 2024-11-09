using UnityEngine;

namespace Paint.Characters.Movement
{
    /// <summary>
    /// Нет возможности перемещатся, но есть возможность вращатся
    /// </summary>
    public class Movement_RotationOnly : Movement_StandartCharacter
    {
        public Movement_RotationOnly(Transform target, float rotationSpeed) : base(target, 0, rotationSpeed)
        { }

        public override void Move(Vector3 mDir)
        { }
    }
}
