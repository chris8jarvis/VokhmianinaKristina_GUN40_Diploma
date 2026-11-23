using UnityEngine;
using Cysharp.Threading.Tasks; 

namespace Project.Core.Upgrades
{
    public class Shield : MonoBehaviour
    {
        public bool IsActive { get; private set; } = false;
        private int _shieldLevel = 0;
        private float _timer = 0f; //Таймер обратного отсчета
        private bool _wasUsed = false;

        private void Start()
        {
            //Читаем уровень из сохранений
            _shieldLevel = PlayerPrefs.GetInt("Shield_Level", 0);
        }

        //Метод для обновления уровня
        public void SetLevel(int level)
        {
            _shieldLevel = level;
            //Debug.Log($"Shield level: {_shieldLevel}");
        }

        //Метод для активации щита во время бега
        public bool TryActivateShield()
        {
            if (_shieldLevel <= 0) return false; //Нет щита — урон проходит
            if (IsActive) return true; //Уже активен — урон игнорируется
            if (_wasUsed) return false;

            //Активация щита
            IsActive = true;
            _wasUsed = true;
            _timer = 3f + (_shieldLevel * 1f); //Длительность
            Debug.Log($"Щит активирован на {_timer} секунд!");
            
            return true;
        }

        private void Update()
        {
            if (!IsActive) return;

            //Уменьшаем таймер
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                IsActive = false;
                Debug.Log("Щит уничтожился");
            }
        }
    }
}
