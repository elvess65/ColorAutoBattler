using System.Collections;
using UnityEngine;

namespace FrameworkPackage.Pool
{
    /// <summary>
    /// Объект, который можно поместить в пулл
    /// </summary>
    public class PoolObject : MonoBehaviour
    {
        public System.Action<PoolObject> OnDisable;

        protected Transform m_PoolParent;
        private float m_DisableTime = -1;

        public bool IsEnabled { get; private set; } = false;


        /// <summary>
        /// Включить объект
        /// </summary>
        public virtual void EnablePoolObject()
        {
            gameObject.SetActive(true);
            transform.SetParent(null);
            IsEnabled = true;
        }

        /// <summary>
        /// Выключить объект
        /// </summary>
        public void DisablePoolObject()
        {
#if USE_POOL
            DisableObject();
#else
            Destroy(gameObject);
#endif
        }

        /// <summary>
        /// Выключить объект с задержкой
        /// </summary>
        /// <param name="timeToDisable">Задержка перед выключением</param>
        public void DisablePoolObject(float timeToDisable)
        {
#if USE_POOL
            if (timeToDisable > 0)
                m_DisableTime = Time.time + timeToDisable;
            else
                DisableObject();
#else
            Destroy(gameObject, timeToDisable);
#endif
        }

        /// <summary>
        /// Обнулить время, через которое объект будет автоматически выключен
        /// </summary>
        public void ResetDisableTime() => m_DisableTime = 0;

        /// <summary>
        /// Задать объект для содержания выключенных объектов
        /// </summary>
        public void SetPoolParent(Transform poolParent) => m_PoolParent = poolParent;

        /// <summary>
        /// Обнулить локальные координаты объекта
        /// </summary>
        public void ResetLocalTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }


        protected virtual void DisableObject()
        {
            transform.SetParent(m_PoolParent);
            gameObject.SetActive(false);
            IsEnabled = false;

            OnDisable?.Invoke(this);
        }

        protected virtual void Update()
        {
            if (m_DisableTime > 0)
            {
                if (Time.time >= m_DisableTime)
                {
                    m_DisableTime = -1;
                    DisableObject();
                }
            }
        }
    }
}
