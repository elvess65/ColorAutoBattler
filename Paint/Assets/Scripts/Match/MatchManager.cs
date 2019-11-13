using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paint.Match
{
    public class MatchManager 
    {
        private MatchPlayer[] m_MatchPlayers;

        private int m_CurSelectingPlayerIndex = 0;

        private const int m_CONTROLLED_CHARACTERS_AMOUNT = 1;

        public void CreateMatch(int[] ids)
        {
            Debug.Log("Create match. Total players: " + ids.Length);

            m_MatchPlayers = new MatchPlayer[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Debug.Log("Create match for player with id: " + ids[i]);
                MatchPlayer player = new MatchPlayer(ids[i], m_CONTROLLED_CHARACTERS_AMOUNT);
                m_MatchPlayers[i] = player;
            }
        }

        public void StartSelectingCharacters()
        {
            m_CurSelectingPlayerIndex = -1;

            AllowSelectNextPlayer();
        }

        public void SelectionFinished(int characterType, int attackType, int resistType)
        {
            m_MatchPlayers[m_CurSelectingPlayerIndex].AddCharacter(Random.Range(1, 5));

            if (AllPlayersFinishedSelection())
                StartMatch();
            else 
                AllowSelectNextPlayer();
        }

        
        void StartMatch()
        {
            Debug.Log("Start match");
        }

        void AllowSelectNextPlayer()
        {
            m_CurSelectingPlayerIndex++;
            if (m_CurSelectingPlayerIndex >= m_MatchPlayers.Length)
                m_CurSelectingPlayerIndex = 0;

            Debug.Log("Player with id " + m_CurSelectingPlayerIndex + " is selecting");
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
            public int ID { get; private set; }
            public List<int> ControlledCharacters { get; private set; }

            private int m_ControlledCharactersAmount;


            public bool HasFullCharacters => ControlledCharacters.Count == m_ControlledCharactersAmount;


            public MatchPlayer(int id, int controlledCharactersAmount)
            {
                ID = id;
                m_ControlledCharactersAmount = controlledCharactersAmount;

                ControlledCharacters = new List<int>();
            }

            public void AddCharacter(int param) => ControlledCharacters.Add(param);
        }
    }
}
