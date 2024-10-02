using Player;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerSpawner _playerSpawner;
        
        public override void InstallBindings()
        {
            BindInterfacesFromInstance(_playerSpawner);
        }
        
        private void BindInterfacesFromInstance<T>(T instance)
        {
            Container.BindInterfacesTo<T>()
                     .FromInstance(instance)
                     .AsSingle()
                     .NonLazy();
        }
    }
}
