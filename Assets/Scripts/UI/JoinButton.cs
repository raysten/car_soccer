using Network;
using UnityEngine;
using Zenject;

namespace UI
{
    public class JoinButton : MonoBehaviour
    {
        private IGameStarter _gameStarter;

        [Inject]
        private void Construct(IGameStarter gameStarter)
        {
            _gameStarter = gameStarter;
        }

        public void JoinGame()
        {
            _gameStarter.JoinGame();
        }
    }
}
