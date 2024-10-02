using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField]
        private NetworkRunner _runner;

        public void HostAGame()
        {
            StartGame(GameMode.Host);
        }

        private async void StartGame(GameMode mode)
        {
            _runner.ProvideInput = true;
            var scene = CreateSceneRef();

            await _runner.StartGame(new StartGameArgs
                                    {
                                        GameMode = mode,
                                        SessionName = "TestRoom",
                                        Scene = scene,
                                        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
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
