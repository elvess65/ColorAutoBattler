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
        //iInteractable
        public bool IsSelected { get; private set; }

        public void Select()
        {
            if (IsSelected)
                return;

            IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;

            Debug.Log("Select " + gameObject.name);
        }

        public void Unselect()
        {
            if (!IsSelected)
                return;

            IsSelected = false;
            GetComponent<Renderer>().material.color = Color.white;

            Debug.Log("Unselect " + gameObject.name);
        }


        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;

        private iMoveStrategy m_MoveStrategy;


        public void SetMovePosition(Vector3 movePos, GridController gridController, float d)
        {
            m_MoveStrategy = new MoveStrategy_Bezier(transform, gridController);
            m_MoveStrategy.DistanceToUpdate = d;
            m_MoveStrategy.MoveToPosition(movePos);
        }
    

        void Update()
        {
            if (m_MoveStrategy != null && m_MoveStrategy.IsMoving)
                m_MoveStrategy.Update(Time.deltaTime);
        }
    }
}

public class MoveStrategy_Bezier : iMoveStrategy
{
    public event Action OnUpdatePosition;

    public float DistanceToUpdate { get; set; }
    public bool IsMoving { get; private set; }

    //Movement
    private MovePathController m_MovePathController;
    private Vector3 m_AnchorPos;

    //Update position
    private float m_CurDistToUpdate;
    private float m_PassedDistanceSinceLastPoint;
    private GridController m_Grid;


    private const float m_FIRST_UPDATE_DISTANCE_MULTIPLAYER = 0.6f;


    public MoveStrategy_Bezier(Transform controlledObject, GridController grid)
    {
        m_MovePathController = new MovePathController(controlledObject);
        m_MovePathController.OnMovementFinished += MovementFinished;
        m_Grid = grid;
    }

    public void MoveToPosition(Vector3 targetPos)
    {
        VertexPath vertexPath = null;
        List<GridCell> path = m_Grid.FindPath(m_Grid.GetCellByWorldPos(m_MovePathController.ControlledTransform.position),
                                              m_Grid.GetCellByWorldPos(targetPos));
        if (path != null)
        {
            //Создать массив позиций, из которіх состоит путь
            List<Vector3> pathPos = new List<Vector3>();
            for (int i = 0; i < path.Count; i++)
                pathPos.Add(m_Grid.GetCellWorldPosByCoord(path[i].X, path[i].Y));

            //Исправление ошибки с путем состоящим из двух точек
            if (pathPos.Count == 2)
                pathPos.Insert(1, (pathPos[0] + pathPos[1]) / 2);

            vertexPath = GeneratePath(pathPos.ToArray());
        }

        int pointsAtPath = path.Count;

        m_AnchorPos = m_MovePathController.ControlledTransform.position;

        DistanceToUpdate = vertexPath.length / (pointsAtPath - 1);

        m_CurDistToUpdate = DistanceToUpdate * m_FIRST_UPDATE_DISTANCE_MULTIPLAYER;
        m_PassedDistanceSinceLastPoint = 0;

        Debug.Log("VERTEX PATH LENGTH: " + vertexPath.length + " Points at path: " + pointsAtPath + " DISTANCE TO UPDATE: " + DistanceToUpdate);

        m_MovePathController.StartMovement(vertexPath, 1f);
        IsMoving = true;
    }

    public void StopMove()
    {
        throw new NotImplementedException();
    }

    public void Update(float deltaTime)
    {
        if (IsMoving)
        {
            m_MovePathController.Update(deltaTime);

            //Обновление позиции ячейки
            float distTravelled = m_MovePathController.DistanceTravelled - m_PassedDistanceSinceLastPoint;
            Debug.Log(distTravelled + " " + m_PassedDistanceSinceLastPoint);
            if (distTravelled >= m_CurDistToUpdate)
            {
                m_PassedDistanceSinceLastPoint = m_MovePathController.DistanceTravelled;
                m_CurDistToUpdate = DistanceToUpdate;

                OnUpdatePosition?.Invoke();
            }
        }
    }


    void MovementFinished()
    {
        IsMoving = false;
        OnUpdatePosition?.Invoke();
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