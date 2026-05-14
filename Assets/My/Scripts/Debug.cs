using System;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class EventLogger : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHolder.OnRunStarted += LogRunStarted;
            EventHolder.OnPCStarted += LogPCStarted;
            EventHolder.OnChoosingStarted += LogChoosingStarted;
            EventHolder.OnChoosingFinished += LogChoosingFinished;
            EventHolder.OnPCFinished += LogPCFinished;
            EventHolder.OnRunFinished += LogRunFinished;
        }

        private void OnDisable()
        {
            EventHolder.OnRunStarted -= LogRunStarted;
            EventHolder.OnPCStarted -= LogPCStarted;
            EventHolder.OnChoosingStarted -= LogChoosingStarted;
            EventHolder.OnChoosingFinished -= LogChoosingFinished;
            EventHolder.OnPCFinished -= LogPCFinished;
            EventHolder.OnRunFinished -= LogRunFinished;
        }

        private void LogRunStarted() => Log(nameof(EventHolder.OnRunStarted));
        private void LogPCStarted() => Log(nameof(EventHolder.OnPCStarted));
        private void LogChoosingStarted() => Log(nameof(EventHolder.OnChoosingStarted));
        private void LogChoosingFinished() => Log(nameof(EventHolder.OnChoosingFinished));
        private void LogPCFinished() => Log(nameof(EventHolder.OnPCFinished));
        private void LogRunFinished() => Log(nameof(EventHolder.OnRunFinished));

        private void Log(string eventName)
        {
            Debug.Log($"[{DateTime.Now:HH:mm:ss.fff}] {eventName}");
        }
    }
}