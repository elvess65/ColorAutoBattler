using UnityEngine;

namespace Paint.Grid
{
    public class GridGizmoDrawer : MonoBehaviour
    {
        private GridController m_Grid;

        public void SetGrid(GridController grid) => m_Grid = grid;

        private void OnDrawGizmos()
        {
            if (m_Grid != null)
                m_Grid.ForEachCell(DrawGizmoForCell);
        }

        void DrawGizmoForCell(GridCell cell)
        {
            Vector3 pos = m_Grid.GetCellWorldPosByCoord(cell.X, cell.Y);

            switch (cell.CellType)
            {
                case GridCell.CellTypes.Normal:
                    Gizmos.color = Color.white;
                    break;
                case GridCell.CellTypes.LowObstacle:
                    Gizmos.color = Color.blue;
                    break;
                case GridCell.CellTypes.HighObstacle:
                    Gizmos.color = Color.red;
                    break;
            }

            Gizmos.DrawWireSphere(pos, cell.CellSize / 2);
        }
    }
}
