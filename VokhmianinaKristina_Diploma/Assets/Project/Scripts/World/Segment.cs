using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.World
{
    public class Segment : MonoBehaviour
    {
        //Список объектов, которые мы заспавнили на этом сегменте
        private List<IPoolableObject> _spawnedObjects = new List<IPoolableObject>();

        //Метод для добавления объекта в список
        public void AddObject(IPoolableObject obj)
        {
            _spawnedObjects.Add(obj);
        }

        //Метод для очистки (возврата в пул)
        public void Clear()
        {
            foreach (var obj in _spawnedObjects)
            {
                if (obj is MonoBehaviour mono && mono.gameObject.activeSelf)
                {
                    obj.Despawn();
                }
            }
            _spawnedObjects.Clear();
        }
    }
}