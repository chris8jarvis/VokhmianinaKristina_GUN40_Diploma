using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player.Commands
{
    public class JumpCommand : ICommand
    {
        public void Execute()
        {
            // Временный вывод текста в консоль для проверки работы
            Debug.Log("Прыжок");
        }
    }
}
