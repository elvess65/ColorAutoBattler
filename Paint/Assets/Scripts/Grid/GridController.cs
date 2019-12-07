﻿using Paint.Grid.Interaction;
using System.Collections.Generic;
using UnityEngine;

namespace Paint.Grid
{
    public class GridController
    {
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }

        private float m_CellSize;
        private Vector3 m_Offset;
        private GridCell[,] m_Grid;
        private GridPathFindController m_GridPathFindController;

        private List<GridCell> m_NormalCells;


        public GridController(int widthInCells, int heightInCells, float cellSize, Vector2 _offset, int percentOfLowObstacles, int mercentOfHighObstacles)
        {
            GridWidth = widthInCells;
            GridHeight = heightInCells;
            m_CellSize = cellSize;
            m_Offset = new Vector3(_offset.x, 0, _offset.y);

            m_Grid = new GridCell[GridWidth, GridHeight];
            m_NormalCells = new List<GridCell>();
            m_GridPathFindController = new GridPathFindController(this);

            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    GridCell gridCell = new GridCell(i, j, m_CellSize, GridCell.CellTypes.Normal);
                    m_Grid[i, j] = gridCell;
                    m_NormalCells.Add(gridCell);
                }
            }

            int amountOfLowObstacles = m_NormalCells.Count * percentOfLowObstacles / 100;
            int amountOfHighObstacles = m_NormalCells.Count * mercentOfHighObstacles / 100;

            while (amountOfLowObstacles > 0)
            {
                int randomIndex = Random.Range(0, m_NormalCells.Count);

                GridCell cell = m_NormalCells[randomIndex];
                m_NormalCells.RemoveAt(randomIndex);
                amountOfLowObstacles--;

                cell.SetCellType(GridCell.CellTypes.LowObstacle);
                CreateDummyObstacle(PrimitiveType.Sphere, GetCellWorldPosByCoord(cell.X, cell.Y));

            }

            while (amountOfHighObstacles > 0)
            {
                int randomIndex = Random.Range(0, m_NormalCells.Count);

                GridCell cell = m_NormalCells[randomIndex];
                m_NormalCells.RemoveAt(randomIndex);
                amountOfHighObstacles--;

                cell.SetCellType(GridCell.CellTypes.HighObstacle);
                CreateDummyObstacle(PrimitiveType.Cylinder, GetCellWorldPosByCoord(cell.X, cell.Y));
            }
        }

        public List<GridCell> FindPath(GridCell startNode, GridCell targetNode) => m_GridPathFindController.FindPath(startNode, targetNode);


        /// <summary>
        /// Получить ячейку по координатам сетки
        /// </summary>
        public GridCell GetCellByCoord(int x, int y)
        {
            if (CoordIsOnGrid(x, y))
                return m_Grid[x, y];

            return null;
        }

        /// <summary>
        /// Получить ячейку по расположению
        /// </summary>
        public GridCell GetCellByWorldPos(Vector3 pos)
        {
            (int x, int y) coord = GetCellCoordByWorldPos(pos);
            return GetCellByCoord(coord.x, coord.y);
        }


        /// <summary>
        /// Получить расположение ячейки по координатам сетки
        /// </summary>
        public Vector3 GetCellWorldPosByCoord(int x, int y)
        {
            Vector3 pos = new Vector3(x, 0, y) * m_CellSize + m_Offset;
            pos.x += m_CellSize / 2;
            pos.z += m_CellSize / 2;

            return pos;
        }

        /// <summary>
        /// Получить координаты клетки по расположению
        /// </summary>
        public (int x, int y) GetCellCoordByWorldPos(Vector3 pos) => (Mathf.FloorToInt((pos - m_Offset).x / m_CellSize), Mathf.FloorToInt((pos - m_Offset).z / m_CellSize));


        /// <summary>
        /// Список координат 4 соседей для указанной координаты
        /// </summary>
        public (int x, int y)[] GetCell4NeighboursCoord(int x, int y)
        {
            //Если координата, для которой нужно получить список соседей не находится в пределах сетки - вернуть пустой список
            if (!CoordIsOnGrid(x, y))
                return new (int x, int y)[0];

            //Список соседних координат относительно указанной
            List<(int x, int y)> cellNeighbours = new List<(int x, int y)>();

            //Добавить координаты соседних клеток, если они находятся в пределах сетки
            (int x, int y) neighbouCoord = (x - 1, y);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x + 1, y);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x, y - 1);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            neighbouCoord = (x, y + 1);
            if (CoordIsOnGrid(neighbouCoord.x, neighbouCoord.y))
                cellNeighbours.Add(neighbouCoord);

            return cellNeighbours.ToArray();
        }

        /// <summary>
        /// Выполнить событие для каждой ячейки
        /// </summary>
        /// <param name="forEachEvent">Событие, которое должно выполняться для каждой ячеки</param>
        public void ForEachCell(System.Action<GridCell> forEachEvent)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                    forEachEvent(m_Grid[i, j]);
            }
        }


        /// <summary>
        /// Находится ли указанная координата в пределах сетки
        /// </summary>
        public bool CoordIsOnGrid(int x, int y) => (x >= 0 && x < GridWidth) && (y >= 0 && y < GridHeight);


        void CreateDummyObstacle(PrimitiveType type, Vector3 pos)
        {
            GameObject cube = GameObject.CreatePrimitive(type);
            cube.transform.position = pos;
            cube.transform.localScale *= 0.5f;
        }
    }


    public class GridCell
    {
        public enum CellTypes { Normal, LowObstacle, HighObstacle }
        
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
