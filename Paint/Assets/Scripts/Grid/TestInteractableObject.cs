using System;
using Paint.Grid.Movement;
using UnityEngine;

namespace Paint.Grid.Interaction
{
    public class TestInteractableObject : MonoBehaviour, iMovableObject
    {
        //iInteractable
        private bool m_IsSelected = false;
        public bool IsSelected => m_IsSelected;

        public void Select()
        {
            if (m_IsSelected)
                return;

            m_IsSelected = true;
            GetComponent<Renderer>().material.color = Color.green;

            Debug.Log("Select " + gameObject.name);
        }

        public void Unselect()
        {
            if (!m_IsSelected)
                return;

            m_IsSelected = false;
            GetComponent<Renderer>().material.color = Color.white;

            Debug.Log("Unselect " + gameObject.name);
        }


        //iMovableObject
        public event Action<Vector3, Vector3, iMovableObject> OnUpdatePosition;

        private bool m_IsMoving = false;
        private float m_CurDistToUpdate;
        private Vector3 m_TargetPos;
        private Vector3 m_AnchorPos;

        public float DistanceToUpdate { get; set; }

        public void SetMovePosition(Vector3 movePos)
        {
            m_AnchorPos = transform.position;
            m_TargetPos = movePos;

            m_CurDistToUpdate = DistanceToUpdate * 0.6f;
            m_IsMoving = true;
        }


        void Update()
        {
            if (m_IsMoving)
            {
                //Весь путь
                float distToTarget = (m_TargetPos - transform.position).magnitude;
                if (distToTarget <= 0.001f)
                {
                    m_IsMoving = false;
                    OnUpdatePosition?.Invoke(m_AnchorPos, transform.position, this);
                    return;
                }

                //Обновление позиции ячейки
                float distTravelled = (transform.position - m_AnchorPos).magnitude;
                if (distTravelled >= m_CurDistToUpdate)
                {
                    OnUpdatePosition?.Invoke(m_AnchorPos, transform.position, this);

                    m_CurDistToUpdate = DistanceToUpdate;
                    m_AnchorPos = transform.position;
                }

                transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime);
            }
        }
    }
}
