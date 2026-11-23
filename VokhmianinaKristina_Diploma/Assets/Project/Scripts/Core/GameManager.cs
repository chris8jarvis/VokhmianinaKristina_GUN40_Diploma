using UnityEngine;
using Zenject;
using Project.World;
using Project.Player;
using Project.Core.States;
using TMPro;
using UnityEngine.SceneManagement;
using Project.Core.Upgrades;
using Cysharp.Threading.Tasks;
using System.Threading;
using Project.Core;
using System.Linq;

namespace Project.Core
{
    public class GameManager : MonoBehaviour
    {

        //--- Поля для UI ---
        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _coinText;  //Текст для монет
        [SerializeField] private TextMeshProUGUI _scoreText; //Текст для очков
        [SerializeField] private TextMeshProUGUI _walletText; //Текст для кошелька
        [SerializeField] private GameObject _gameOverPanel; //Меню окончания игры
        [SerializeField] private GameObject _pausePanel; //Панель паузы
        [SerializeField] private GameObject _mainMenuPanel; //Главное меню
        [SerializeField] private TextMeshProUGUI _highScoreText; //Текст для рекорда
        [SerializeField] private GameObject _shopPanel; //Панель магазина
        [SerializeField] private TextMeshProUGUI _countdownText; //Текст отсчета времени перед игрой

        // --- Данные ---
        public int Coins { get; private set; } = 0;
        public int TotalCoins { get; private set; } = 0; //Кошелек
        public int Score { get; private set; } = 0; //Текущий счет
        public int HighScore { get; private set; } = 0; //Рекорд
        public bool IsGameOver { get; private set; } = false;//Флаг, что игра окончена
        private PlayerMovement _player; //Ссылка на игрока (внедряем через Zenject)
        private float _startZ; //Запоминаем позицию игрока
        private float _restartTimer = 0f;
        private GameStateMachine _stateMachine; //Машина состояний
        // Ссылки на состояния
        private PlayState _playState;
        private PauseState _pauseState;
        private GameOverState _gameOverState;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(PlayerMovement player, SignalBus signalBus)
        {
            _player = player;
            _signalBus = signalBus;
        }

         private void Awake()
        {
            //Инициализируем машину состояний и сами состояния
            _stateMachine = new GameStateMachine();
            _playState = new PlayState(this);
            _pauseState = new PauseState(this);
            _gameOverState = new GameOverState(this);
        }
        private void Start()
        {
            FindObjectsOfType<Obstacle>().ToList().ForEach(o => o.Despawn());
            FindObjectsOfType<Coin>().ToList().ForEach(c => c.Despawn());

            _player.enabled = false;

            //Загружаем рекорд из сохранений при старте
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
            if (_highScoreText != null) _highScoreText.text = $"Record: {HighScore}";

            //Загрузка кошелька
            TotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

            //Обнуление монет для текущего забега
            Coins = 0; 

            //Запоминаем где находится игрок при старте
            _startZ = _player.transform.position.z;

            //Показываем главное меню при старте
            _gameOverPanel.SetActive(false);
            _pausePanel.SetActive(false);
            _mainMenuPanel.SetActive(true);

            _player.OnPlayerDied -= HandleGameOver;
            _player.OnPlayerDied += HandleGameOver;

            _signalBus.Subscribe<CoinCollectedSignal>(AddCoin);
            
            Time.timeScale = 0f;
            UpdateUI();
        }

        private void OnDisable()
        {
            if (_player != null) _player.OnPlayerDied -= HandleGameOver;
         }
        
        private void Update()
        {
            if (IsGameOver) return;

            //Считаем счет
            float distance = _player.transform.position.z - _startZ;
            Score = Mathf.Max(0, Mathf.FloorToInt(distance));
            UpdateUI();

            //Обработка паузы (TAB)
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePause();
            }

            //Хоткей рестарта (зажатие ~ на 1-2 секунды)
            if (Input.GetKeyDown(KeyCode.BackQuote)) //BackQuote = клавиша ~ (тильда)
            {
                _restartTimer = 0f; //Начинаем отсчет
            }

