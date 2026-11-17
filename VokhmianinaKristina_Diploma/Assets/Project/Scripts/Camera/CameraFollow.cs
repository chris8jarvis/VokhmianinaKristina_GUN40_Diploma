using UnityEngine;
using Zenject;
using Project.Player;

namespace Project.Core
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 3, -5);
        [SerializeField] private float _smoothSpeed = 0.125f;

        private Transform _target; //За кем следит камера

        [Inject]
        public void Construct(PlayerMovement playerMovement)
        {
            //Zenject передает нам игрока
            _target = playerMovement.transform;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            //Вычисляем, где камера должна быть (позиция игрока + смещение)
            Vector3 desiredPosition = _target.position + _offset;
            
            //Плавно двигаем камеру к этой точке (используя Lerp)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
