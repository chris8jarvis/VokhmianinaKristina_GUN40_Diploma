using UnityEngine;
using Zenject;
using Project.Player.Commands;

namespace Project.Player
{
    [RequireComponent(typeof(CharacterController))] 
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 10f; 
        [SerializeField] private float acceleration = 0.5f; 
        [SerializeField] private float laneDistance = 3f; 
        [SerializeField] private float sideSpeed = 15f; 
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -20f;

        [Header("Slide Settings")]
        [SerializeField] private float slideDuration = 0.8f;
        [SerializeField] private float slideHeight = 1f;

        private PlayerInputHandler _inputHandler;
        private CharacterController _controller;
        
        private int _currentLane = 1;
        private float _currentSpeed;
        private float _verticalVelocity;

        private SlideCommand _activeSlideCommand;
        private float _normalHeight;

        [Inject]
        public void Construct(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _currentSpeed = forwardSpeed;
            _normalHeight = _controller.height;
        }

        private void Update()
        {
            //Движение вперед (ускорение)
            _currentSpeed += acceleration * Time.deltaTime;
            Vector3 moveVector = Vector3.forward * _currentSpeed;

            //Смена полос (по горизонтали)
            if (_inputHandler.MoveInput.x > 0.5f)
                _currentLane = Mathf.Min(_currentLane + 1, 2);
            else if (_inputHandler.MoveInput.x < -0.5f)
                _currentLane = Mathf.Max(_currentLane - 1, 0);

            float targetX = (_currentLane - 1) * laneDistance;
            moveVector.x = (targetX - transform.position.x) * sideSpeed;

            //Прыжок
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0) _verticalVelocity = -2f; // Прижимаем к земле

                if (_inputHandler.JumpCommand != null)
                {
                    _verticalVelocity = jumpForce;
                    _inputHandler.ClearJumpCommand();
                }
            }
            _verticalVelocity += gravity * Time.deltaTime;
            moveVector.y = _verticalVelocity;


            // Подкат
            // Если игрок нажал S и находится не в подкате
            if (_inputHandler.SlideRequested && _activeSlideCommand == null && _controller.isGrounded)
            {
                _activeSlideCommand = new SlideCommand(_controller, slideDuration, _normalHeight, slideHeight);
                _activeSlideCommand.Execute();
            }

            // Если подкат уже активен - обновляем его таймер
            if (_activeSlideCommand != null)
            {
                _activeSlideCommand.Tick(Time.deltaTime);
                
                if (Mathf.Approximately(_controller.height, _normalHeight))
                {
                    _activeSlideCommand = null;
                }
            }

            // Движение
            _controller.Move(moveVector * Time.deltaTime);
        }
    }
}