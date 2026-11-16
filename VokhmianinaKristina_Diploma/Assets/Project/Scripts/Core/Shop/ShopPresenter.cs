using UnityEngine;
using Zenject;

namespace Project.Core.Shop
{
    public class ShopPresenter : MonoBehaviour
    {
        [SerializeField] private ShopView _view; //Ссылка на витрину
        [SerializeField] private UpgradeData _magnetData; //Ссылка на данные магнита
        [SerializeField] private UpgradeData _shieldData;
        [SerializeField] private UpgradeData _multiplierData;

        private ShopService _shopService;
        private GameManager _gameManager;

        [Inject]
        public void Construct(ShopService shopService, GameManager gameManager)
        {
            _shopService = shopService;
            _gameManager = gameManager;
        }

        private void Start()
        {
            //Подписываемся на обновления магазина
            _shopService.OnShopUpdated += UpdateUI;
            
            //Подписываемся на кнопки
            _view.MagnetBuyButton.onClick.AddListener(() => BuyUpgrade(_magnetData));
            _view.ShieldBuyButton.onClick.AddListener(() => BuyUpgrade(_shieldData));
            _view.MultiplierBuyButton.onClick.AddListener(() => BuyUpgrade(_multiplierData));

            UpdateUI(); //Обновляем витрину при старте
        }

        private void OnDestroy()
        {
            //Отписываемся
            _shopService.OnShopUpdated -= UpdateUI;
        }

        private void BuyUpgrade(UpgradeData data)
        {
            bool success = _shopService.TryBuyUpgrade(data);
            if (success)
            {
                Debug.Log($"Receive Upgrade: {data.UpgradeName}");
            }
            else
            {
                Debug.Log("Not enough Coins or Maximum Level reached");
            }
        }

        private void UpdateUI()
        {
            //Обновляем магнит
            int magnetLevel = _shopService.GetLevel(_magnetData);
            int magnetPrice = _shopService.GetCurrentPrice(_magnetData);
            _view.MagnetLevelText.text = $"Lvl: {magnetLevel}";
            _view.MagnetPriceText.text = $"{magnetPrice}";
            _view.MagnetBuyButton.interactable = _gameManager.TotalCoins >= magnetPrice;

            int shieldLevel = _shopService.GetLevel(_shieldData);
            int shieldPrice = _shopService.GetCurrentPrice(_shieldData);
            _view.ShieldLevelText.text = $"Lvl: {shieldLevel}";
            _view.ShieldPriceText.text = $"{shieldPrice}";
            _view.ShieldBuyButton.interactable = _gameManager.TotalCoins >= shieldPrice;

            int multiplierLevel = _shopService.GetLevel(_multiplierData);
            int multiplierPrice = _shopService.GetCurrentPrice(_multiplierData);
            _view.MultiplierLevelText.text = $"Lvl: {multiplierLevel}";
            _view.MultiplierPriceText.text = $"{multiplierPrice}";
            _view.MultiplierBuyButton.interactable = _gameManager.TotalCoins >= multiplierPrice;

        }
    }
}