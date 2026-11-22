using UnityEngine;

namespace Project.World
{
    public interface IZoneStrategy
    {
        //Цвет освещения в зоне
        Color GetLightColor();
        
        //Префаб в зоне
        GameObject[] GetObstaclePrefabs();
    }
}
