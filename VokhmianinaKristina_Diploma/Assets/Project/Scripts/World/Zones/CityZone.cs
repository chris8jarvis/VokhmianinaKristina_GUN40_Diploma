using UnityEngine;

namespace Project.World
{
    public class CityZone : IZoneStrategy
    {
        private readonly GameObject[] _prefabs;

        //Передаем префабы
        public CityZone(GameObject lowPrefab, GameObject highPrefab)
        {
            _prefabs = new GameObject[] { lowPrefab, highPrefab };
        }

        public Color GetLightColor() => new Color(0.5f, 0.7f, 1f); //Голубое небо
        public GameObject[] GetObstaclePrefabs() => _prefabs;
    }
}