using UnityEngine;
using System;
using Zenject;

namespace Project.Core.Shop
{
    public class ShopService
    {
        //Событие: когда данные в магазине меняются (для обновления UI)
        public event Action OnShopUpdated;

        private GameManager _gameManager; //Ссылка на GameManager

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        //Получить текущий уровень апгрейда
        public int GetLevel(UpgradeData data)
        {
            return PlayerPrefs.GetInt(data.UpgradeName + "_Level", 0);
        }

        //Получить цену следующего уровня
        public int GetCurrentPrice(UpgradeData data)
        {
            int level = GetLevel(data);
            return data.BasePrice + (level * data.PriceIncrease);
        }

        //Попытка купить апгрейд
        public bool TryBuyUpgrade(UpgradeData data)
        {
            int level = GetLevel(data);
            
            if (level >= data.MaxLevel) return false; //Уже максимум

            int price = GetCurrentPrice(data);
            
            //Проверяем, хватает ли денег в кошельке
            if (_gameManager.TotalCoins >= price)
            {
                //Списываем деньги
                _gameManager.SpendCoins(price);
                
                //Повышаем уровень и сохраняем
                PlayerPrefs.SetInt(data.UpgradeName + "_Level", level + 1);
                PlayerPrefs.Save();

                OnShopUpdated?.Invoke(); //Обновление
                return true;
            }

            return false; //Не хватает денег
        }
    }
}
