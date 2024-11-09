using System.Collections.Generic;
using UnityEngine;

namespace FrameworkPackage.Pool
{
    /// <summary>
    /// Объект, кодержащий пулл определенных типов объектов
    /// </summary>
    public class ObjectPool
    {
        private Stack<PoolObject> m_Pool;
        private readonly Transform m_PoolParent;

        public ObjectPool(Transform poolParent)
        {
            m_Pool = new Stack<PoolObject>();
            m_PoolParent = poolParent;
        }


        /// <summary>
        /// Получить объект из пулла или создать недостающий
        /// </summary>
        public PoolObject GetObject(PoolObject source, Vector3 pos, Quaternion rotation)
        {
            PoolObject result = GetObject(source);

            //Задать объекту расположение
            result.transform.position = pos;
            result.transform.rotation = rotation;

            return result;
        }

        /// <summary>
        /// Получить объект из пулла или создать недостающий
        /// </summary>
        public PoolObject GetObject(PoolObject source, Transform parent)
        {
            PoolObject result = GetObject(source);

            //Задать объекту перент
            result.transform.SetParent(parent);

            return result;
        }

        /// <summary>
        /// Получить объект из пулла или создать недостающий
        /// </summary>
        public PoolObject GetObject(PoolObject source)
        {
            PoolObject result;

            //Если в пулле нет объектов - создать объект, если есть - достать из пулла
            if (m_Pool.Count == 0)
                result = CreatePoolObject(source);
            else
                result = m_Pool.Pop();

            //Активировать объект
            result.EnablePoolObject();

            return result;
        }


        /// <summary>
        /// Инициализация пулла объектами
        /// </summary>
        /// <param name="source">Объект</param>
        /// <param name="count">Количество объектов</param>
        public void InitializeObjects(PoolObject source, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PoolObject obj = CreatePoolObject(source);
                obj.DisablePoolObject();
            }
        }

        /// <summary>
        /// Удаление всех объектов из пулла
        /// </summary>
        public void ClearPool()
        {
            foreach (PoolObject ob in m_Pool)
                Object.Destroy(ob.gameObject);
                
            m_Pool.Clear();
        }

       
        /// <summary>
        /// Вернуть объект в пулл
        /// </summary>
        void ReturnObjectToPool(PoolObject ob) => m_Pool.Push(ob);

        /// <summary>
        /// Создать объект и подписатся на событие его отключения
        /// </summary>
        PoolObject CreatePoolObject(PoolObject source)
        {
            PoolObject result = Object.Instantiate(source);
            result.OnDisable += ReturnObjectToPool;
            result.SetPoolParent(m_PoolParent);

            return result;
        }
    }
}
