using UnityEngine;
using Zenject;

namespace Project.World
{
    public class SegmentFactory
    {
        private readonly LowObstacle.Pool _lowPool;
        private readonly HighObstacle.Pool _highPool;
        private readonly SlideObstacle.Pool _slidePool;
        private readonly Coin.Pool _coinPool;
        private readonly float _laneDistance = 3f;

        
        public SegmentFactory(
        LowObstacle.Pool lowPool,
        HighObstacle.Pool highPool,
        SlideObstacle.Pool slidePool,
        Coin.Pool coinPool)
        {
            _lowPool = lowPool;
            _highPool = highPool;
            _slidePool = slidePool;
            _coinPool = coinPool;
        }

        //Метод, выдающий препятствия
        public void PopulateSegment(Transform segmentTransform, float zPosition)
        {
            //Получаем Segment на префабе дороги
            Segment segment = segmentTransform.GetComponent<Segment>();

            int randomLane = Random.Range(0, 3); 
            float xPosition = (randomLane - 1) * _laneDistance;
            int randomType = Random.Range(0, 3);

            Obstacle obstacle = null;
            if (randomType == 0) obstacle = _lowPool.Spawn();
            else if (randomType == 1) obstacle = _highPool.Spawn();
            else if (randomType == 2) obstacle = _slidePool.Spawn();

            if (obstacle != null)
            {
                obstacle.transform.position = new Vector3(xPosition, obstacle.transform.position.y, segmentTransform.position.z + zPosition);
                segment.AddObject(obstacle);
            }

            //Монетки
            int coinCount = Random.Range(1, 3); 
            for (int i = 0; i < coinCount; i++)
            {
                int coinLane = Random.Range(0, 3);
                if (coinLane == randomLane) coinLane = (coinLane + 1) % 3;
                float coinX = (coinLane - 1) * _laneDistance;
                
                Coin coin = _coinPool.Spawn();
                coin.transform.position = new Vector3(coinX, 1f, segmentTransform.position.z + zPosition + 5f);
                
                //Добавляем в список сегмента
                segment.AddObject(coin);
            }
        }
    }
}