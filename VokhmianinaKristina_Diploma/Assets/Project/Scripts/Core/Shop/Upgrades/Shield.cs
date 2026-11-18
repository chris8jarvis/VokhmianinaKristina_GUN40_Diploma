using UnityEngine;
using System.Collections;

namespace Project.Core.Upgrades
{
    public class Shield : MonoBehaviour
    {
        public bool IsActive { get; private set; } = false;
        private int _shieldLevel = 0;

        private void Start()
        {
            //Читаем уровень из сохранений
            _shieldLevel = PlayerPrefs.GetInt("Shield_Level", 0);
            
            if (_shieldLevel > 0)
            {
                Activate(); //Если куплен, включаем сразу при старте
            }
        }

        //Метод для обновления уровня
        public void SetLevel(int level)
        {
            _shieldLevel = level;
            Debug.Log($"Shield level: {_shieldLevel}");
        }

        //Метод для активации щита во время бега
        public void Activate()
        {
            if (_shieldLevel <= 0) return; //Если не куплен, то не работает
            if (IsActive) return; //Если уже активен, то не включаем заново

            //Длительность: базовые 3 секунды + 1 секунда за каждый уровень
            float duration = 3f + (_shieldLevel * 1f);
            StartCoroutine(ShieldRoutine(duration));
        }

        private IEnumerator ShieldRoutine(float duration)
        {
            IsActive = true;
            Debug.Log($"Activate shield for {duration} seconds");
            
            yield return new WaitForSeconds(duration); //Ждем время
            
            IsActive = false;
            Debug.Log("Shield inactive");
        }
    }
}
