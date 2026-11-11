using UnityEngine;
using Zenject;
using Project.Player;
using Project.World;

namespace Project.Core
{
    public class GameInstaller : MonoInstaller
    {
        //Префабы препятствий 
        [Header("Obstacle Prefabs")]
        [SerializeField] private GameObject _lowObstaclePrefab;
        [SerializeField] private GameObject _highObstaclePrefab;
        [SerializeField] private GameObject _slideObstaclePrefab;
        [SerializeField] private GameObject _coinPrefab;
        public override void InstallBindings()
        {
            Container.Bind<PlayerInputHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle();

            Container.Bind<SegmentFactory>()
                .AsSingle()
                .WithArguments(_lowObstaclePrefab, _highObstaclePrefab, _slideObstaclePrefab, _coinPrefab);
            
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();

        }
    }
}