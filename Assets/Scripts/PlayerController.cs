using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private NetworkCharacterController _characterController;

    [SerializeField]
    private float _speed = 10f;

    private void Reset()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(_speed * data.direction * Runner.DeltaTime);
        }
    }
}
