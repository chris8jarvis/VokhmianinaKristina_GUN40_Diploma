using System;
using UnityEngine;

namespace Project.World
{
    public class Coin : MonoBehaviour
    {
        public static event Action OnCoinCollected;
        private bool _isCollected = false; //Флаг, чтобы монетку нельзя было собрать дважды

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isCollected)
            {
                Collect();
            }
        }
        //Публичный метод, чтобы Magnet мог его вызвать
        public void Collect()
        {
            if (_isCollected) return; //Защита от двойного сбора монет
            _isCollected = true;
            
            OnCoinCollected?.Invoke();
            Destroy(gameObject); 
        }
    }
}