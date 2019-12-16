using Paint.Grid.Interaction;
using UnityEngine;

namespace Paint.Grid.Interaction
{
    public interface iInteractableObject 
    {
        bool IsSelected { get; }

        void Select();
        void Unselect();
    }
}

namespace Paint.Grid.Movement
{
    public interface iMovableObject : iInteractableObject
    {
        event System.Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;

        Vector3 GetPosition { get; }

        void SetMovePosition(Vector3 movePos);
    }
}

namespace Paint.Movement
{
    public interface iMoveStrategy
    {
        event System.Action<Vector3> OnUpdatePosition;
        bool IsMoving { get; }
        Vector3 GetPosition { get; }

        void MoveToPosition(Vector3 pos);
        void StopMovement();
        void Update(float deltaTime);
    }
}
