using Paint.Character.Weapon;
using Paint.Characters;
using Paint.General;
using System.Collections.Generic;
using UnityEngine;

namespace Paint.Match
{
    public class MatchManager 
    {
        public System.Action OnSelectionIterationFinished;
        public System.Action OnSelectingCharactersStarted;
        public System.Action<int> OnPlayerStartSelectCharacters;
        public System.Action OnStartMatch;
        public System.Action<int> OnRoundFinished;

        private int m_CurSelectingPlayerIndex = 0;
        private int m_IterationIndex = 0;

        private const int m_CONTROLLED_CHARACTERS_AMOUNT = 2;

        public MatchPlayer[] MatchPlayers { get; private set; }


        public void CreateMatch(int[] ids)
        {
            Debug.Log("Create match. Total players: " + ids.Length);

            MatchPlayers = new MatchPlayer[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                MatchPlayer player = new MatchPlayer(ids[i], m_CONTROLLED_CHARACTERS_AMOUNT);
                MatchPlayers[i] = player;

                if (i != 0)
                    player.TeamColor = new Color32(248, 155, 255, 255);
                else
                    player.TeamColor = new Color32(253, 255, 155, 255);
            }
        }

        public void StartSelectingCharacters()
        {
            OnSelectingCharactersStarted?.Invoke();

            m_CurSelectingPlayerIndex = -1;

            AllowSelectNextPlayer();
        }

        public void SelectionFinished(CharacterTypes characterType, WeaponTypes attackType, WeaponTypes resistType)
        {
            MatchPlayers[m_CurSelectingPlayerIndex].AddCharacter(characterType, attackType, resistType);

            if (AllPlayersFinishedSelection())
                StartMatch();
            else 
                AllowSelectNextPlayer();
        }

        public void CreateCharactersByIteration()
        {
            for (int i = 0; i < MatchPlayers.Length; i++)
            {
                MatchPlayer.CharactersData data = MatchPlayers[i].ControlledCharactersData[m_IterationIndex];

                Vector3 pos = Vector3.zero;
                if (i == 0)
                    pos = GameManager.Instance.Player1SpawnPoints[m_IterationIndex].position;
                else
                    pos = GameManager.Instance.Player2SpawnPoints[m_IterationIndex].position;

                int hpAmount = 20;
                switch (data.CharacterType)
                {
                    case CharacterTypes.Melee:
                        hpAmount = 30;
                        break;
                }

                UnitCharacter character = GameManager.Instantiate(GetUnitPrefabByType(data.CharacterType), pos, Quaternion.identity) as UnitCharacter;
                character.Init(MatchPlayers[i].ID, data.ID, hpAmount, MatchPlayers[i].TeamColor, data.CharacterType, data.AttackType, data.ResistType);
                character.gameObject.name = "Unit. ID: " + data.ID + ". PlayerID: " + MatchPlayers[i].ID;
                character.OnDestroy += CharacterDestroyedHandler;

                MatchPlayers[i].ControlledCharacters.Add(character);
            }

            m_IterationIndex++;
        }


        void CharacterDestroyedHandler()
        {
            int amountOfLostTeams = 0;
            int winnerPlayerID = -1;
            for (int i = 0; i < MatchPlayers.Length; i++)
            {
                if (MatchPlayers[i].AllCharactersDestroyed)
                    amountOfLostTeams++;
                else
                    winnerPlayerID = MatchPlayers[i].ID;
            }

            if (amountOfLostTeams == MatchPlayers.Length)
                OnRoundFinished?.Invoke(winnerPlayerID);
            else if (amountOfLostTeams == MatchPlayers.Length - 1)
                OnRoundFinished?.Invoke(winnerPlayerID);
        }

        void StartMatch()
        {
            OnStartMatch?.Invoke();
        }

        void AllowSelectNextPlayer()
        {
            m_CurSelectingPlayerIndex++;
            if (m_CurSelectingPlayerIndex >= MatchPlayers.Length)
            {
                m_CurSelectingPlayerIndex = 0;

                OnSelectionIterationFinished?.Invoke();
            }

            OnPlayerStartSelectCharacters?.Invoke(MatchPlayers[m_CurSelectingPlayerIndex].ID);
        }

        bool AllPlayersFinishedSelection()
        {
            int playersFinishedSelectionAmount = 0;
            for (int i = 0; i < MatchPlayers.Length; i++)
            {
                if (MatchPlayers[i].HasFullCharacters)
                    playersFinishedSelectionAmount++;
            }

            return playersFinishedSelectionAmount == MatchPlayers.Length;
        }

        UnitCharacter GetUnitPrefabByType(CharacterTypes type)
        {
            UnitCharacter result = null;

            switch (type)
            {
                case CharacterTypes.Melee:
                    result = GameManager.Instance.AssetsLibrary.Library_Prefabs.UnitCharacterMelee_Prefab as UnitCharacter;
                    break;
                case CharacterTypes.Range:
                    result = GameManager.Instance.AssetsLibrary.Library_Prefabs.UnitCharacterRange_Prefab as UnitCharacter;
                    break;
                case CharacterTypes.Fly:
                    result = GameManager.Instance.AssetsLibrary.Library_Prefabs.UnitCharacterFly_Prefab as UnitCharacter;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }


        public class MatchPlayer
        {
            public static int GLOBAL_ID = 100;

            public Color TeamColor;
            public int ID { get; private set; }
            public List<CharactersData> ControlledCharactersData { get; private set; }
            public List<UnitCharacter> ControlledCharacters { get; set; }
            public bool HasFullCharacters => ControlledCharactersData.Count == m_ControlledCharactersAmount;
            public bool AllCharactersDestroyed
            {
                get
                {
                    int destroyedCharacters = 0;
                    foreach(UnitCharacter u in ControlledCharacters)
                    {
                        if (u.IsDestroyed)
                            destroyedCharacters++;
                    }

                    return destroyedCharacters == ControlledCharacters.Count;
                }
            }


            private int m_ControlledCharactersAmount;


            public MatchPlayer(int id, int controlledCharactersAmount)
            {
                ID = id;
                m_ControlledCharactersAmount = controlledCharactersAmount;

                ControlledCharactersData = new List<CharactersData>();
                ControlledCharacters = new List<UnitCharacter>();
            }

            public void AddCharacter(CharacterTypes characterType, WeaponTypes attackType, WeaponTypes resistType)
            {
                ControlledCharactersData.Add(new CharactersData(characterType, attackType, resistType, GLOBAL_ID++));
            }


            public struct CharactersData
            {
                public CharacterTypes CharacterType;
                public WeaponTypes AttackType;
                public WeaponTypes ResistType;
                public int ID;

                public CharactersData(CharacterTypes characterType, WeaponTypes attackType, WeaponTypes resistType, int id)
                {
                    CharacterType = characterType;
                    AttackType = attackType;
                    ResistType = resistType;
                    ID = id;
                }
            }
        }
    }
}
