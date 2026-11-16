using UnityEngine;

namespace Project.Core.States
{
    public class PauseState : IGameState
    {
        private GameManager _gameManager;

        public PauseState(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Enter()
        {
            Debug.Log("Пауза");
            Time.timeScale = 0f;
            _gameManager.GetPausePanel().SetActive(true); //Включаем панель паузы
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            _gameManager.GetPausePanel().SetActive(false); //Выключаем панель
        }
    }
}