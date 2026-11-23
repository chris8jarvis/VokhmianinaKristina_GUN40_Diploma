using UnityEngine;
using Zenject;

namespace Project.Core.States
{
    public class PlayState : IGameState
    {
        private GameManager _gameManager;

        public PlayState(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        public void Enter()
        {
            Time.timeScale = 1f; //Возобновляем время
        }

        public void Exit()
        {
        }
    }
}
