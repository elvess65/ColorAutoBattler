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

            Color color = Color.white;
            switch (cell.CellType)
            {
                case GridCell.CellTypes.Normal:
                    color = Color.white;
                    break;
                case GridCell.CellTypes.LowObstacle:
                    color = Color.blue;
                    break;
                case GridCell.CellTypes.HighObstacle:
                    color = Color.red;
                    break;
                case GridCell.CellTypes.FinishPathCell:
                    color = Color.yellow;
                    break;
            }

            Gizmos.color = cell.HasObject ? Color.green : color; 

            Gizmos.DrawWireSphere(pos, cell.CellSize / 2);
        }
    }
}
