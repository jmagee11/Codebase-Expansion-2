using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace util
{
    public class UsesControllerChecker : MonoBehaviour
    {
        public PlayerInput input;
        public UnityEvent<bool> usesMouse;

        public void OnChanged(PlayerInput playerInput)
        {
            Debug.Log("Changed Scheme: " + playerInput.currentControlScheme);
            usesMouse.Invoke(playerInput.currentControlScheme == "Mouse");
        }
    }
}