using UnityEngine;
using Zenject;
using Project.World;
using Project.Player;
using Project.Core.States;
using TMPro;
using UnityEngine.SceneManagement;

namespace Project.Core
{
    public class GameManager : MonoBehaviour
    {

        //--- Поля для UI ---
        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _coinText;  //Текст для монет
        [SerializeField] private TextMeshProUGUI _scoreText; //Текст для очков
        [SerializeField] private GameObject _gameOverPanel; //Меню окончания игры
        [SerializeField] private GameObject _pausePanel; //Панель паузы
        [SerializeField] private GameObject _mainMenuPanel; //Главное меню

        // --- Данные ---
        public int Coins { get; private set; } = 0;
        public int Score { get; private set; } = 0; //Текущий счет
        public int HighScore { get; private set; } = 0; //Рекорд
        public bool IsGameOver { get; private set; } = false;//Флаг, что игра окончена
        private PlayerMovement _player; //Ссылка на игрока (внедряем через Zenject)
        private float _startZ; //Запоминаем позицию игрока
        private GameStateMachine _stateMachine; //Машина состояний
        // Ссылки на состояния
        private PlayState _playState;
        private PauseState _pauseState;
        private GameOverState _gameOverState;

        [Inject]
        public void Construct(PlayerMovement player)
        {
            _player = player;
        }

         private void Awake()
        {
            //Инициализируем машину состояний и сами состояния
            _stateMachine = new GameStateMachine();
            _playState = new PlayState(this);
            _pauseState = new PauseState(this);
            _gameOverState = new GameOverState(this);
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

            //Запоминаем где находится игрок при старте
            _startZ = _player.transform.position.z;

            // Показываем главное меню при старте
            _gameOverPanel.SetActive(false);
            _pausePanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            
            Time.timeScale = 0f;
            //Выключаем панель Game Over при старте игры
            // if (_gameOverPanel != null) 
            // {
            //     _gameOverPanel.SetActive(false);
            // }
            // UpdateUI(); //Обновляем экран
        }
        private void Update()
        {
            if (IsGameOver) return;

            //Считаем счет
            float distance = _player.transform.position.z - _startZ;
            Score = Mathf.Max(0, Mathf.FloorToInt(distance));
            UpdateUI();

            // Обработка паузы (TAB)
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePause();
            }
        }

        public void StartGame()
        {
            _mainMenuPanel.SetActive(false);
            _stateMachine.ChangeState(_playState); //Переключаемся в игру
            Time.timeScale = 1f;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Игра закончена");
        }

        private void AddCoin()
        {
            Coins++;
            UpdateUI();
        }
        private void HandleGameOver()
        {
            IsGameOver = true;
            _stateMachine.ChangeState(_gameOverState);

            //Проверяем, побит ли рекорд
            // if (Score > HighScore)
            // {
            //     HighScore = Score;
            //     PlayerPrefs.SetInt("HighScore", HighScore);//Сохраняем
            //     PlayerPrefs.Save(); //Принудительно сохраняем на диск
            //     Debug.Log("New record!");
            // }

            // //Показываем панель Game Over
            // if (_gameOverPanel != null)
            // {
            //     _gameOverPanel.SetActive(true);
            // }

            // //Останавливаем время
            // Time.timeScale = 0f;
        }
        private void UpdateUI()
        {
            if (_coinText != null) _coinText.text = $"Coins: {Coins}";
            if (_scoreText != null) _scoreText.text = $"Score: {Score}";
        }
        
        //Геттеры для состояний (чтобы они имели доступ к панелям)
        public GameObject GetGameOverPanel() => _gameOverPanel;
        public GameObject GetPausePanel() => _pausePanel;
    }
}
