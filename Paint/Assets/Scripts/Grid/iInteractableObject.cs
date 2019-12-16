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
        event System.Action<int, int> OnSetTargetCell;
        event System.Action<int, int> OnReleaseTargetCell;

        Vector3 GetPosition { get; }

        void SetMovePosition(Vector3 movePos, int x, int y);
    }
}

namespace Paint.Movement
{
    public interface iMoveStrategy
    {
        event System.Action<Vector3, bool> OnUpdatePosition;
        bool IsMoving { get; }
        Vector3 GetPosition { get; }

        void MoveToPosition(Vector3 pos);
        void StopMovement();
        void Update(float deltaTime);
    }
}
