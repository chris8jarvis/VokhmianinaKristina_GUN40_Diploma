using UnityEngine;

namespace Project.Core.States
{
    public class GameOverState : IGameState
    {
        private GameManager _gameManager;

        public GameOverState(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Enter()
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;
            _gameManager.GetGameOverPanel().SetActive(true);
        }

        public void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}