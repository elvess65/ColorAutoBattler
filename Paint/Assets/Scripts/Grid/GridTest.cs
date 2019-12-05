using UnityEngine;

namespace Paint.Grid
{
    public class GridTest : MonoBehaviour
    {
        public GridGizmoDrawer GridGizmoDrawer;
        public LayerMask GroundLayer;

        private GridController m_Grid;

        void Start()
        {
            m_Grid = new GridController(5, 5, 1, Vector2.zero);
            GridGizmoDrawer.SetGrid(m_Grid);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, GroundLayer))
                {
                    (int x, int y) coord = m_Grid.GetCellCoordByWorldPos(hitInfo.point);
                    if (m_Grid.CoordIsOnGrid(coord.x, coord.y))
                    {
                        Vector3 cellPos = m_Grid.GetCellWorldPositionByCoord(coord.x, coord.y);
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = cellPos;
                        cube.transform.localScale *= 0.5f;
                    }
                }
            }
        }

    }
}
