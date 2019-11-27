using Paint.CameraSystem;
using Paint.Character.Weapon;
using Paint.InputSystem;
using Paint.Match;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Paint.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public AssetsLibrary AssetsLibrary;
        public CameraController CameraController;
        public InputManager InputManager;
        public AutoBattleController AutoBattleController;
        public UIWindow_CharacterSelection UIWindow_Selection;

        public Transform PlayerSpawnPoint;
        public Transform[] ManekenSpawnPoints;
        public Transform[] TurrentSpawnPoints;
        public Transform[] Player1SpawnPoints;
        public Transform[] Player2SpawnPoints;
        public Transform RoundResultUI;
        public Text Text_RoundResult;
        public Button Button_Replay;

        public MatchManager MatchManager { get; private set; }
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
            //CreatePlayer();
            //CreateManeken();
            //CreateTurrent();
            CreateMatch();
            //StartLoop();
        }


        void Initialize()
        {
            InputManager.Init();
            CameraController.Init();

            UIWindow_Selection.gameObject.SetActive(false);
            RoundResultUI.gameObject.SetActive(false);
            Button_Replay.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

        void CreatePlayer()
        {
            (WeaponTypes type, int health)[] healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Frost, 1),
                (WeaponTypes.Sun, 2),
                (WeaponTypes.Earth, 3),
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
                (WeaponTypes.Frost, 3),
            };

            Characters.Character maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, ManekenSpawnPoints[i].position, Quaternion.identity);
            maneken.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Sun, 3),
            };

            maneken = Instantiate(AssetsLibrary.Library_Prefabs.ManekenCharacterPrefab, ManekenSpawnPoints[i].position, Quaternion.identity);
            maneken.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Earth, 3),
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
                (WeaponTypes.Frost, 1),
                (WeaponTypes.Sun, 2),
            };

            Characters.Character turrent = Instantiate(AssetsLibrary.Library_Prefabs.TurrentCharacterPrefab, TurrentSpawnPoints[i].position, Quaternion.identity);
            turrent.Init(healthData);
            i++;


            healthData = new (WeaponTypes type, int health)[]
            {
                (WeaponTypes.Frost, 1),
                (WeaponTypes.Earth, 3),
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

        void CreateMatch()
        {
            int[] playerIDs = new int[] { 1, 2 };

            MatchManager = new MatchManager();

            MatchManager.OnSelectingCharactersStarted += () => UIWindow_Selection.gameObject.SetActive(true);
            MatchManager.OnSelectionIterationFinished += () => MatchManager.CreateCharactersByIteration();
            MatchManager.OnPlayerStartSelectCharacters += UIWindow_Selection.SetSelectingPlayer;
            MatchManager.OnStartMatch += StartMatch;
            MatchManager.OnRoundFinished += FinishRound;

            UIWindow_Selection.OnSelectionFinished += MatchManager.SelectionFinished;

            MatchManager.CreateMatch(playerIDs);
            MatchManager.StartSelectingCharacters();
        }

        void StartMatch()
        {
            UIWindow_Selection.gameObject.SetActive(false);
            MatchManager.CreateCharactersByIteration();

            AutoBattleController.Init(MatchManager.MatchPlayers);
            AutoBattleController.StartBattle();

            IsActive = true;
        }

        void FinishRound(int winnerPlayerID)
        {
            Text_RoundResult.text = winnerPlayerID >= 0 ? string.Format("Player {0} wins", winnerPlayerID) : "Draw";
            RoundResultUI.gameObject.SetActive(true);
        }

        void HandleCameraFinishedAligning()
        {
            CameraController.OnCameraFinishedAligning -= HandleCameraFinishedAligning;

            InputManager.InputIsEnabled = true;
        }
    }
}
