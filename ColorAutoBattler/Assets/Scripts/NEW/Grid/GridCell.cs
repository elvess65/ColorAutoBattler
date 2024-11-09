using Paint.Objects.Interfaces;
using UnityEngine;

namespace Paint.Grid
{
    public class GridCell
    {
        public enum CellTypes { Normal, LowObstacle, HighObstacle, FinishPathCell }

        //Base
        public int X { get; private set; }
        public int Y { get; private set; }
        public float CellSize { get; private set; }
        public CellTypes CellType { get; private set; }

        //Object
        public bool HasObject => m_Object != null;

        //Pathfinding
        public int GCost;
        public int HCost;
        public int FCost { get { return GCost + HCost; } }
        public Vector2Int CoordAsVec2Int => new Vector2Int(X, Y);
        public GridCell ParentNode;

        private iInteractableObject m_Object = null;


        public GridCell(int xCoord, int yCoord, float cellSize, CellTypes type)
        {
            X = xCoord;
            Y = yCoord;
            CellSize = cellSize;

            SetCellType(type);
        }


        public void AddObject(iInteractableObject obj) => m_Object = obj;

        public iInteractableObject GetObject() => m_Object;

        public void RemoveObject() => m_Object = null;


        public void SetCellType(CellTypes type) => CellType = type;

        public bool IsEqualCoord(GridCell otherCell) => X == otherCell.X && Y == otherCell.Y;


        public override string ToString() => $"(x: {X}. y: {Y})";
    }
}
