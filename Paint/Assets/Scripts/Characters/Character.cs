using Paint.Characters.Movement;
using UnityEngine;

namespace Paint.Characters
{
    /// <summary>
    /// Общий класс для всех персонажей
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        //iMoveBehavour
        protected iMovement m_MoveController;

        //iShootBehaviour
        //iHealthBehaviour


        public virtual void Init()
        {
        }

        public abstract void Move(Vector2 mDir);


        protected virtual void Update()
        {            
        }
    }
}
