﻿using Paint.Grid.Interaction;
using Paint.Grid.Movement;
using UnityEngine;

namespace Paint.Grid
{
    public class GridTest : MonoBehaviour
    {
        public GridGizmoDrawer GridGizmoDrawer;
        public LayerMask GroundLayer;

        private GridController m_Grid;
        private iInteractableObject m_SelectedObject;

        private const int m_PERCENT_OF_LOW_OBSTACLES = 20;
        private const int m_PERCENT_OF_HIGH_OBSTACLES = 15;


        void Start()
        {
            m_Grid = new GridController(5, 5, 1, Vector2.zero, m_PERCENT_OF_LOW_OBSTACLES, m_PERCENT_OF_HIGH_OBSTACLES);
            GridGizmoDrawer.SetGrid(m_Grid);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastInGrid(out (int x, int y) coord))
                {
                    GridCell cell = m_Grid.GetCellByCoord(coord.x, coord.y);
                    if (cell.CellType == GridCell.CellTypes.Normal)
                    {
                        if (!cell.HasObject)
                        {
                            if (m_SelectedObject != null && m_SelectedObject.IsSelected)
                            {
                                m_SelectedObject.Unselect();
                                m_SelectedObject = null;
                            }

                            Vector3 cellPos = m_Grid.GetCellWorldPosByCoord(coord.x, coord.y);

                            //Создать агент
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            TestInteractableObject obj = cube.AddComponent<TestInteractableObject>();
                            obj.transform.position = cellPos;
                            obj.transform.localScale *= 0.5f;

                            obj.Init(cell.CellSize, m_Grid.FindPath);
                            obj.OnUpdatePosition += UpdatePosition;
                            obj.OnSetTargetCell += SetTargetCell;
                            obj.OnReleaseTargetCell += ReleaseTargetCell;

                            //Расположить агент в ячейке
                            cell.AddObject(obj);
                        }
                        else
                        {
                            if (m_SelectedObject != null && m_SelectedObject.IsSelected && m_SelectedObject != cell.GetObject())
                                m_SelectedObject.Unselect();

                            if (cell.GetObject().IsSelected)
                                cell.GetObject().Unselect();
                            else
                                cell.GetObject().Select();

                            m_SelectedObject = cell.GetObject();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (RaycastInGrid(out (int x, int y) coord))
                {
                    GridCell cell = m_Grid.GetCellByCoord(coord.x, coord.y);
                    if (cell.HasObject)
                    {
                        if (m_SelectedObject != null && m_SelectedObject.IsSelected)
                        {
                            m_SelectedObject.Unselect();
                            m_SelectedObject = null;
                        }

                        iInteractableObject iObj = cell.GetObject();
                        TestInteractableObject obj = iObj as TestInteractableObject;
                        cell.RemoveObject();
                        Destroy(obj.gameObject);
                    }
                }
            }
             
            if (Input.GetMouseButtonDown(2))
            {
                if (m_SelectedObject != null && RaycastInGrid(out (int x, int y) coord))
                {
                    //Get target cell
                    GridCell targetCell = m_Grid.GetCellByCoord(coord.x, coord.y);

                    //If cell is normal
                    if (targetCell.CellType == GridCell.CellTypes.Normal)
                    {
                        iMovableObject selectedObj = m_SelectedObject as iMovableObject;
                        Vector3 movePos = Vector3.zero;

                        //Cell has object - find nearest walkable cell
                        if (targetCell.HasObject)
                        {
                            if (targetCell.GetObject() == m_SelectedObject)
                                return;

                            GridCell fromCell = m_Grid.GetCellByWorldPos(selectedObj.GetPosition);
                            targetCell = m_Grid.GetClosestWalkableCell(fromCell, targetCell, 1.5f);

                            if (targetCell != null)
                                movePos = m_Grid.GetCellWorldPosByCoord(targetCell.X, targetCell.Y);
                            else
                                return;
                        }
                        else
                            movePos = m_Grid.GetCellWorldPosByCoord(coord.x, coord.y);

                        selectedObj.SetMovePosition(movePos, targetCell.X, targetCell.Y);
                    }
                }
            }
        }


        void UpdatePosition(Vector3 prevPos, Vector3 curPos, iMovableObject sender)
        {
            GridCell fromCell = m_Grid.GetCellByWorldPos(prevPos);
            GridCell toCell = m_Grid.GetCellByWorldPos(curPos);

            if (!fromCell.IsEqualCoord(toCell))
            {
                fromCell.RemoveObject();
                toCell.AddObject(sender);
            }
        }

        void ReleaseTargetCell(int x, int y) => m_Grid.GetCellByCoord(x, y).SetCellType(GridCell.CellTypes.Normal);

        void SetTargetCell(int x, int y) => m_Grid.GetCellByCoord(x, y).SetCellType(GridCell.CellTypes.FinishPathCell);

        bool RaycastInGrid(out (int x, int y) coord)
        {
            coord = (0, 0);

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, GroundLayer))
            {
                coord = m_Grid.GetCellCoordByWorldPos(hitInfo.point);
                return m_Grid.CoordIsOnGrid(coord.x, coord.y);
            }

            return false;
        }
    }
}
