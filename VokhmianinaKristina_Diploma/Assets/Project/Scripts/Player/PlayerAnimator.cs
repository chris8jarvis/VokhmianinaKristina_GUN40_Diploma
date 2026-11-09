using UnityEngine;
using Zenject;
using Project.Player.Commands;

namespace Project.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        private PlayerMovement _playerMovement;

        [Inject]
        public void Construct(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_animator == null || _playerMovement == null) return;

            //Берем состояния из PlayerMovement
            bool isJumping = _playerMovement.IsJumping;
            bool isSliding = _playerMovement.IsSliding;

            //Передаем в Animator
            _animator.SetBool("IsJumping", isJumping);
            _animator.SetBool("IsSliding", isSliding);
        }
    }
}
