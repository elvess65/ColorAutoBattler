using UnityEngine;

namespace Paint.Objects.Interfaces
{
    public interface iMovableObject : iInteractableObject
    {
        event System.Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;
    }
}