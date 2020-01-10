using System;
using Paint.Commands;
using Paint.Logic;
using Paint.Movement;
using Paint.Objects.Interfaces;
using Paint.Processors;
using UnityEngine;

namespace Paint.Objects
{
    public class TestUnitObject : MonoBehaviour, iMovableObject, iBattleObject
    {
        private iMoveStrategy m_MoveStrategy;
        private iObjectBattleProcessor m_BattleProcessor;

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
            m_BattleProcessor = new StandartObjectBattleProcessor(this, m_AttackDistance);
            m_BattleProcessor.OnAttack += AttackTarget;
        }        

        void Update()
        {
            if (m_MoveStrategy != null && m_MoveStrategy.IsMoving)
                m_MoveStrategy.Update(Time.deltaTime);

            if (m_BattleProcessor != null)
                m_BattleProcessor.ProcessAttack();
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
                    m_BattleProcessor.SetTarget(attackCommand.Target);

                    break;
            }
        }


        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition; //Событие обновления позиции объекта при перемещении

        private event Action m_OnMovementFinished;                              //Внутренее событие окончания движения
        private (int x, int y) m_TargetCellCoord = (-1, -1);                    //Целевая ячейка перемещения (чтобы освободить по прибитии)


        void SetMovePosition(Vector3 movePos, int x, int y, Action onMovementFinishedFunc)
        {
            if (m_MoveStrategy.MoveToPosition(movePos))
            {
                //Освободить целевую ячейку если она не была освобождена до начала перемещения
                if (!TargetCellIsReleased())
                    ReleaseTargetCell();

                SetTargetCell(x, y);

                m_OnMovementFinished += onMovementFinishedFunc;
            }
        }

        void StopMovement() => m_MoveStrategy.StopMovement();

        void SetTargetCell(int x, int y)
        {
            GridTest.Instance.GridController.GetCellByCoord(x, y).SetCellType(Grid.GridCell.CellTypes.FinishPathCell);
            m_TargetCellCoord = (x, y);
        }

        void ReleaseTargetCell()
        {
            GridTest.Instance.GridController.GetCellByCoord(m_TargetCellCoord.x, m_TargetCellCoord.y).SetCellType(Grid.GridCell.CellTypes.Normal);
            m_TargetCellCoord = (-1, -1);
        }

        bool TargetCellIsReleased() => m_TargetCellCoord.x < 0 && m_TargetCellCoord.y < 0;


        //iBattleObject
        private int m_AttackDistance = 1;

        void AttackTarget(iBattleObject target)
        {
            Debug.Log("Attack : " + target.GetTransform.gameObject);
        }
    }
}