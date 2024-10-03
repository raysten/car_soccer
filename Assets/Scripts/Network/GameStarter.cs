using Fusion;
using UnityEngine.SceneManagement;
using Zenject;

namespace Network
{
    public class GameStarter : IGameStarter
    {
        private NetworkRunner _networkRunner;

        [Inject]
        private void Construct(NetworkRunner networkRunner)
        {
            _networkRunner = networkRunner;
        }

        public void HostAGame()
        {
            StartGame(GameMode.Host);
        }

        private async void StartGame(GameMode mode)
        {
            _networkRunner.ProvideInput = true;
            var scene = CreateSceneRef();

            await _networkRunner.StartGame(new StartGameArgs
                                           {
                                               GameMode = mode,
                                               SessionName = "TestRoom",
                                               Scene = scene,
                                               SceneManager = _networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>()
                                           });
        }

        private SceneRef CreateSceneRef()
        {
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();

            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            return scene;
        }

        public void JoinGame()
        {
            StartGame(GameMode.Client);
        }
    }
}
