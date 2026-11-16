using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Project.Core.Shop
{
    public class ShopView : MonoBehaviour
    {
        [Header("Magnet UI")]
        public TextMeshProUGUI MagnetLevelText;
        public TextMeshProUGUI MagnetPriceText;
        public Button MagnetBuyButton;

        [Header("Shield UI")]
        public TextMeshProUGUI ShieldLevelText;
        public TextMeshProUGUI ShieldPriceText;
        public Button ShieldBuyButton;

        [Header("Multiplier UI")]
        public TextMeshProUGUI MultiplierLevelText;
        public TextMeshProUGUI MultiplierPriceText;
        public Button MultiplierBuyButton;
    }
}
