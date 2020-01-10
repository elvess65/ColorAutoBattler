using System;
using Paint.Commands;
using Paint.Grid;
using Paint.Logic;
using Paint.Objects.Interfaces;
using UnityEngine;

namespace Paint.Processors
{
    public class StandartObjectBattleProcessor : iObjectBattleProcessor
    {
        public event Action<iBattleObject> OnAttack;

        private int m_AttackDistance;                               //Дистанция атаки
        private float m_LeftTimeToDistanceCheck;                    //Оставшееся время до следующей проверки расстояния
        private iMovableObject m_ControlledObject;                  //Контролируемый объект
        private iBattleObject m_Target;                             //Цель
        private GridCell m_TargetLastCell = null;                   //Последняя позиция цели

        private const float m_DIAGONAL_CELL_DIST_OFFSET = 0.5f;     //Смещение для проверки расстояния по диагонали 
        private const float m_DISTANCE_CHECK_TIME = 0.5f;           //Время проверки расстояния

        public bool HasTarget => m_Target != null;


        public StandartObjectBattleProcessor(iMovableObject controlledObject, int attackDist)
        {
            m_ControlledObject = controlledObject;
            m_AttackDistance = attackDist;
            m_LeftTimeToDistanceCheck = 0;
        }


        public void SetTarget(iBattleObject target) => m_Target = target;

        public void ClearTarget()
        {
            m_Target = null;
            m_LeftTimeToDistanceCheck = 0;
        }

        public void ProcessAttack()
        {
            if (HasTarget)
            {
                m_LeftTimeToDistanceCheck -= Time.deltaTime;

                if (m_LeftTimeToDistanceCheck <= 0)
                {
                    m_LeftTimeToDistanceCheck = m_DISTANCE_CHECK_TIME;
                    TryAttackTarget();
                }
            }
        }


        void TryAttackTarget()
        {
            if (CanAttackTarget(m_Target))
                OnAttack?.Invoke(m_Target);
        }

        bool CanAttackTarget(iBattleObject target)
        {
            GridCell fromCell = GridTest.Instance.GridController.GetCellByWorldPos(m_ControlledObject.GetPosition);
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

                        m_ControlledObject.ExecuteCommand(subCommand);

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
