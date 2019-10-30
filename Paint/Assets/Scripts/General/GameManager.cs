using Paint.CameraSystem;
using UnityEngine;

namespace Paint.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public AssetsLibrary AssetsLibrary;
        public CameraController CameraController;

        public Characters.Character PlayerCharacter { get; private set; }
        public bool IsActive { get; private set; }

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else 
                Instance = this;
        }

        void Start()
        {
            Initialize();
            CreatePlayer();

            IsActive = true;
        }

        void Initialize()
        {
            CameraController.Init();
        }

        void CreatePlayer()
        {
            PlayerCharacter = Instantiate(AssetsLibrary.Library_Prefabs.PlayerCharacterPrefab, Vector3.zero, Quaternion.identity);
            PlayerCharacter.Init();

            CameraController.SetTarget(PlayerCharacter.transform);
        }
    }
}
