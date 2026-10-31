using UnityEngine;
using UnityEngine.InputSystem;
using Project.Player.Commands;

namespace Project.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        
        public Vector2 MoveInput { get; private set; }
        //public bool JumpPressed { get; private set; }
        public ICommand JumpCommand { get; private set; }
        public bool SlideRequested { get; private set; }

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            
            _inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

            //_inputActions.Player.Jump.performed += ctx => JumpPressed = true;
            _inputActions.Player.Jump.performed += ctx => JumpCommand = new JumpCommand();
            _inputActions.Player.Slide.performed += ctx => SlideRequested = true;
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void LateUpdate()
        {
            //JumpPressed = false;
            SlideRequested = false;
        }

        public void ClearJumpCommand() => JumpCommand = null;
    }
}
