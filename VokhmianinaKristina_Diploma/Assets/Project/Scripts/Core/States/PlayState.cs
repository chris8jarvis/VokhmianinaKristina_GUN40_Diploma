using UnityEngine;
using Zenject;

namespace Project.Core.States
{
    public class PlayState : IGameState
    {
        public void Enter()
        {
            Debug.Log("Вошли в состояние игры)");
            Time.timeScale = 1f; //Возобновляем время
        }

        public void Exit()
        {
            Debug.Log("Вышли из состояния игры");
        }
    }
}
