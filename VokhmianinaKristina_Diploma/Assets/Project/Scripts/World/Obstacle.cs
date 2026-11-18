using UnityEngine;
using Zenject;

namespace Project.World
{
    public class Obstacle : MonoBehaviour, IPoolable<IMemoryPool>, IPoolableObject
    {
        private IMemoryPool _pool;

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void OnDespawned()
        {
        }

        //Вызываем, когда сегмент уходит назад
        public void Despawn()
        {
            if (_pool == null) return;
            _pool.Despawn(this);
        }
    }
}
