using System;
using Paint.Commands;
using Paint.Grid;
using Paint.Logic;
using Paint.Movement;
using Paint.Objects.Interfaces;
using UnityEngine;

namespace Paint.Objects
{
    public class TestUnitObject : MonoBehaviour, iMovableObject, iBattleObject
    {
        private iMoveStrategy m_MoveStrategy;

        public void Init(ObjectTypes objectType, iMoveStrategy moveStrategy)
        {
            //Object
            ObjectType = objectType;

            //Movement
            m_MoveStrategy = moveStrategy;
            m_MoveStrategy.OnUpdatePosition += (Vector3 positionAtLastPoint, bool movementFinished) =>
            {
                //Anyway call position update event
                OnUpdatePosition?.Invoke(positionAtLastPoint, transform.position, this);

                //Call only when movement was finished
                if (movementFinished)
                {
                    OnReleaseTargetCell?.Invoke(m_TargetCellCoord.x, m_TargetCellCoord.y);

                    m_OnMovementFinished?.Invoke();
                    m_OnMovementFinished = null;
                }
            };
        }

        void Update()
        {
            if (m_MoveStrategy != null && m_MoveStrategy.IsMoving)
                m_MoveStrategy.Update(Time.deltaTime);

            ProcessAttack();
        }


        //iInteractable
        public ObjectTypes ObjectType { get; private set; }
        public Vector3 GetPosition => m_MoveStrategy.GetPosition;
        public Transform GetTransform => m_MoveStrategy.GetTransform;
        public bool IsSelected { get; private set; }

        public void Select()
        {
            if (IsSelected)
                return;

            IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void Unselect()
        {
            if (!IsSelected)
                return;

            IsSelected = false;
            GetComponent<Renderer>().material.color = Color.white;
        }

        public void ExecuteCommand(iCommand command)
        {
            Debug.Log("Execute command: " + command.CommandType);
            switch(command.CommandType)
            {
                case CommandTypes.MoveCommand:
                    MoveCommand moveCommand = command as MoveCommand;
                    SetMovePosition(moveCommand.MovePos, moveCommand.TargetCellCoord.x, moveCommand.TargetCellCoord.y, moveCommand.OnMovementFinished);
                    break;

                case CommandTypes.AttackCommand:

                    AttackCommand attackCommand = command as AttackCommand;

                    m_Target = attackCommand.Target;

                    GridCell fromCell = GridTest.Instance.GridController.GetCellByWorldPos(GetPosition); 
                    GridCell targetCell = GridTest.Instance.GridController.GetCellByWorldPos(m_Target.GetPosition); 
                    if (!TargetIsInAttackRange(fromCell, targetCell))
                    {
                        Debug.Log("Target is out of range. Trying to find closest walkable cell");

                        GridCell closestCell = GridTest.Instance.GridController.GetClosestWalkableCell(fromCell, targetCell, 1.5f);
                        if (closestCell != null)
                        {
                            iCommand subCommand = new MoveCommand(GridTest.Instance.GridController.GetCellWorldPosByCoord(closestCell.X, closestCell.Y), 
                                closestCell.X, closestCell.Y, () => Debug.Log("Movement finished"));
                            ExecuteCommand(subCommand);
                        }
                    }
                    else
                        Debug.Log("Player is in attack range");

                    break;
            }
        }

        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition; //Событие обновления позиции объекта при перемещении
        public event Action<int, int> OnSetTargetCell;                          //Событие сохранения координат конечной ячейки
        public event Action<int, int> OnReleaseTargetCell;                      //Событие освобождения координат конечной ячейки

        private event Action m_OnMovementFinished;                              //Внутренее событие окончания движения

        private (int x, int y) m_TargetCellCoord;


        void SetMovePosition(Vector3 movePos, int x, int y, Action onMovementFinishedFunc)
        {
            if (m_MoveStrategy.MoveToPosition(movePos))
            {
                m_TargetCellCoord = (x, y);
                m_OnMovementFinished += onMovementFinishedFunc;

                OnSetTargetCell?.Invoke(x, y);
            }
        }

        void StopMovement() => m_MoveStrategy.StopMovement();


        //iBattleObject
        private iBattleObject m_Target;

        bool TargetIsInAttackRange(GridCell fromCell, GridCell targetCell) => (int)GridTest.Instance.GridController.GetDistanceBetweenCells(fromCell, targetCell) == 1;

        void ProcessAttack()
        {
            if (m_Target != null)
            {
            }
        }
    }
}