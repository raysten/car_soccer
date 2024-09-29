using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private Rigidbody _rigidbody;
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _rigidbody.velocity = _speed * data.direction;
        }
    }
}
