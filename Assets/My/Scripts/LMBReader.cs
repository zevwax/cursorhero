using UnityEngine;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public class LMBReader : MonoBehaviour
    {
        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                EventHolder.OnRunStarted?.Invoke();
            }
        }
    }
}