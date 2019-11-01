using Paint.CameraSystem;
using Paint.InputSystem;
using UnityEngine;

namespace Paint.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public AssetsLibrary AssetsLibrary;
        public CameraController CameraController;
        public InputManager InputManager;

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
            CreateManeken();
            CreateTurrent();
            StartLoop();
        }


        void Initialize()
        {
            InputManager.Init();
            CameraController.Init();
        }

        void CreatePlayer()
        {
            PlayerCharacter = Instantiate(AssetsLibrary.Library_Prefabs.PlayerCharacterPrefab, Vector3.zero, Quaternion.identity);
            PlayerCharacter.OnDestroy += () => InputManager.InputIsEnabled = false; 
            PlayerCharacter.Init();

            InputManager.OnMove += (Vector2 dir) => PlayerCharacter.SetMoveDiretion(dir);
            InputManager.OnShoot += (Vector2 dir) => PlayerCharacter.Shoot(dir);
        }

        void CreateManeken()
        {
            for (int i = 0; i < 3; i++)
            {
                Characters.Character maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, new Vector3(-3 + i * 3, 0, 4), Quaternion.identity);
                maneken.Init();
            }
        }

        void CreateTurrent()
        {
            for (int i = 0; i < 2; i++)
            {
                Characters.Character turrent = Instantiate(AssetsLibrary.Library_Prefabs.TurrentCharacterPrefab, new Vector3(-2 + i * 4, 0, -4), Quaternion.identity);
                turrent.Init();
            }
        }

        void StartLoop()
        {
            CameraController.OnCameraFinishedAligning += HandleCameraFinishedAligning;
            CameraController.SetTarget(PlayerCharacter.transform);
            
            IsActive = true;
        }


        void HandleCameraFinishedAligning()
        {
            CameraController.OnCameraFinishedAligning -= HandleCameraFinishedAligning;

            InputManager.InputIsEnabled = true;
        }
    }
}
