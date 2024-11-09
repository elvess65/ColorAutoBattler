using UnityEngine;

namespace Paint.Movement
{
    public interface iMoveStrategy
    {
        event System.Action<Vector3, bool> OnUpdatePosition;
        bool IsMoving { get; }
        Vector3 GetPosition { get; }
        Transform GetTransform { get; }

        bool MoveToPosition(Vector3 pos);
        void StopMovement();
        void Update(float deltaTime);
    }
}
