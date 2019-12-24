using System;
using Paint.Grid;
using UnityEngine;

namespace Paint.Inputs
{
    public class StandartInputManager : iInputManager
    {
        public event Action<GridCell> OnInputResult;

        private LayerMask m_GroundLayer;
        private GridController m_GridController;

        public StandartInputManager(LayerMask groundLayer, GridController gridController)
        {
            m_GroundLayer = groundLayer;
            m_GridController = gridController;
        }

        public void ProcessUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastInGrid(out (int x, int y) coord))
                    OnInputResult?.Invoke(m_GridController.GetCellByCoord(coord.x, coord.y));
            }
        }


        bool RaycastInGrid(out (int x, int y) coord)
        {
            coord = (0, 0);

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, m_GroundLayer))
            {
                coord = m_GridController.GetCellCoordByWorldPos(hitInfo.point);
                return m_GridController.CoordIsOnGrid(coord.x, coord.y);
            }

            return false;
        }
    }
}
