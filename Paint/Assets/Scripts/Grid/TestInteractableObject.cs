using UnityEngine;

namespace Paint.Grid.Interaction
{
    public class TestInteractableObject : MonoBehaviour, iInteractableObject
    {
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
    }
}
