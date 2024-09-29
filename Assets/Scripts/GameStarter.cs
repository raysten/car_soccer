using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner _runner;
    
    [SerializeField]
    private RunnerSimulatePhysics3D _runnerSimulatePhysics;
    
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