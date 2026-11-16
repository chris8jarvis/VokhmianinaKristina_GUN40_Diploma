using UnityEngine;

namespace Project.Core.Shop
{
    [CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade Data")]
    public class UpgradeData : ScriptableObject
    {
        public string UpgradeName; //Название (например, "Магнит")
        public string Description; //Описание
        public int BasePrice = 50; //Базовая цена
        public int PriceIncrease = 25; //Насколько дорожает каждый уровень
        public int MaxLevel = 5; //Максимальный уровень
    }
}