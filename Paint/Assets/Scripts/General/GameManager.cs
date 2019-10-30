using UnityEngine;

namespace Paint.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public AssetsLibrary AssetsLibrary;

        public Characters.Character PlayerCharacter;

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else 
                Instance = this;
        }

        void Start()
        {
            CreatePlayer();
        }

        void CreatePlayer()
        {
            PlayerCharacter = Instantiate(AssetsLibrary.Library_Prefabs.PlayerCharacterPrefab, Vector3.zero, Quaternion.identity);
            PlayerCharacter.Init();
        }
    }
}
