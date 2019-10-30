using Paint.Characters;
using UnityEngine;

namespace Paint.General
{
    public class AssetsLibrary : MonoBehaviour
    {
        public PrefabsLibrary Library_Prefabs;

        [System.Serializable]
        public class PrefabsLibrary
        {
            public Character PlayerCharacterPrefab;
        }
    }
}
