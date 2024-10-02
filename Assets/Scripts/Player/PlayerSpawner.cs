using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Player
{
    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        [SerializeField]
        private NetworkPrefabRef _playerPrefab;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
    
        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                var spawnPosition =
                    new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount * 3, 0, 0); // @todo: why 3?, whats rawEncoded etc?

                var networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }

        public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out var networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }
    }
}