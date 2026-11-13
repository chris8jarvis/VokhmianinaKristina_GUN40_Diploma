using UnityEngine;
using Zenject;
using Project.World;
using TMPro;
using Project.Player;

namespace Project.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _coinText;  //Текст для монет
        [SerializeField] private TextMeshProUGUI _scoreText; //Текст для очков
        [SerializeField] private GameObject _gameOverPanel;
        public int Coins { get; private set; } = 0;
        public int Score { get; private set; } = 0; //Текущий счет (дистанция + монеты)
        public int HighScore { get; private set; } = 0; //Рекорд
        public bool IsGameOver { get; private set; } = false;//Флаг, что игра окончена

        //Ссылка на игрока (внедряем через Zenject)
        private PlayerMovement _player;

        [Inject]
        public void Construct(PlayerMovement player)
        {
            _player = player;
        }

        private void OnEnable()
        {
            //Подписываемся на события
            Coin.OnCoinCollected += AddCoin;
            _player.OnPlayerDied += HandleGameOver; //Подписываемся на смерть игрока
        }

        private void OnDisable()
        {
            //Отписываемся
            Coin.OnCoinCollected -= AddCoin;
            _player.OnPlayerDied -= HandleGameOver;
        }

         private void Start()
        {
            //Загружаем рекорд из сохранений при старте
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
            
            //Выключаем панель Game Over при старте игры
            if (_gameOverPanel != null) 
            {
                _gameOverPanel.SetActive(false);
            }
            UpdateUI(); //Обновляем экран
        }
        private void Update()
        {
            //Если игра не окончена, считаем счет
            if (!IsGameOver)
            {
                //Берем позицию игрока по Z и округляем до целого
                Score = Mathf.FloorToInt(_player.transform.position.z);
                UpdateUI();
            }
        }
        private void AddCoin()
        {
            Coins++;
            UpdateUI();
        }
        private void HandleGameOver()
        {
            IsGameOver = true;
            Debug.Log("Game Over! Final Score: " + Score);

            //Проверяем, побит ли рекорд
            if (Score > HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt("HighScore", HighScore);//Сохраняем
                PlayerPrefs.Save(); //Принудительно сохраняем на диск
                Debug.Log("New record!");
            }

            //Показываем панель Game Over
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
            }

            //Останавливаем время
            Time.timeScale = 0f;
        }
        private void UpdateUI()
        {
            if (_coinText != null)
            {
                if (_coinText != null) _coinText.text = $"Coins: {Coins}";
                if (_scoreText != null) _scoreText.text = $"Score: {Score}";
            }
        }
    }
}
