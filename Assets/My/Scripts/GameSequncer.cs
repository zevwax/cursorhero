using UnityEngine;
using ZevWaxGames.CursorHero;

namespace ZevWaxGames.CursorHero
{
    public class GameSequncer : MonoBehaviour
    {
        public bool IsBIOS => isBIOS;
        private bool isBIOS = false;
        public bool IsRun => isRun;
        private bool isRun = false;
        public static GameSequncer Instance { get; private set; }
        private void Awake() => Instance = this;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += HandleRunStarted;
            EventHolder.OnRunFinished += HandleRunFinished;
            EventHolder.OnBIOSStarted += HandleBiosStarted;
            EventHolder.OnBIOSFinished += HandleBiosFinished;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= HandleRunStarted;
            EventHolder.OnRunFinished -= HandleRunFinished;
            EventHolder.OnBIOSStarted -= HandleBiosStarted;
            EventHolder.OnBIOSFinished -= HandleBiosFinished;
        }
        private void HandleRunStarted() => SetRunState(true);
        private void HandleRunFinished() => SetRunState(false);
        private void SetRunState(bool value) => isRun = value;
        private void HandleBiosStarted() => SetBiosState(true);
        private void HandleBiosFinished() => SetBiosState(false);
        private void SetBiosState(bool value) => isBIOS = value;
    }
}