            if (Input.GetKey(KeyCode.BackQuote)) //Если клавиша зажата
            {
                _restartTimer += Time.unscaledDeltaTime; //Считаем время (unscaled, чтобы работало даже на паузе)

                if (_restartTimer >= 1.5f) //Если держим 1.5 секунды
                {
                    RestartGame();
                }
            }
            else
            {
                _restartTimer = 0f; //Если отпустили — сбрасываем таймер
            }
        }

        public async void StartGame()
        {
            _mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;

            //Блокировка управления, чтобы игрок не бежал раньше времени отсчета
            _player.enabled = false; 
            _countdownText.gameObject.SetActive(true);

            //Отсчет
            for (int i = 3; i > 0; i--)
            {
                if (_countdownText == null) return; 

                _countdownText.text = i.ToString();
                await UniTask.Delay(System.TimeSpan.FromSeconds(1));
            }

            if (_countdownText == null) return;

            _countdownText.text = "GO!";
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));

            if (_countdownText == null) return;

            // Включаем управление
            _countdownText.gameObject.SetActive(false);
            _player.enabled = true;

            _stateMachine.ChangeState(_playState); //Переключаемся в игру
            
        }

        public void TogglePause()
        {
            if (IsGameOver) return; //На паузу можно ставить только в игре

            if (Time.timeScale > 0)
            {
                _stateMachine.ChangeState(_pauseState); //Ставим на паузу
            }
            else
            {
                _stateMachine.ChangeState(_playState); //Снимаем с паузы
            }
        }

        public void RestartGame()
        {
            //Перезагружаем текущую сцену
            //Debug.Log("Рестарт игры");
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll(); //Удаляем все сохранения
            PlayerPrefs.Save();
            //Debug.Log("Прогресс сброшен");

            //Перезагружаем сцену, чтобы обновить UI
            RestartGame(); 
        }

        public void QuitGame()
        {
            Application.Quit();
            //Debug.Log("Игра закончена");
        }

        private void AddCoin()
        {
            //Debug.Log($"AddCoin: {Coins + 1}");
            if (_player == null) return;

            //Ищем компонент CoinMultiplier на игроке
            CoinMultiplier multiplier = _player.GetComponent<CoinMultiplier>();

            //Если нашли — берем его значение, если нет — умножаем на 1
            int amount = (multiplier != null) ? multiplier.Multiplier : 1;
            Coins += amount;
            TotalCoins += amount;


            // Coins++; //Увеличение текущих монет
            // TotalCoins++; //Увеличение кошелька
            PlayerPrefs.SetInt("TotalCoins", TotalCoins);
            PlayerPrefs.Save();
            UpdateUI();
        }

        //Метод для списания монет
        public void SpendCoins(int amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt("TotalCoins", TotalCoins);
            PlayerPrefs.Save();
            UpdateUI();
        }
        private void HandleGameOver()
        {
            if (IsGameOver) return;
            IsGameOver = true;

            //Проверяем, побит ли рекорд
            if (Score > HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt("HighScore", HighScore);
                PlayerPrefs.Save();
                //Debug.Log("Новый рекорд");
            }

            if (_highScoreText != null) 
            {
                _highScoreText.text = $"Record: {HighScore}";
            }

            if (_gameOverPanel != null) _gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }
        public void OpenShop()
        {
            _mainMenuPanel.SetActive(false); //Скрываем меню
            _shopPanel.SetActive(true); //Показываем магазин
            Time.timeScale = 0f;
        }
        public void CloseShop()
        {
            _shopPanel.SetActive(false); //Скрываем магазин
            _mainMenuPanel.SetActive(true); //Показываем меню обратно
            Time.timeScale = 1f;
        }
        private void UpdateUI()
        {
            //Показываем текущие монеты в игре
            if (_coinText != null) _coinText.text = $"Coins: {Coins}";
            if (_scoreText != null) _scoreText.text = $"Score: {Score}";
            if (_walletText != null) _walletText.text = $"Wallet: {TotalCoins}";
        }
        
        //Геттеры для состояний (чтобы они имели доступ к панелям)
        public GameObject GetGameOverPanel() => _gameOverPanel;
        public GameObject GetPausePanel() => _pausePanel;
        public PlayerMovement GetPlayer() => _player;
    }
}
