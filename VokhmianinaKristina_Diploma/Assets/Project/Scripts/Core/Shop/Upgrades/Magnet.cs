using UnityEngine;
using System.Collections.Generic;
using Project.World;

namespace Project.Player
{
    public class Magnet : MonoBehaviour
    {
        [Header("Magnet Settings")]
        [SerializeField] private float _baseRadius = 5f; 
        [SerializeField] private float _pullSpeed = 20f; 

        private float _currentRadius;
        private Collider[] _hitColliders = new Collider[20]; //Массив для результатов поиска

        private void Start()
        {
            _currentRadius = _baseRadius;
        }

        private void Update()
        {
            //Ищем все коллайдеры в радиусе от центра игрока
            int numFound = Physics.OverlapSphereNonAlloc(transform.position, _currentRadius, _hitColliders);

            //Проходимся по найденным объектам
            for (int i = 0; i < numFound; i++)
            {
                Collider hit = _hitColliders[i];

                //Если это монетка (проверяем по тегу)
                if (hit.CompareTag("Coin"))
                {
                    Transform coin = hit.transform;

                    //Двигаем монетку к игроку
                    coin.position = Vector3.MoveTowards(coin.position, transform.position, _pullSpeed * Time.deltaTime);

                    //Если подлетела близко — собираем
                    if (Vector3.Distance(coin.position, transform.position) < 1.5f)
                    {
                        Coin coinScript = coin.GetComponent<Coin>();
                        if (coinScript != null)
                        {
                            coinScript.Collect();
                        }
                    }
                }
            }
        }

        public void SetRadius(float newRadius)
        {
            _currentRadius = newRadius;
        }
    }
}