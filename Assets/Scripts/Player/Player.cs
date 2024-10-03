using Fusion;
using Soccer;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private ETeam _team;
        
        public ETeam Team => _team;
    }
}
