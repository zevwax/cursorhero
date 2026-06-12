using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class HangTheScreenAtBoss : MonoBehaviour
    {
        public static HangTheScreenAtBoss Instance { get; private set; }
        private void Hang(float angle) => transform.rotation = Quaternion.Euler(0f, 0f, angle);
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += HangWhat;
            EventHolder.OnBIOSStarted += HangOff;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= HangWhat;
            EventHolder.OnBIOSStarted -= HangOff;
        }
        private void Awake() => Instance = this;
        private void HangOn() => Hang(7);
        private void HangOff() => Hang(0);
        private void HangWhat()
        {
            if (WaveManager.Instance.CurrentWaveIndex == 12)
                HangOn();
            else
                HangOff();
        }
    }
}