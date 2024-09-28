using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private NetworkRunner _runner; // @todo: not really needed cached

    public void HostAGame()
    {
        StartGame(GameMode.Host);
    }

    private async void StartGame(GameMode mode)
    {
        _runner = CreateRunner();
        var scene = CreateSceneRef();

        await _runner.StartGame(new StartGameArgs
                                {
                                    GameMode = mode,
                                    SessionName = "TestRoom",
                                    Scene = scene,
                                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                                });
    }

    private NetworkRunner CreateRunner()
    {
        var runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        gameObject.AddComponent<RunnerSimulatePhysics3D>();

        return runner;
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