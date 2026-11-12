using UnityEngine;
using Zenject;
using Project.World;
using TMPro;

namespace Project.Core
{
    public class GameManager : MonoBehaviour
    {
        public int Coins { get; private set; } = 0;
        [SerializeField] private TextMeshProUGUI _coinText;

        private void OnEnable()
        {
            Coin.OnCoinCollected += AddCoin;
        }

        private void OnDisable()
        {
            Coin.OnCoinCollected -= AddCoin;
        }

         private void Start()
        {
            UpdateUI();
        }
        private void AddCoin()
        {
            Coins++;
            UpdateUI();
        }
        private void UpdateUI()
        {
            if (_coinText != null)
            {
                _coinText.text = $"Coins: {Coins}";
            }
        }
    }
}
