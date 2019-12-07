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
        float DistanceToUpdate { get; set; }

        void SetMovePosition(Vector3 movePos);
    }
}
