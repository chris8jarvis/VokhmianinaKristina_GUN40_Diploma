using UnityEngine;
using Zenject;

namespace Project.World
{
    public class SegmentFactory
    {
        private readonly GameObject _lowPrefab;
        private readonly GameObject _highPrefab;
        private readonly GameObject _slidePrefab;
        private readonly GameObject _coinPrefab;
        private readonly float _laneDistance = 3f;

        
        public SegmentFactory(
            GameObject lowPrefab, 
            GameObject highPrefab, 
            GameObject slidePrefab,
            GameObject coinPrefab)
        {
            _lowPrefab = lowPrefab;
            _highPrefab = highPrefab;
            _slidePrefab = slidePrefab;
            _coinPrefab = coinPrefab; 
        }

        //Метод, выдающий препятствия
        public void PopulateSegment(Transform segment, float zPosition)
        {
            //Случайно выбираем полосу (-1, 0, 1) и умножаем на 3 (laneDistance)
            int randomLane = Random.Range(0, 3); 
            float xPosition = (randomLane - 1) * _laneDistance;

            //Случайно выбираем тип препятствия
            int randomType = Random.Range(0, 3);
            

            GameObject obstacleToSpawn = null;

            switch (randomType)
            {
                case 0: obstacleToSpawn = _lowPrefab; break;
                case 1: obstacleToSpawn = _highPrefab; break;
                case 2: obstacleToSpawn = _slidePrefab; break;
            }

            //Создаем препятствие внутри сегмента
            if (obstacleToSpawn != null)
            {
                //Временно используем Instantiate, позже будет пул.
                GameObject obstacle = Object.Instantiate(obstacleToSpawn);

                obstacle.transform.position = new Vector3(
                xPosition, 
                obstacle.transform.position.y, 
                segment.position.z + zPosition);
            }

            //Спавн монет
            //Генерируем 1-2 монетки на сегмент
            int coinCount = Random.Range(1, 3); 

            for (int i = 0; i < coinCount; i++)
            {
                //Выбираем случайную полосу для монетки
                int coinLane = Random.Range(0, 3);
                
                //Если полоса монетки совпадает с полосой препятствия, 
                //сдвигаем монетку на соседнюю полосу
                if (coinLane == randomLane) 
                {
                    coinLane = (coinLane + 1) % 3; //Сдвигаем на следующую полосу (0->1, 1->2, 2->0)
                }

                float coinX = (coinLane - 1) * _laneDistance;
                
                //Спавним монетку
                GameObject coin = Object.Instantiate(_coinPrefab);
                
                //Ставим моненту чуть выше земли (Y = 1), чтобы кот её задел коллайдером
                coin.transform.position = new Vector3(
                    coinX, 
                    1f, //Высота монетки
                    segment.position.z + zPosition + 5f //Сдвигаем по Z, чтобы она была не ровно в препятствии, а чуть дальше/ближе
                );
            }
        }
    }
}