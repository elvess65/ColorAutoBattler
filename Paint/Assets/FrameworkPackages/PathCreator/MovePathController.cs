using PathCreation;
using UnityEngine;

namespace FrameworkPackage.PathCreation
{
    /// <summary>
    /// Перемещение по указанному пути
    /// </summary>
    public class MovePathController
    {
        public event System.Action OnMovementFinished;

        private VertexPath m_VertexPath;
        private bool m_IsStarted = false;
        private float m_Speed;
        private float m_SpeedMltp;

        public Transform ControlledTransform { get; set; }
        public float DistanceTravelled { get; private set; }


        public MovePathController(Transform controlledTransform) => ControlledTransform = controlledTransform;

        /// <summary>
        /// Начать передвигаться по пути
        /// </summary>
        public void StartMovement(VertexPath vertexPath, float speed, float speedMltp = 1)
        {
            if (ControlledTransform == null)
            {
                Debug.LogError("FrameworkPackage -> MovePathController: ERROR: Controlled Transform is not set");
                return;
            }

            SetSpeedMultiplyer(speedMltp);

            m_VertexPath = vertexPath;
            DistanceTravelled = 0;
            m_Speed = speed;

            m_IsStarted = true;
        }

        /// <summary>
        /// Остановить движение по пути
        /// </summary>
        public void StopMovement() => m_IsStarted = false;

        /// <summary>
        /// Изменить множитель времени
        /// </summary>
        public void SetSpeedMultiplyer(float speedMltp) => m_SpeedMltp = speedMltp;

        public void Update(float deltaTime)
        {
            if (m_IsStarted)
            {
                DistanceTravelled += deltaTime * m_Speed * m_SpeedMltp;

                ControlledTransform.position = m_VertexPath.GetPointAtDistance(DistanceTravelled, EndOfPathInstruction.Stop);
                ControlledTransform.rotation = m_VertexPath.GetRotationAtDistance(DistanceTravelled, EndOfPathInstruction.Stop);

                if (DistanceTravelled >= m_VertexPath.length)
                {
                    m_IsStarted = false;
                    OnMovementFinished?.Invoke();
                }

                //Quaternion rot = m_PathCreator.path.GetRotation(t / tt);
                //Debug.Log(rot.eulerAngles);
                //transform.localEulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y - 90, rot.eulerAngles.z);
                //transform.rotation = rot * Quaternion.Euler(0, -90, 0);
            }
        }
    }
}
