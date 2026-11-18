using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.World
{
    public class LowObstacle : Obstacle 
    { 
        public class Pool : MemoryPool<LowObstacle> { } 
    }
}
