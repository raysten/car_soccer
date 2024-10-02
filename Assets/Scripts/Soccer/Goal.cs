using UnityEngine;

namespace Soccer
{
    public class Goal : MonoBehaviour
    {
        [SerializeField]
        private ETeam _team;
        
        public ETeam Team => _team;
    }
}
