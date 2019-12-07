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

        void SetMovePosition(Vector3 movePos, GridController gridController, float d);
    }
}

namespace Paint.Movement
{
    public interface iMoveStrategy
    {
        event System.Action OnUpdatePosition;
        float DistanceToUpdate { get; set; }
        bool IsMoving { get; }

        void MoveToPosition(Vector3 pos);
        void StopMove();
        void Update(float deltaTime);
    }
}
