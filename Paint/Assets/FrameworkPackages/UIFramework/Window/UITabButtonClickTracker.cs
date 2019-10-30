using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Отслеживает нажатие на вкладку и включает/выключает указанные объекты
    /// </summary>
    public class UITabButtonClickTracker : MonoBehaviour
    {
        public GameObject[] ObjectsToBeEnabled;
        public GameObject[] ObjectsToBeDisabled;

        public void ButtonPressHandler()
        {
            for (int i = 0; i < ObjectsToBeEnabled.Length; i++)
            {
                if (!ObjectsToBeEnabled[i].activeSelf)
                    ObjectsToBeEnabled[i].SetActive(true);
            }

            for (int i = 0; i < ObjectsToBeDisabled.Length; i++)
            {
                if (ObjectsToBeDisabled[i].activeSelf)
                    ObjectsToBeDisabled[i].SetActive(false);
            }
        }
    }
}
