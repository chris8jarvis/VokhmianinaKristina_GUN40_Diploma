using UnityEngine;
using Zenject;
using Project.Player;

namespace Project.Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().FromComponentInHierarchy().AsSingle();
        }
    }
}