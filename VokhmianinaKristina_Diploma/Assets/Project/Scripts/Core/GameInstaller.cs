using UnityEngine;
using Zenject;
using Project.Player;
using Project.World;
using Project.Core;
using Project.Core.Shop;

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

            Container.Bind<SegmentFactory>().AsSingle();
            
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShopService>().AsSingle();

            Container.BindMemoryPool<Coin, Coin.Pool>()
                .WithInitialSize(100)
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransformGroup("Pool_Coins");

            Container.BindMemoryPool<LowObstacle, LowObstacle.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_lowObstaclePrefab)
                .UnderTransformGroup("Pool_Low");

            Container.BindMemoryPool<HighObstacle, HighObstacle.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_highObstaclePrefab)
                .UnderTransformGroup("Pool_High");

            Container.BindMemoryPool<SlideObstacle, SlideObstacle.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_slideObstaclePrefab)
                .UnderTransformGroup("Pool_Slide");

        }
    }
}