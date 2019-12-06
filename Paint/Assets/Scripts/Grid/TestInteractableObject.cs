using UnityEngine;

namespace Paint.Grid.Interaction
{
    public class TestInteractableObject : MonoBehaviour, iInteractableObject
    {
        private bool m_IsSelected = false;
        private bool m_IsMoving = false;
        private Vector3 m_MovePos;
        private Vector3 m_StartMovePos;

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

        public void SetMovePosition(Vector3 movePos)
        {
            m_StartMovePos = transform.position;
            m_MovePos = movePos;
            m_IsMoving = true;
        }


        void Update()
        {
            if (m_IsMoving)
            {
                float sqrDistTravelled = (transform.position - m_StartMovePos).sqrMagnitude;
                Debug.Log(sqrDistTravelled);

                if (sqrDistTravelled / 2 >= 0.5f)
                    Debug.Break();

                transform.position = Vector3.MoveTowards(transform.position, m_MovePos, Time.deltaTime * 2);
            }
        }
    }
}
