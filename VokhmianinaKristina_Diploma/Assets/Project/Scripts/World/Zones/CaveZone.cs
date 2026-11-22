using UnityEngine;

namespace Project.World
{
    public class CaveZone : IZoneStrategy
    {
        private readonly GameObject[] _prefabs;

        //Передаем префабы
        public CaveZone(GameObject highPrefab, GameObject slidePrefab)
        {
            _prefabs = new GameObject[] { highPrefab, slidePrefab };
        }

        public Color GetLightColor() => new Color(0.2f, 0.1f, 0.1f); //Темное небо
        public GameObject[] GetObstaclePrefabs() => _prefabs;
    }
}
