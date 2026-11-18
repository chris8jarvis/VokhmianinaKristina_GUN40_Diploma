using System;
using UnityEngine;
using Zenject;

namespace Project.World
{
    public class Coin : MonoBehaviour, IPoolable<IMemoryPool>, IPoolableObject
    {
        public static event Action OnCoinCollected;

        private IMemoryPool _pool;
        private bool _isCollected = false;

        //Вызывается, когда монетку достали из пула
        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _isCollected = false;
        }

        //Вызывается, когда монетку вернули в пул
        public void OnDespawned()
        {
            _isCollected = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isCollected)
            {
                Collect();
            }
        }

        public void Collect()
        {
            if (_isCollected) return;
            OnCoinCollected?.Invoke();
            
            if (_pool != null)
            {
                _pool.Despawn(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Метод для возврата в пул из Magnet
        public void Despawn()
        {
            if (_pool == null) return;
            _pool.Despawn(this);
        }

        public class Pool : MemoryPool<Coin> { }
    }
}