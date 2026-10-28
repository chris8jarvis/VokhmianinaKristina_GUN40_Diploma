using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        
        public Vector2 MoveInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SlidePressed { get; private set; }

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            
            _inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

            _inputActions.Player.Jump.performed += ctx => JumpPressed = true;
            _inputActions.Player.Slide.performed += ctx => SlidePressed = true;
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void LateUpdate()
        {
            JumpPressed = false;
            SlidePressed = false;
        }
    }
}
