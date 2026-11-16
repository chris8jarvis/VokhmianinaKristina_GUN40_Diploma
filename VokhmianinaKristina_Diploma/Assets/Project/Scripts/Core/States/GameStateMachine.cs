using UnityEngine;

namespace Project.Core.States
{
    public class GameStateMachine
    {
        private IGameState _currentState;

        //Метод для смены состояния
        public void ChangeState(IGameState newState)
        {
            //Выходим из старого состояния
            _currentState?.Exit(); 
            
            //Запоминаем новое
            _currentState = newState; 
            
            //Входим в новое состояние
            _currentState?.Enter(); 
        }
    }
}
