using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Player;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private Ball _ballPrefab;

    private Ball _ballInstance;
    
    private IPlayerSpawner _playerSpawner;

    [Inject]
    private void Construct(IPlayerSpawner playerSpawner)
    {
        _playerSpawner = playerSpawner;
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            _playerSpawner.SpawnPlayer(runner, player);

            SpawnBall(runner);
        }
    }

    private void SpawnBall(NetworkRunner runner)
    {
        if (_ballInstance == null)
        {
            _ballInstance = runner.Spawn(_ballPrefab);
            _ballInstance.OnEnteredGoal += GoalScored;
        }

        void GoalScored()
        {
            _ballInstance.OnEnteredGoal -= GoalScored;
            runner.Despawn(_ballInstance.Object);
            _ballInstance = null;

            SpawnBall(runner);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            _playerSpawner.DespawnPlayer(runner, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData
                   {
                       moveInput = Input.GetAxis("Vertical"),
                       steerInput = Input.GetAxis("Horizontal")
                   };

        input.Set(data);
    }

    #region UnusedNetworkCallbacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    { }

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
