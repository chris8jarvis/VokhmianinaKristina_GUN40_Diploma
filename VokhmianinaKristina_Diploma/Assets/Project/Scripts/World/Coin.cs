using System;
using UnityEngine;

namespace Project.World
{
    public class Coin : MonoBehaviour
    {
        public static event Action OnCoinCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnCoinCollected?.Invoke();
                
                //Уничтожаем монетку (позже сделать через пул)
                Destroy(gameObject); 
            }
        }
    }
}