using UnityEngine;

namespace Network
{
    public class InputSampler : IInputSampler
    {
        public NetworkInputData SampleInput()
        {
            return new NetworkInputData
                   {
                       moveInput = Input.GetAxis("Vertical"),
                       steerInput = Input.GetAxis("Horizontal")
                   };
        }
    }
}
