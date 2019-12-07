using Paint.Grid.Interaction;
using Paint.Grid.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace Paint.Grid
{
    public class GridTest : MonoBehaviour
    {
        public GridGizmoDrawer GridGizmoDrawer;
        public LayerMask GroundLayer;

        private List<GameObject> m_PathVisualization = new List<GameObject>();

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

                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                            TestInteractableObject obj = cube.AddComponent<TestInteractableObject>();
                            cube.transform.position = cellPos;
                            cube.transform.localScale *= 0.5f;

                            cell.AddObject(obj);

                            obj.OnUpdatePosition += UpdatePosition;
                            obj.DistanceToUpdate = cell.CellSize;
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
                    GridCell cell = m_Grid.GetCellByCoord(coord.x, coord.y);

                    if (!cell.HasObject && cell.CellType == GridCell.CellTypes.Normal)
                    {
                        TestInteractableObject obj = m_SelectedObject as TestInteractableObject;
                        obj.SetMovePosition(m_Grid.GetCellWorldPosByCoord(cell.X, cell.Y));

                        List<GridCell> path = m_Grid.FindPath(m_Grid.GetCellByWorldPos(obj.transform.position), cell);
                        if (path != null)
                        {
                            if (m_PathVisualization.Count > 0)
                            {
                                for (int i = 0; i < m_PathVisualization.Count; i++)
                                    Destroy(m_PathVisualization[i]);

                                m_PathVisualization.Clear();
                            }

                            for (int i = 0; i < path.Count; i++)
                            {
                                GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                p.transform.position = m_Grid.GetCellWorldPosByCoord(path[i].X, path[i].Y);
                                p.transform.localScale *= 0.2f;
                                p.GetComponent<Renderer>().material.color = Color.red;

                                m_PathVisualization.Add(p);
                            }
                        }
                    }
                }
            }
        }


        void UpdatePosition(Vector3 prevPos, Vector3 curPos, iMovableObject sender)
        {
            GridCell fromCell = m_Grid.GetCellByWorldPos(prevPos);
            GridCell toCell = m_Grid.GetCellByWorldPos(curPos);

            if (!fromCell.HasEqualCoord(toCell))
            {
                fromCell.RemoveObject();
                toCell.AddObject(sender);

                if (m_PathVisualization != null)
                {
                    Destroy(m_PathVisualization[0]);
                    m_PathVisualization.RemoveAt(0);
                }
            }
        }

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
