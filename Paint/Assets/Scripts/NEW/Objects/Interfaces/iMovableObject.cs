using UnityEngine;

namespace Paint.Objects.Interfaces
{
    public interface iMovableObject : iInteractableObject
    {
        event System.Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;
        event System.Action<int, int> OnSetTargetCell;
        event System.Action<int, int> OnReleaseTargetCell;
    }
}