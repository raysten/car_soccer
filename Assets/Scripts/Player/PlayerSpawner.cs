using System.Collections.Generic;
using System.Linq;
using Fusion;
using Soccer;
using UnityEngine;

namespace Player
{
    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        [SerializeField]
        private Player _redPlayerPrefab;

        [SerializeField]
        private Player _bluePlayerPrefab;

        private readonly Dictionary<PlayerRef, Player> _spawnedPlayers = new();

        private int RedPlayersCount => _spawnedPlayers.Values.Count(player => player.Team == ETeam.Red);
        private int BluePlayersCount => _spawnedPlayers.Values.Count(player => player.Team == ETeam.Blue);
    
        public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer)
            {
                var spawnPosition = CalculateSpawnPositionForNextPlayer(runner, playerRef);
                var playerPrefab = FindPlayerPrefabToSpawn();
                var networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, playerRef);
                _spawnedPlayers.Add(playerRef, networkPlayerObject);
            }
        }

        private Vector3 CalculateSpawnPositionForNextPlayer(NetworkRunner runner, PlayerRef player)
        {
            var spawnPosition = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount * 3, 0, 0);
            return spawnPosition;
        }

        private Player FindPlayerPrefabToSpawn()
        {
            var prefab = _redPlayerPrefab;

            if (RedPlayersCount > BluePlayersCount)
            {
                prefab = _bluePlayerPrefab;
            }
            
            return prefab;
        }

        public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedPlayers.TryGetValue(player, out var playerObject))
            {
                runner.Despawn(playerObject.Object);
                _spawnedPlayers.Remove(player);
            }
        }
    }
}