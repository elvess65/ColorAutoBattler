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

        private int m_CurSelectingPlayerIndex = 0;
        private int m_IterationIndex = 0;

        private const int m_CONTROLLED_CHARACTERS_AMOUNT = 1;

        public MatchPlayer[] MatchPlayers { get; private set; }


        public void CreateMatch(int[] ids)
        {
            Debug.Log("Create match. Total players: " + ids.Length);

            MatchPlayers = new MatchPlayer[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                MatchPlayer player = new MatchPlayer(ids[i], m_CONTROLLED_CHARACTERS_AMOUNT);
                MatchPlayers[i] = player;
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

                UnitCharacter character = GameManager.Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.UnitCharacterPrefab, pos, Quaternion.identity) as UnitCharacter;
                character.Init(MatchPlayers[i].ID, 20, data.AttackType, data.ResistType);

                MatchPlayers[i].ControlledCharacters.Add(character);
            }

            m_IterationIndex++;
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



        public class MatchPlayer
        {
            public static int GLOBAL_ID = 100;

            public int ID { get; private set; }
            public List<CharactersData> ControlledCharactersData { get; private set; }
            public List<UnitCharacter> ControlledCharacters { get; set; }
            public bool HasFullCharacters => ControlledCharactersData.Count == m_ControlledCharactersAmount;

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
