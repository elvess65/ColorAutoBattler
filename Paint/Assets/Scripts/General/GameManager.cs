﻿using Paint.CameraSystem;
using Paint.Character.Weapon;
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

        public Transform PlayerSpawnPoint;
        public Transform[] ManekenSpawnPoints;
        public Transform[] TurrentSpawnPoints;

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
            (WeaponTypes type, int health)[] healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Red, 1),
                (WeaponTypes.Green, 2),
                (WeaponTypes.Blue, 3),
            };

            PlayerCharacter = Instantiate(AssetsLibrary.Library_Prefabs.PlayerCharacterPrefab, PlayerSpawnPoint.position, Quaternion.identity);
            PlayerCharacter.OnDestroy += () => InputManager.InputIsEnabled = false; 
            PlayerCharacter.Init(healthData);

            InputManager.OnMove += PlayerCharacter.SetMoveDiretion;
            InputManager.OnShoot += PlayerCharacter.Shoot;
            InputManager.OnWeaponTypeChange += PlayerCharacter.SelectWeaponType;
            InputManager.OnShieldActivate += PlayerCharacter.ShieldActivate;
        }

        void CreateManeken()
        {
            int i = 0;

            (WeaponTypes type, int health)[] healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Red, 3),
            };

            Characters.Character maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, ManekenSpawnPoints[i].position, Quaternion.identity);
            maneken.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Green, 3),
            };

            maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, ManekenSpawnPoints[i].position, Quaternion.identity);
            maneken.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Blue, 3),
            };

            maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, ManekenSpawnPoints[i].position, Quaternion.identity);
            maneken.Init(healthData);
            i++;
        }

        void CreateTurrent()
        {
            int i = 0;

            (WeaponTypes type, int health)[] healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Red, 1),
                (WeaponTypes.Green, 2),
            };

            Characters.Character turrent = Instantiate(AssetsLibrary.Library_Prefabs.TurrentCharacterPrefab, TurrentSpawnPoints[i].position, Quaternion.identity);
            turrent.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Red, 1),
                (WeaponTypes.Blue, 3),
            };

            turrent = Instantiate(AssetsLibrary.Library_Prefabs.TurrentCharacterPrefab, TurrentSpawnPoints[i].position, Quaternion.identity);
            turrent.Init(healthData);
            i++;
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
