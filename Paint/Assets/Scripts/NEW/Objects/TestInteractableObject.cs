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
                    ReleaseTargetCell();

                    m_OnMovementFinished?.Invoke();
                    m_OnMovementFinished = null;
                }
            };

            //Battle
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

                    break;
            }
        }


        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition; //Событие обновления позиции объекта при перемещении
        public event Action<int, int> OnSetTargetCell;                          //Событие сохранения координат конечной ячейки
        public event Action<int, int> OnReleaseTargetCell;                      //Событие освобождения координат конечной ячейки

        private event Action m_OnMovementFinished;                              //Внутренее событие окончания движения
        private (int x, int y) m_TargetCellCoord = (-1, -1);                    //Целевая ячейка перемещения (чтобы освободить по прибитии)


        void SetMovePosition(Vector3 movePos, int x, int y, Action onMovementFinishedFunc)
        {
            if (m_MoveStrategy.MoveToPosition(movePos))
            {
                //Освободить целевую ячейку если она не была освобождена до начала перемещения
                if (!TargetCellIsReleased())
                    ReleaseTargetCell();

                m_TargetCellCoord = (x, y);
                m_OnMovementFinished += onMovementFinishedFunc;

                OnSetTargetCell?.Invoke(x, y);
            }
        }

        void StopMovement() => m_MoveStrategy.StopMovement();

        void ReleaseTargetCell()
        {
            OnReleaseTargetCell?.Invoke(m_TargetCellCoord.x, m_TargetCellCoord.y);
            m_TargetCellCoord = (-1, -1);
        }

        bool TargetCellIsReleased() => m_TargetCellCoord.x < 0 && m_TargetCellCoord.y < 0;


        //iBattleObject
        private iBattleObject m_Target;

        private float m_LeftTimeToDistanceCheck = 0;
        private float m_DistanceCheckTime = 0.5f;
        private int m_AttackDistance = 1;
        private GridCell m_TargetLastCell = null;

        private const float m_DIAGONAL_CELL_DIST_OFFSET = 0.5f;


        void ProcessAttack()
        {
            if (m_Target != null)
            {
                m_LeftTimeToDistanceCheck -= Time.deltaTime;

                if (m_LeftTimeToDistanceCheck <= 0)
                {
                    m_LeftTimeToDistanceCheck = m_DistanceCheckTime;
                    TryAttackTarget();
                }
            }
        }

        void TryAttackTarget()
        {
            if (CanAttackTarget(m_Target))
            {
                Debug.Log("Attack target");
            }
        }

        bool CanAttackTarget(iBattleObject target)
        {
            GridCell fromCell = GridTest.Instance.GridController.GetCellByWorldPos(GetPosition);
            GridCell targetCell = GridTest.Instance.GridController.GetCellByWorldPos(target.GetPosition);

            if (!TargetIsInAttackRange(fromCell, targetCell))
            {
                Debug.Log("Target is out of range");

                //Если позиция цели еще ни разу не кешировалась или цель переместилась - найти ближайшую к цели доступную для перемещения ячейку 
                if (m_TargetLastCell == null || (targetCell != m_TargetLastCell))
                {
                    Debug.Log("Trying to find closest walkable cell");

                    //Cache last target cell
                    m_TargetLastCell = targetCell;
                    
                    //Find closest cell
                    GridCell closestCell = GridTest.Instance.GridController.GetClosestWalkableCell(fromCell, targetCell, m_AttackDistance + m_DIAGONAL_CELL_DIST_OFFSET);
                    if (closestCell != null)
                    {
                        iCommand subCommand = new MoveCommand(GridTest.Instance.GridController.GetCellWorldPosByCoord(closestCell.X, closestCell.Y),
                                                              closestCell.X, closestCell.Y, 
                                                              () => TryAttackTarget());
                        ExecuteCommand(subCommand);

                        return false;
                    }
                }

                //В противном случае продолжать движение
                Debug.Log("Target didnt move. Continue movement");

                return false;
            }

            return true;
        }

        bool TargetIsInAttackRange(GridCell fromCell, GridCell targetCell) => (int)GridTest.Instance.GridController.GetDistanceBetweenCells(fromCell, targetCell) == m_AttackDistance;
    }
}