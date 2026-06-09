using UnityEngine;
using ZevWaxGames.CursorHero;

namespace ZevWaxGames.CursorHero
{
    public class GameSequncer : MonoBehaviour
    {
        public bool IsBIOS => isBIOS;
        private bool isBIOS = false;
        public static GameSequncer Instance { get; private set; }
        private void Awake() => Instance = this;
        private void OnEnable()
        {
            EventHolder.OnBIOSStarted += HandleBiosStarted;
            EventHolder.OnBIOSFinished += HandleBiosFinished;
        }
        private void OnDisable()
        {
            EventHolder.OnBIOSStarted += HandleBiosStarted;
            EventHolder.OnBIOSFinished += HandleBiosFinished;
        }
        private void HandleBiosStarted() => SetBiosState(true);
        private void HandleBiosFinished() => SetBiosState(false);
        private void SetBiosState(bool value) => isBIOS = value;
    }
}