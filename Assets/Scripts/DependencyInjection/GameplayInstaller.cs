using Fusion;
using Network;
using Player;
using Soccer;
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

        [SerializeField]
        private Score _score;

        [SerializeField]
        private NetworkRunner _networkRunner;
        
        public override void InstallBindings()
        {
            BindInterfacesAndSelfFromInstance(_networkRunner);
            
            BindInterfacesFromInstance(_playerSpawner);
            BindInterfacesFromInstance(_ballSpawner);
            BindInterfacesFromInstance(_score);
            
            BindInterfacesTo<InputSampler>();
        }
        
        protected void BindInterfacesAndSelfFromInstance<T>(T instance)
        {
            Container.BindInterfacesAndSelfTo<T>()
                     .FromInstance(instance)
                     .AsSingle()
                     .NonLazy();
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
