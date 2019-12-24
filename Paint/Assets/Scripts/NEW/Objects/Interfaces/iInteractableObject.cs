using Paint.Commands;
using UnityEngine;

namespace Paint.Objects.Interfaces
{
    public interface iInteractableObject
    {
        ObjectTypes ObjectType { get; }
        Vector3 GetPosition { get; }
        Transform GetTransform { get; }
        bool IsSelected { get; }

        void Select();
        void Unselect();
        void ExecuteCommand(iCommand command);
    }

    public enum ObjectTypes { ControlledObject, EnemyObject }
}



