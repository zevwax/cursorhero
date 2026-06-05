using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ConditionManager : MonoBehaviour
    {
        public static ConditionManager Instance { get; private set; }
        public enum Condition
        {
            Null,
            Fighting,
            Choosing,
            AfterFightChilling,
            BinLooting
        }
        public Condition prevCondition;
        public Condition currCondition;
        private void OnEnable()
        {
            EventHolder.OnPCStarted += HandleOnPCStarted;
            EventHolder.OnYouWinStarted += HandleOnChoosingStarted;
            EventHolder.OnYouWinFinished += HandleOnChoosingFinished;
            EventHolder.OnPCFinished += HandleOnPCFinished;
            EventHolder.OnBinStarted += HandleOnBinStarted;
        }
        private void OnDisable()
        {
            EventHolder.OnPCStarted -= HandleOnPCStarted;
            EventHolder.OnYouWinStarted -= HandleOnChoosingStarted;
            EventHolder.OnYouWinFinished -= HandleOnChoosingFinished;
            EventHolder.OnPCFinished -= HandleOnPCFinished;
            EventHolder.OnBinStarted -= HandleOnBinStarted;
        }
        private void Awake() => Instance = this;
        private void HandleOnPCStarted()
        {
            currCondition = Condition.Fighting;
        }
        private void HandleOnChoosingStarted()
        {
            prevCondition = currCondition;
            currCondition = Condition.Choosing;
        }
        private void HandleOnChoosingFinished()
        {
            currCondition = prevCondition;
        }
        private void HandleOnPCFinished()
        {
            currCondition = Condition.AfterFightChilling;
        }
        private void HandleOnBinStarted()
        {
            currCondition = Condition.BinLooting;
        }
    }
}