using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private Rigidbody _rigidbody;
    
    [Networked]
    private NetworkInputData Inputs { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Inputs = data;
            
            Inputs.direction.Normalize();
            _rigidbody.velocity = _speed * Inputs.direction;
        }
    }
}
