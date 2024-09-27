using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private NetworkCharacterController _characterController;

    private void Reset()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}
