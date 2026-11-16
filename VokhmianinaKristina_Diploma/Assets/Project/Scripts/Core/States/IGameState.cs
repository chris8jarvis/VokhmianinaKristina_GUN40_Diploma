using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.States
{
    public interface IGameState
    {
        void Enter(); //Вход в состояние
        void Exit(); //Выход
    }
}
