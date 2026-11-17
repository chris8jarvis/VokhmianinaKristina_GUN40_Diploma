using UnityEngine;

namespace Project.Core.Upgrades
{
    public class CoinMultiplier : MonoBehaviour
    {
        public int Multiplier { get; private set; } = 1; //Множитель (x1)

        public void SetMultiplier(int level)
        {
            //Логика: 1 уровень = x2, 2 уровень = x3 и т.д.
            Multiplier = 1 + level;
        }
    }
}
