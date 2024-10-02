using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Transform _interpolationTarget;

    private ChangeDetector _changeDetector;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Quaternion _previousRotation;
    private Quaternion _currentRotation;

    private float _interpolationTimer;

    [Networked]
    private Vector3 NetworkedPosition { get; set; }

    [Networked]
    private Quaternion NetworkedRotation { get; set; }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void FixedUpdateNetwork()
    {
        SimulateOnServer();
    }

    private void SimulateOnServer()
    {
        if (GetInput(out NetworkInputData data) && HasStateAuthority)
        {
            MovePlayer(data);
            CacheNetworkedOrientation();
        }
    }

    private void MovePlayer(NetworkInputData data)
    {
        data.direction.Normalize();
        _rigidbody.velocity = _speed * data.direction;
    }

    private void CacheNetworkedOrientation()
    {
        NetworkedPosition = _rigidbody.position;
        NetworkedRotation = _rigidbody.rotation;
    }

    public override void Render()
    {
        base.Render();

        InterpolateLocalClient();
    }

    /// <summary>
    /// Interpolation is very basic as I expected it to be a built-in feature,
    /// but for some reason rigidbody is not synced with server if client has input authority
    /// Some people mention it as a bug on Photon's discord and devs haven't responded to it so this is my simple workaround
    /// </summary>
    private void InterpolateLocalClient()
    {
        if (HasInputAuthority && HasStateAuthority == false)
        {
            DetectNetworkedRigidBodyChanges();
            Interpolate();
        }
    }

    private void DetectNetworkedRigidBodyChanges()
    {
        foreach (var propertyName in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (propertyName)
            {
                case nameof(NetworkedPosition):
                {
                    var positionReader = GetPropertyReader<Vector3>(propertyName);
                    (_previousPosition, _currentPosition) = positionReader.Read(previousBuffer, currentBuffer);
                    break;
                }
                case nameof(NetworkedRotation):
                    var rotationReader = GetPropertyReader<Quaternion>(propertyName);
                    (_previousRotation, _currentRotation) = rotationReader.Read(previousBuffer, currentBuffer);
                    break;
            }
        }
    }

    private void Interpolate()
    {
        _interpolationTimer += Time.deltaTime;
        var interpolationValue = Mathf.Clamp01(_interpolationTimer / Runner.DeltaTime);

        if (interpolationValue >= 1f)
        {
            _interpolationTimer = 0f;
            _previousPosition = _currentPosition;
            _previousRotation = _currentRotation;
        }

        _interpolationTarget.position = Vector3.Lerp(_previousPosition, _currentPosition, interpolationValue);
        _interpolationTarget.rotation = Quaternion.Slerp(_previousRotation, _currentRotation, interpolationValue);
    }
}
