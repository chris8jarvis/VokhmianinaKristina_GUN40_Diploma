using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player.Commands
{
    public class SlideCommand : ICommand
    {
        private readonly CharacterController _controller;
        private readonly float _slideDuration;
        private readonly float _normalHeight;
        private readonly float _slideHeight;
        
        private float _timer;
        public bool IsFinished { get; private set; }

        public SlideCommand(CharacterController controller, float duration, float normalHeight, float slideHeight)
        {
            _controller = controller;
            _slideDuration = duration;
            _normalHeight = normalHeight;
            _slideHeight = slideHeight;
        }

        public void Execute()
        {
            IsFinished = false;
            _timer = _slideDuration;
            
            // Уменьшаем высоту контроллера, центрируем его
            _controller.height = _slideHeight;
            _controller.center = new Vector3(0, _slideHeight / 2f, 0);
        }

        // Нужен Update, чтобы таймер работал 
        // Но у Command нет Update, поэтому добавляем метод, который будет вызывать PlayerMovement
        public void Tick(float deltaTime)
        {
            if (IsFinished) return;

            _timer -= deltaTime;
            
            if (_timer <= 0)
            {
                StopSlide();
            }
        }

        private void StopSlide()
        {
            IsFinished = true;
            _controller.height = _normalHeight;
            _controller.center = new Vector3(0, _normalHeight / 2f, 0);
        }
    }
}