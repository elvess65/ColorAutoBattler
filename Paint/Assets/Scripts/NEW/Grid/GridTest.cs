﻿using Paint.Commands;
using Paint.Grid;
using Paint.Inputs;
using Paint.Movement;
using Paint.Objects;
using Paint.Objects.Interfaces;
using UnityEngine;

namespace Paint.Logic
{
    public class GridTest : MonoBehaviour
    {
        public static GridTest Instance;

        public GridGizmoDrawer GridGizmoDrawer;
        public LayerMask GroundLayer;

        private iInputManager m_InputManager;
        private iInteractableObject m_SelectedObject;

        private const int m_PERCENT_OF_LOW_OBSTACLES = 20;
        private const int m_PERCENT_OF_HIGH_OBSTACLES = 15;

        public GridController GridController { get; private set; }


        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            GridController = new GridController(5, 5, 1, Vector2.zero, m_PERCENT_OF_LOW_OBSTACLES, m_PERCENT_OF_HIGH_OBSTACLES);
            GridGizmoDrawer.SetGrid(GridController);

            m_InputManager = new StandartInputManager(GroundLayer, GridController);
            m_InputManager.OnInputResult += InputManager_OnInputResult;
        }

        void Update()
        {
            m_InputManager.ProcessUpdate(Time.deltaTime);

            if (Input.GetMouseButtonDown(1))
            {
                if (RaycastInGrid(out (int x, int y) coord))
                {
                    GridCell cell = GridController.GetCellByCoord(coord.x, coord.y);
                    if (cell.CellType == GridCell.CellTypes.Normal && !cell.HasObject)
                    {
                        Vector3 cellPos = GridController.GetCellWorldPosByCoord(cell.X, cell.Y);

                        //Создать агент
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        TestUnitObject obj = cube.AddComponent<TestUnitObject>();
                        obj.transform.position = cellPos;
                        obj.transform.localScale *= 0.5f;

                        iMoveStrategy moveStrategy = new MoveStrategy_Bezier(obj.transform, GridController.FindPath, cell.CellSize);

                        obj.Init(ObjectTypes.ControlledObject, moveStrategy);
                        obj.OnUpdatePosition += UpdatePosition;
                        obj.OnSetTargetCell += SetTargetCell;
                        obj.OnReleaseTargetCell += ReleaseTargetCell;

                        //Расположить агент в ячейке
                        cell.AddObject(obj);
                    }
                }
            }
             
            if (Input.GetMouseButtonDown(2))
            {
                if (RaycastInGrid(out (int x, int y) coord))
                {
                    GridCell cell = GridController.GetCellByCoord(coord.x, coord.y);
                    if (cell.CellType == GridCell.CellTypes.Normal && !cell.HasObject)
                    {
                        Vector3 cellPos = GridController.GetCellWorldPosByCoord(cell.X, cell.Y);

                        //Создать агент
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.GetComponent<MeshRenderer>().material.color = Color.red;

                        TestUnitObject obj = cube.AddComponent<TestUnitObject>();
                        obj.transform.position = cellPos;
                        obj.transform.localScale *= 0.5f;

                        iMoveStrategy moveStrategy = new MoveStrategy_Bezier(obj.transform, GridController.FindPath, cell.CellSize);

                        obj.Init(ObjectTypes.EnemyObject, moveStrategy);
                        obj.OnUpdatePosition += UpdatePosition;
                        obj.OnSetTargetCell += SetTargetCell;
                        obj.OnReleaseTargetCell += ReleaseTargetCell;

                        //Расположить агент в ячейке
                        cell.AddObject(obj);
                    }
                }
            }
        }


        void InputManager_OnInputResult(GridCell cell)
        {
            if (cell.CellType == GridCell.CellTypes.Normal)
            {
                //Empty cell click
                if (!cell.HasObject)
                {
                    //Action only for selected object
                    if (m_SelectedObject != null)
                    {
                        //Empty cell click while selected controlled object
                        if (m_SelectedObject.ObjectType == ObjectTypes.ControlledObject)
                        {
                            m_SelectedObject.ExecuteCommand(new MoveCommand(GridController.GetCellWorldPosByCoord(cell.X, cell.Y), cell.X, cell.Y));
                            UnselectObject();
                        }
                        //Empty cell click while selected not controlled object 
                        else
                        {
                            Debug.Log("Unselect not controlled object");
                            UnselectObject();
                        }
                    }
                }
                //Object cell click
                else
                {
                    iInteractableObject objectInCell = cell.GetObject();

                    //There is selected object
                    if (m_SelectedObject != null)
                    {
                        //If controlled object selected
                        if (m_SelectedObject.ObjectType == ObjectTypes.ControlledObject)
                        {
                            //If object in cell also is controlled
                            if (objectInCell.ObjectType == ObjectTypes.ControlledObject)
                            {
                                if (m_SelectedObject != objectInCell)
                                {
                                    Debug.Log("Select controlled object");
                                    SelectObject(objectInCell);
                                }
                            }
                            //If object in cell is not under control
                            else
                            {
                                switch (objectInCell.ObjectType)
                                {
                                    case ObjectTypes.EnemyObject:

                                        Debug.Log("Unselect controlled object");

                                        m_SelectedObject.ExecuteCommand(new AttackCommand(objectInCell as iBattleObject));
                                        UnselectObject();

                                        break;
                                }
                            }
                        }
                        //If selected object is not under control
                        else
                        {
                            Debug.Log("Select not controlled object");
                            SelectObject(objectInCell);
                        }
                    }
                    else
                    {
                        Debug.Log("Select object");
                        SelectObject(objectInCell);
                    }
                }
            }
        }


        void SelectObject(iInteractableObject objectInCell)
        {
            UnselectObject();

            objectInCell.Select();
            m_SelectedObject = objectInCell;
        }

        void UnselectObject()
        {
            if (m_SelectedObject != null)
                m_SelectedObject.Unselect();

            m_SelectedObject = null;
        }


        void UpdatePosition(Vector3 prevPos, Vector3 curPos, iMovableObject sender)
        {
            GridCell fromCell = GridController.GetCellByWorldPos(prevPos);
            GridCell toCell = GridController.GetCellByWorldPos(curPos);

            if (!fromCell.IsEqualCoord(toCell))
            {
                fromCell.RemoveObject();
                toCell.AddObject(sender);
            }
        }

        void ReleaseTargetCell(int x, int y) => GridController.GetCellByCoord(x, y).SetCellType(GridCell.CellTypes.Normal);

        void SetTargetCell(int x, int y) => GridController.GetCellByCoord(x, y).SetCellType(GridCell.CellTypes.FinishPathCell);

        bool RaycastInGrid(out (int x, int y) coord)
        {
            coord = (0, 0);

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, GroundLayer))
            {
                coord = GridController.GetCellCoordByWorldPos(hitInfo.point);
                return GridController.CoordIsOnGrid(coord.x, coord.y);
            }

            return false;
        }
    }
}
