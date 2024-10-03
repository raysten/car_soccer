using Fusion;
using Network;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField]
        private float _forwardSpeed = 10f;

        [SerializeField]
        private float _reverseSpeed = 1f;

        [SerializeField]
        private float _rotationSpeed = 5f;

        [SerializeField]
        private float _minimumVelocityToRotate = 0.01f;

        [SerializeField]
        private Rigidbody _rigidbody;

        public override void FixedUpdateNetwork()
        {
            SimulateOnServer();
        }

        private void SimulateOnServer()
        {
            if (GetInput(out NetworkInputData data) && HasStateAuthority)
            {
                MovePlayer(data);
            }
        }

        private void MovePlayer(NetworkInputData inputData)
        {
            var moveInput = inputData.moveInput;

            MoveVertically(moveInput);
            Rotate(inputData.steerInput);
        }

        private void MoveVertically(float moveInput)
        {
            if (moveInput > 0)
            {
                _rigidbody.AddForce(transform.forward * moveInput * _forwardSpeed, ForceMode.Acceleration);
            }
            else if (moveInput < 0)
            {
                _rigidbody.AddForce(-transform.forward * _reverseSpeed, ForceMode.Acceleration);
            }
        }

        private void Rotate(float steerInput)
        {
            if (_rigidbody.velocity.magnitude >= _minimumVelocityToRotate)
            {
                var verticalDirectionFactor = CalculateVerticalDirectionFactor();
                var rotationValue = steerInput * verticalDirectionFactor * _rotationSpeed * Runner.DeltaTime;
                var rotationAroundYAxis = Quaternion.Euler(new Vector3(0f, rotationValue, 0f));

                _rigidbody.MoveRotation(_rigidbody.rotation * rotationAroundYAxis);
            }
        }

        private float CalculateVerticalDirectionFactor()
        {
            var dotProduct = Vector3.Dot(_rigidbody.velocity, transform.forward);

            return dotProduct > 0 ? 1f : dotProduct == 0 ? 0f : -1f;
        }
    }
}
