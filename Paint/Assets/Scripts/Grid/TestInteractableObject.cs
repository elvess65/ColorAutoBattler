using System;
using System.Collections.Generic;
using FrameworkPackage.PathCreation;
using Paint.Grid;
using Paint.Grid.Movement;
using Paint.Movement;
using PathCreation;
using UnityEngine;

namespace Paint.Grid.Interaction
{
    public class TestInteractableObject : MonoBehaviour, iMovableObject
    {
        public void Init(float cellPositionUpdateDist, GridController gridController)
        {
            m_MoveStrategy = new MoveStrategy_Bezier(transform, gridController, cellPositionUpdateDist);
            m_MoveStrategy.OnUpdatePosition += (Vector3 positionAtLastPoint) => OnUpdatePosition?.Invoke(positionAtLastPoint, transform.position, this);
        }


        //iInteractable
        public bool IsSelected { get; private set; }

        public void Select()
        {
            if (IsSelected)
                return;

            IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;
        }

        public void Unselect()
        {
            if (!IsSelected)
                return;

            IsSelected = false;
            GetComponent<Renderer>().material.color = Color.white;
        }


        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;

        private iMoveStrategy m_MoveStrategy;

        public void SetMovePosition(Vector3 movePos) => m_MoveStrategy.MoveToPosition(movePos);
    

        void Update()
        {
            if (m_MoveStrategy != null && m_MoveStrategy.IsMoving)
                m_MoveStrategy.Update(Time.deltaTime);
        }
    }
}

public class MoveStrategy_Bezier : iMoveStrategy
{
    public event Action<Vector3> OnUpdatePosition;

    public bool IsMoving => m_MovePathController.IsMoving;

    //Movement
    private MovePathController m_MovePathController;
    private Vector3 m_PositionAtLastPoint;
    private GridController m_Grid;

    //Update position
    private float m_CurDistToUpdate;
    private float m_CellPositionUpdateDist;
    private float m_PassedDistanceSinceLastPoint;

    private const float m_FIRST_UPDATE_DISTANCE_MULTIPLAYER = 0.6f;


    public MoveStrategy_Bezier(Transform controlledObject, GridController grid, float cellPositionUpdateDist)
    {
        m_MovePathController = new MovePathController(controlledObject);
        m_MovePathController.OnMovementFinished += () => OnUpdatePosition?.Invoke(m_PositionAtLastPoint);

        m_CellPositionUpdateDist = cellPositionUpdateDist;
        m_Grid = grid;
    }

    public void MoveToPosition(Vector3 targetPos)
    {
        List<GridCell> path = m_Grid.FindPath(m_Grid.GetCellByWorldPos(m_MovePathController.ControlledTransform.position),
                                              m_Grid.GetCellByWorldPos(targetPos));
        if (path != null)
        {
            //Создать массив позиций, из которых состоит путь
            List<Vector3> pathPos = new List<Vector3>();
            for (int i = 0; i < path.Count; i++)
                pathPos.Add(m_Grid.GetCellWorldPosByCoord(path[i].X, path[i].Y));

            //Исправление ошибки с путем состоящим из двух точек
            if (pathPos.Count == 2)
                pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

            //Создать кривую по массиву точек
            VertexPath vertexPath = GeneratePath(pathPos.ToArray());

            //Данные для обновления позиции при смене ячеек
            m_CellPositionUpdateDist = vertexPath.length / (path.Count - 1);
            m_CurDistToUpdate = m_CellPositionUpdateDist * m_FIRST_UPDATE_DISTANCE_MULTIPLAYER;
            m_PassedDistanceSinceLastPoint = 0;

            //Текущая опорная позиция
            m_PositionAtLastPoint = m_MovePathController.ControlledTransform.position;

            //Начать движение
            m_MovePathController.StartMovement(vertexPath, 1f);
        }
    }

    public void StopMovement() => m_MovePathController.StopMovement();

    public void Update(float deltaTime)
    {
        if (IsMoving)
        {
            m_MovePathController.Update(deltaTime);

            //Обновление позиции ячейки
            float distTravelled = m_MovePathController.DistanceTravelled - m_PassedDistanceSinceLastPoint;

            if (distTravelled >= m_CurDistToUpdate)
            {
                //Каждый раз при прохождении необходимой для обновления дистанции "расстояние с последнего обновления" задаеться текущему пройденному расстоянию.
                m_PassedDistanceSinceLastPoint = m_MovePathController.DistanceTravelled;
                //Текущее расстояние для обновления всегда после первого обновления изменяется на значение по-умолчанию (первое значение всегда меньше) 
                m_CurDistToUpdate = m_CellPositionUpdateDist;

                OnUpdatePosition?.Invoke(m_PositionAtLastPoint);

                //Текущая опорная позиция
                m_PositionAtLastPoint = m_MovePathController.ControlledTransform.position;
            }
        }
    }


    VertexPath GeneratePath(Vector3[] points)
    {
        // Create a closed, 2D bezier path from the supplied points array
        // These points are treated as anchors, which the path will pass through
        // The control points for the path will be generated automatically
        BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
        // Then create a vertex path from the bezier path, to be used for movement etc
        return new VertexPath(bezierPath);
    }
}