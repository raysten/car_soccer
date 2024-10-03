using Network;
using UnityEngine;
using Zenject;

namespace UI
{
    public class HostButton : MonoBehaviour
    {
        private IGameStarter _gameStarter;

        [Inject]
        private void Construct(IGameStarter gameStarter)
        {
            _gameStarter = gameStarter;
        }

        public void HostGame()
        {
            _gameStarter.HostAGame();
        }
    }
}
