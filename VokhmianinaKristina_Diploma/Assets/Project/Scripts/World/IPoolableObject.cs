using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.World
{
    public interface IPoolableObject
    {
        void Despawn();
    }
}
