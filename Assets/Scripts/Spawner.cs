using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
    private NetworkRunner _runner;

    public void HostAGame()
    {
        StartGame(GameMode.Host);
    }

    // @todo: extract starting game and creating runner to separate class
    private async void StartGame(GameMode mode)
    {
        _runner = CreateRunner();
        var scene = CreateSceneRef();

        // Start or join (depends on gamemode) a session with a specific name
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

    #region NetworkCallbacks

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            var spawnPosition = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount * 3, 1, 0); // @todo: why 3?
            var networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out var networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // @todo: refactor, extract input system
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    { }

    public void OnConnectedToServer(NetworkRunner runner)
    { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    { }

    public void OnSceneLoadDone(NetworkRunner runner)
    { }

    public void OnSceneLoadStart(NetworkRunner runner)
    { }

    #endregion
}
