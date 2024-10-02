using Ball;
using Network;
using Player;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerSpawner _playerSpawner;

        [SerializeField]
        private BallSpawner _ballSpawner;
        
        public override void InstallBindings()
        {
            BindInterfacesFromInstance(_playerSpawner);
            BindInterfacesFromInstance(_ballSpawner);
            BindInterfacesTo<InputSampler>();
        }
        
        private void BindInterfacesFromInstance<T>(T instance)
        {
            Container.BindInterfacesTo<T>()
                     .FromInstance(instance)
                     .AsSingle()
                     .NonLazy();
        }
        
        protected void BindInterfacesTo<T>()
        {
            Container.BindInterfacesTo<T>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}
