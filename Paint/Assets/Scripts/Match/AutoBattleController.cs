using Paint.Characters;
using UnityEngine;
using static Paint.Match.MatchManager;

namespace Paint.Match
{
    public class AutoBattleController : MonoBehaviour
    {
        private bool m_BattleIsStarted = false;
        private MatchPlayer[] m_Players;


        public void Init(MatchPlayer[] matchPlayers) => m_Players = matchPlayers;

        public void StartBattle() => m_BattleIsStarted = true;

        public void StopBattle() => m_BattleIsStarted = false;


        void Update()
        {
            if (m_BattleIsStarted)
            {
                for (int i = 0; i < m_Players.Length; i++)
                {
                    for (int j = 0; j < m_Players[i].ControlledCharacters.Count; j++)
                    {
                        //Текущий юнит
                        UnitCharacter currentCharacter = m_Players[i].ControlledCharacters[j];

                        //Текущий юнит не уничтожен
                        if (!currentCharacter.IsDestroyed)
                        {
                            //Нет цели
                            if (!currentCharacter.HasTarget)
                                currentCharacter.SetTarget(FindClosestCharacter(m_Players[i].ID, currentCharacter));
                            else
                                currentCharacter.AttackTarget();
                        }
                    }
                }
            }
        }

        UnitCharacter FindClosestCharacter(int excludedPlayerID, UnitCharacter currentCharater)
        {
            float distToEnemy = float.MaxValue;
            UnitCharacter closestCharacter = null;

            for (int i = 0; i < m_Players.Length; i++)
            {
                //Исключить определенного игрока
                if (m_Players[i].ID != excludedPlayerID)
                {
                    //Пройтись по всем игрокам остальных игроков
                    for (int j = 0; j < m_Players[i].ControlledCharacters.Count; j++)
                    {
                        //Юнит игрока
                        UnitCharacter character = m_Players[i].ControlledCharacters[j];

                        //Если юнит не уничтожен
                        if (!character.IsDestroyed)
                        {
                            //Вектор от текущего юнита, к юниту другого игрока
                            Vector3 dirToEnemy = character.transform.position - character.transform.position;

                            //Если этот юнит ближе чем все предыдущие
                            if (dirToEnemy.sqrMagnitude < distToEnemy)
                            {
                                distToEnemy = dirToEnemy.sqrMagnitude;
                                closestCharacter = character;
                            }
                        }
                    }
                }
            }

            return closestCharacter;
        }
    }

}
