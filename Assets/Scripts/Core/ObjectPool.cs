using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool<T> where T: MonoBehaviour
	{
		private Dictionary<string, Pool> _pools = new();

        public void InitPool(T prefab, Transform container)
        {
            if (prefab != null && !_pools.ContainsKey(prefab.name))
            {
                _pools[prefab.name] = new Pool(prefab, container);
            }
        }

        public T Spawn(T prefab, Transform container)
        {
            InitPool(prefab, container);
			return _pools[prefab.gameObject.name].Spawn(container);
        }

        public void Despawn(T obj)
        {
			if (_pools.ContainsKey(obj.name))
            {
                _pools[obj.name].Despawn(obj);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        class Pool
        {
            private List<T> _inactive = new ();
            private T _prefab;
            private Transform _container;

            public Pool(T prefab, Transform container)
            {
                _prefab = prefab;
                _container = container;
            }

            public T Spawn(Transform container)
            {
                T unit;
                if (_inactive.Count == 0)
                {
                    unit = Object.Instantiate(_prefab, container);
                    unit.name = _prefab.name;
                }
                else
                {
                    unit = _inactive[_inactive.Count - 1];
                    _inactive.RemoveAt(_inactive.Count - 1);
                    unit.gameObject.SetActive(true);
                    unit.transform.SetParent(container);
                }
                return unit;
            }

            public void Despawn(T unit)
            {
                unit.transform.SetParent(_container);
                _inactive.Add(unit);
            }
        }
    }
}