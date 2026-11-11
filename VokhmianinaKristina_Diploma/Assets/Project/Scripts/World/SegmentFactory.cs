using UnityEngine;
using Zenject;

namespace Project.World
{
    public class SegmentFactory
    {
        private readonly GameObject _lowPrefab;
        private readonly GameObject _highPrefab;
        private readonly GameObject _slidePrefab;
        private readonly float _laneDistance = 3f;

        
        public SegmentFactory(
            GameObject lowPrefab, 
            GameObject highPrefab, 
            GameObject slidePrefab)
        {
            _lowPrefab = lowPrefab;
            _highPrefab = highPrefab;
            _slidePrefab = slidePrefab;
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
        }
    }
}