using UnityEngine;

namespace Paint.Grid
{
    public class GridGizmoDrawer : MonoBehaviour
    {
        public Transform Cube;

        private GridController m_Grid;

        public void SetGrid(GridController grid) => m_Grid = grid;

        private void OnDrawGizmos()
        {
            if (m_Grid != null)
                m_Grid.ForEachCell(DrawGizmoForCell);
        }

        void DrawGizmoForCell(GridCell cell)
        {
            Vector3 pos = m_Grid.GetCellWorldPositionByCoord(cell.X, cell.Y);
            Gizmos.DrawWireSphere(pos, cell.CellSize / 2);
        }
    }
}
