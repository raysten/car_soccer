using System.Globalization;
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
        if (GetInput(out NetworkInputData data) && HasStateAuthority)
        {
            data.direction.Normalize();
            _rigidbody.velocity = _speed * data.direction;

            NetworkedPosition = _rigidbody.position;
            NetworkedRotation = _rigidbody.rotation;
        }
    }

    public override void Render()
    {
        base.Render();
        
        // for some reason rigidbody is not synced with server when on client with input authority, so I'm interpolating the view manually
        if (HasInputAuthority && HasStateAuthority == false)
        {
             DetectChanges();
            
             _interpolationTimer += Time.deltaTime;
             var interpolationValue = Mathf.Clamp01(_interpolationTimer / Runner.DeltaTime);
             
             if (interpolationValue >= 1f)
             {
                 _interpolationTimer = 0f;
                 _previousPosition = _currentPosition;
                 _previousRotation = _currentRotation;
             }
             
             _interpolationTarget.position =
                 Vector3.Lerp(_previousPosition, _currentPosition, interpolationValue);
             
             _interpolationTarget.rotation =
                 Quaternion.Slerp(_previousRotation, _currentRotation, interpolationValue);
             
             UiDebug.Instance.WriteText($"{interpolationValue}");
        }
    }

    private void DetectChanges()
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
}
