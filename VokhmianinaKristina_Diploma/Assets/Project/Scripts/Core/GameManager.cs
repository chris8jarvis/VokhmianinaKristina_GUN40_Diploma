using UnityEngine;
using Zenject;
using Project.World;

namespace Project.Core
{
    public class GameManager : MonoBehaviour
    {
        public int Coins { get; private set; } = 0;

        private void OnEnable()
        {
            Coin.OnCoinCollected += AddCoin;
        }

        private void OnDisable()
        {
            Coin.OnCoinCollected -= AddCoin;
        }

        private void AddCoin()
        {
            Coins++;
            Debug.Log($"Монет собрано: {Coins}");
        }
    }
}
