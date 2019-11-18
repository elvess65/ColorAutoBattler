using Paint.Character.Weapon;
using Paint.Characters;
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

        private MatchPlayer[] m_MatchPlayers;
        private int m_CurSelectingPlayerIndex = 0;
        private int m_IterationIndex = 0;

        private const int m_CONTROLLED_CHARACTERS_AMOUNT = 2;


        public void CreateMatch(int[] ids)
        {
            Debug.Log("Create match. Total players: " + ids.Length);

            m_MatchPlayers = new MatchPlayer[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                MatchPlayer player = new MatchPlayer(ids[i], m_CONTROLLED_CHARACTERS_AMOUNT);
                m_MatchPlayers[i] = player;
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
            m_MatchPlayers[m_CurSelectingPlayerIndex].AddCharacter(characterType, attackType, resistType);

            if (AllPlayersFinishedSelection())
                StartMatch();
            else 
                AllowSelectNextPlayer();
        }

        public void CreateCharactersByIteration()
        {
            for (int i = 0; i < m_MatchPlayers.Length; i++)
            {
                MatchPlayer.CharactersData data = m_MatchPlayers[i].ControlledCharactersData[m_IterationIndex];
                Debug.Log(data.CharacterType + " " + data.AttackType + " " + data.ResistType);
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
            if (m_CurSelectingPlayerIndex >= m_MatchPlayers.Length)
            {
                m_CurSelectingPlayerIndex = 0;

                OnSelectionIterationFinished?.Invoke();
            }

            OnPlayerStartSelectCharacters?.Invoke(m_MatchPlayers[m_CurSelectingPlayerIndex].ID);
        }

        bool AllPlayersFinishedSelection()
        {
            int playersFinishedSelectionAmount = 0;
            for (int i = 0; i < m_MatchPlayers.Length; i++)
            {
                if (m_MatchPlayers[i].HasFullCharacters)
                    playersFinishedSelectionAmount++;
            }

            return playersFinishedSelectionAmount == m_MatchPlayers.Length;
        }



        public class MatchPlayer
        {
            public static int GLOBAL_ID = 100;

            public int ID { get; private set; }
            public List<CharactersData> ControlledCharactersData { get; private set; }

            private int m_ControlledCharactersAmount;

            public bool HasFullCharacters => ControlledCharactersData.Count == m_ControlledCharactersAmount;


            public MatchPlayer(int id, int controlledCharactersAmount)
            {
                ID = id;
                m_ControlledCharactersAmount = controlledCharactersAmount;

                ControlledCharactersData = new List<CharactersData>();
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
