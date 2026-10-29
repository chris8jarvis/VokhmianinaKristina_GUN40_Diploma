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

        private PlayerInputHandler _inputHandler;
        private CharacterController _controller;
        
        private int _currentLane = 1;
        private float _currentSpeed;
        private float _verticalVelocity;

        [Inject]
        public void Construct(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _currentSpeed = forwardSpeed;
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

            //Прыжок и гравитация
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0) _verticalVelocity = -2f; // Прижимаем к земле

                //Проверка комманды
                if (_inputHandler.CurrentCommand is JumpCommand)
                {
                    _verticalVelocity = jumpForce;
                    _inputHandler.ClearCommand();
                }
            }
            
            _verticalVelocity += gravity * Time.deltaTime;
            moveVector.y = _verticalVelocity;

            //Применяем движение
            _controller.Move(moveVector * Time.deltaTime);
        }
    }
}