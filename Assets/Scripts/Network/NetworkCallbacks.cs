using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Player;
using Soccer;
using UnityEngine;
using Zenject;

namespace Network
{
    public class NetworkCallbacks : MonoBehaviour, INetworkRunnerCallbacks
    {
        private IPlayerSpawner _playerSpawner;
        private IBallSpawner _ballSpawner;
        private IInputSampler _inputSampler;

        [Inject]
        private void Construct(IPlayerSpawner playerSpawner, IBallSpawner ballSpawner, IInputSampler inputSampler)
        {
            _playerSpawner = playerSpawner;
            _ballSpawner = ballSpawner;
            _inputSampler = inputSampler;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                _playerSpawner.SpawnPlayer(runner, player);
                _ballSpawner.SpawnBall(runner);
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
            input.Set(_inputSampler.SampleInput());
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

        public void OnConnectRequest(
            NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
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

        public void OnReliableDataReceived(
            NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        { }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        { }

        public void OnSceneLoadDone(NetworkRunner runner)
        { }

        public void OnSceneLoadStart(NetworkRunner runner)
        { }

        #endregion
    }
}
