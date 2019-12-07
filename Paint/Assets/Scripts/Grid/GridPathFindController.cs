using System.Collections.Generic;
using UnityEngine;

namespace Paint.Grid
{
    public class GridPathFindController 
    {
        private GridController m_GridController;

        public GridPathFindController(GridController gridController) => m_GridController = gridController;

        /// <summary>
        /// Поиск пути
        /// </summary>
        public List<GridCell> FindPath(GridCell startNode, GridCell targetNode)
        {
            //Create open and close sets
            List<GridCell> openSet = new List<GridCell>();
            HashSet<GridCell> closedSet = new HashSet<GridCell>();

            //Add start node to open set
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                //Cur node is the node with the lowest FCost
                GridCell curNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < curNode.FCost || openSet[i].FCost == curNode.FCost && openSet[i].HCost < curNode.HCost)
                        curNode = openSet[i];
                }

                //Remove cur form open
                openSet.Remove(curNode);

                //Add cur to closed set
                closedSet.Add(curNode);

                //if cur note is the target node
                if (curNode == targetNode)
                    return RetracePath(startNode, targetNode);

                //Get neighbours
                (int x, int y)[] neighbours = m_GridController.GetCellNeighboursCoord(curNode.X, curNode.Y);
                for (int i = 0; i < neighbours.Length; i++)
                {
                    GridCell neighbourNode = m_GridController.GetCellByCoord(neighbours[i].x, neighbours[i].y);

                    bool cellTypeIsIgnorable = neighbourNode.CellType != GridCell.CellTypes.Normal;

                    //if neighbour is not walkable or is in closed set - skip
                    if (cellTypeIsIgnorable || closedSet.Contains(neighbourNode))
                        continue;

                    int newGCostToNeighbour = curNode.GCost + GetDistanceForPathFinding(curNode, neighbourNode);
                    if (newGCostToNeighbour < neighbourNode.GCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = newGCostToNeighbour;
                        neighbourNode.HCost = GetDistanceForPathFinding(neighbourNode, targetNode);

                        neighbourNode.ParentNode = curNode;

                        if (!openSet.Contains(neighbourNode))
                            openSet.Add(neighbourNode);
                    }
                }
            }

            return null;
        }


        List<GridCell> RetracePath(GridCell startNode, GridCell targetNode)
        {
            List<GridCell> path = new List<GridCell>();

            GridCell curNode = targetNode;
            while (curNode != startNode)
            {
                path.Add(curNode);
                curNode = curNode.ParentNode;
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }

        int GetDistanceForPathFinding(GridCell a, GridCell b)
        {
            Vector2Int aCoord = a.CoordAsVec2Int;
            Vector2Int bCoord = b.CoordAsVec2Int;

            int distX = Mathf.Abs(aCoord.x - bCoord.x);
            int distY = Mathf.Abs(aCoord.y - bCoord.y);

            if (distX > distY)
                return 10 * distY + 10 * (distX - distY);

            return 10 * distX + 10 * (distY - distX);
        }
    }
}
