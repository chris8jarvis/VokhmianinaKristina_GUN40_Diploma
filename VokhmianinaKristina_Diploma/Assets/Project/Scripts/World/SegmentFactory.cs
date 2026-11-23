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
        public void PopulateSegment(Transform segmentTransform, float zPosition, IZoneStrategy zoneStrategy)
        {
            //Получаем Segment на префабе дороги
            Segment segment = segmentTransform.GetComponent<Segment>();

            if (segment != null) 
            {
                segment.Clear(); 
            }
            
            if (segmentTransform.position.z < 40f)
            {
                return;
            }

            //Спавн препятствий
            int randomLane = Random.Range(0, 3); 
            float xPosition = (randomLane - 1) * _laneDistance;

            Obstacle obstacle = null;

            //Проверяем, какая у нас зона, выбираем пул
            if (zoneStrategy is CityZone)
            {
                //В городе: Low или High (0 или 1)
                int type = Random.Range(0, 2); 
                if (type == 0) obstacle = _lowPool.Spawn();
                else obstacle = _highPool.Spawn();
            }
            else if (zoneStrategy is CaveZone)
            {
                //В пещере: High или Slide (1 или 2)
                int type = Random.Range(1, 3); 
                if (type == 1) obstacle = _highPool.Spawn();
                else obstacle = _slidePool.Spawn();
            }

            if (obstacle != null)
            {
                obstacle.transform.position = new Vector3(xPosition, obstacle.transform.position.y, segmentTransform.position.z + zPosition);
                if (segment != null) 
                {
                    segment.AddObject(obstacle);
                }
            }

            //Спавн Монеток
            int coinCount = Random.Range(1, 3); 
            for (int i = 0; i < coinCount; i++)
            {
                int coinLane = Random.Range(0, 3);
                if (coinLane == randomLane) coinLane = (coinLane + 1) % 3;
                float coinX = (coinLane - 1) * _laneDistance;
                
                Coin coin = _coinPool.Spawn();
                coin.transform.position = new Vector3(coinX, 1f, segmentTransform.position.z + zPosition + 5f);
                
                if (segment != null) 
                {
                    segment.AddObject(coin);
                }
            }
        }
    }
}