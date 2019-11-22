using Paint.Characters;
using UnityEngine;
using static Paint.Match.MatchManager;

namespace Paint.Match
{
    public class AutoBattleController : MonoBehaviour
    {
        private bool m_BattleIsStarted = false;
        private MatchPlayer[] m_MatchPlayers;


        public void Init(MatchPlayer[] matchPlayers) => m_MatchPlayers = matchPlayers;

        public void StartBattle() => m_BattleIsStarted = true;

        public void StopBattle() => m_BattleIsStarted = false;


        void Update()
        {
            if (m_BattleIsStarted)
            {
                for (int i = 0; i < m_MatchPlayers.Length; i++)
                {
                    for (int j = 0; j < m_MatchPlayers[i].ControlledCharacters.Count; j++)
                    {
                        UnitCharacter currentCharacter = m_MatchPlayers[i].ControlledCharacters[j];

                        for (int i1 = 0; i1 < m_MatchPlayers.Length; i1++)
                        {
                            for (int j1 = 0; j1 < m_MatchPlayers[i1].ControlledCharacters.Count; j1++)
                            {
                                if (m_MatchPlayers[i].ID != m_MatchPlayers[i1].ID)
                                {
                                    UnitCharacter enemyCharacter = m_MatchPlayers[i1].ControlledCharacters[j1];

                                    Vector3 dirToEnemy = enemyCharacter.transform.position - currentCharacter.transform.position;
                                    currentCharacter.Shoot(new Vector2(dirToEnemy.x, dirToEnemy.z).normalized);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
