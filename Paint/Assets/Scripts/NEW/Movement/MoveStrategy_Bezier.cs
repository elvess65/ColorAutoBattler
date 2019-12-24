using FrameworkPackage.PathCreation;
using PathCreation;
using System;
using UnityEngine;

namespace Paint.Movement
{
    public class MoveStrategy_Bezier : iMoveStrategy
    {
        public event Action<Vector3, bool> OnUpdatePosition;

        public bool IsMoving => m_MovePathController.IsMoving;
        public Vector3 GetPosition => m_MovePathController.ControlledTransform.position;
        public Transform GetTransform => m_MovePathController.ControlledTransform;

        //Movement
        private MovePathController m_MovePathController;
        private PathCreator m_PathVisualizer;
        private Func<Vector3, Vector3, Vector3[]> m_GetPathFunc;
        private Vector3 m_PositionAtLastPoint;

        //Update position
        private float m_CurDistToUpdate;
        private float m_CellPositionUpdateDist;
        private float m_PassedDistanceSinceLastPoint;

        private const float m_FIRST_UPDATE_DISTANCE_MULTIPLAYER = 0.6f;


        public MoveStrategy_Bezier(Transform controlledObject, Func<Vector3, Vector3, Vector3[]> getPathFunc, float cellPositionUpdateDist)
        {
            m_MovePathController = new MovePathController(controlledObject);
            m_MovePathController.OnMovementFinished += () =>
            {
                OnUpdatePosition?.Invoke(m_PositionAtLastPoint, true);
                MonoBehaviour.Destroy(m_PathVisualizer);
            };
            m_GetPathFunc = getPathFunc;

            m_CellPositionUpdateDist = cellPositionUpdateDist;
        }


        public bool MoveToPosition(Vector3 targetPos)
        {
            Vector3[] path = m_GetPathFunc(m_MovePathController.ControlledTransform.position, targetPos);
            if (path != null)
            {
                //Создать кривую по массиву точек
                BezierPath bezierPath = GenerateBezierPath(path);
                VertexPath vertexPath = GenerateVertexPath(path);

                //Данные для обновления позиции при смене ячеек
                m_CellPositionUpdateDist = vertexPath.length / (path.Length - 1);
                m_CurDistToUpdate = m_CellPositionUpdateDist * m_FIRST_UPDATE_DISTANCE_MULTIPLAYER;
                m_PassedDistanceSinceLastPoint = 0;

                //Текущая опорная позиция
                m_PositionAtLastPoint = m_MovePathController.ControlledTransform.position;

                //Отображение пути
                m_PathVisualizer = m_MovePathController.ControlledTransform.gameObject.AddComponent<PathCreator>();
                m_PathVisualizer.bezierPath = bezierPath;

                //Начать движение
                m_MovePathController.StartMovement(vertexPath, 1f);

                return true;
            }

            return false;
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

                    OnUpdatePosition?.Invoke(m_PositionAtLastPoint, false);

                    //Текущая опорная позиция
                    m_PositionAtLastPoint = m_MovePathController.ControlledTransform.position;
                }
            }
        }


        BezierPath GenerateBezierPath(Vector3[] points) => new BezierPath(points, false, PathSpace.xyz);

        VertexPath GenerateVertexPath(Vector3[] points)
        {
            // Create a closed, 2D bezier path from the supplied points array
            // These points are treated as anchors, which the path will pass through
            // The control points for the path will be generated automatically
            //BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            // Then create a vertex path from the bezier path, to be used for movement etc
            return new VertexPath(GenerateBezierPath(points));
        }
    }
}