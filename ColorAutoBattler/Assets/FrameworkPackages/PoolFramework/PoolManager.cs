using System.Collections.Generic;
using UnityEngine;

namespace FrameworkPackage.Pool
{
    /// <summary>
    /// Класс для работы с пуллом
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        private Transform m_PoolParent;
        private Dictionary<int, ObjectPool> m_Pools = new Dictionary<int, ObjectPool>();

        private Transform PoolParent
        {
            get
            {
                if (m_PoolParent == null)
                {
                    m_PoolParent = new GameObject { name = "DisabledPoolObjects" }.transform;
                    m_PoolParent.transform.position = new Vector3(1000, 1000, 1000);
                }

                return m_PoolParent;
            }
        }


        /// <summary>
        /// Получить объект из пулла
        /// </summary>
        public PoolObject GetObject(PoolObject source, Vector3 pos, Quaternion rotation) => GetPool(source.GetHashCode()).GetObject(source, pos, rotation);

        /// <summary>
        /// Получить объект из пулла
        /// </summary>
        public PoolObject GetObject(PoolObject source, Transform parent) => GetPool(source.GetHashCode()).GetObject(source, parent);

        /// <summary>
        /// Получить объект из пулла
        /// </summary>
        public PoolObject GetObject(PoolObject source) => GetPool(source.GetHashCode()).GetObject(source);


        /// <summary>
        /// Очистить список пуллов
        /// </summary>
        public void ClearPool()
        {
            foreach (ObjectPool pool in m_Pools.Values)
                pool.ClearPool();

            m_Pools.Clear();
        }

        /// <summary>
        /// Инициализация объектов
        /// </summary>
        public virtual void Init()
        { }

        /// <summary>
        /// Заполнение пулла указанным количеством объектов
        /// </summary>
        protected void InitializePoolWithObjects(PoolObject source, int count) => GetPool(source.GetHashCode()).InitializeObjects(source, count);

        /// <summary>
        /// Получить пулл для создаваемого объекта
        /// </summary>
        /// <returns>Пулл объектов</returns>
        /// <param name="objectTypeID">ИД по которому идентифицируется тип объекта</param>
        ObjectPool GetPool(int objectTypeID)
        {
            //Если пулл уже существует - вернуть пулл
            if (m_Pools.ContainsKey(objectTypeID))
                return m_Pools[objectTypeID];

            //Если пулл не создан - создать новый
            ObjectPool pool = new ObjectPool(PoolParent);
            m_Pools.Add(objectTypeID, pool);
            return pool;
        }

        /*
        #if USE_POOL
            PoolManager.Instance.GetObject();
        #else
        #endif
        */
    }
}
