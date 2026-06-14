using System;
namespace ZevWaxGames.CursorHero
{
    public static class EventHolder
    {
        public static Action OnRunStarted;
        public static Action OnRunFinished;
        public static Action OnFadingOutFromPCStarted;
        public static Action OnFadingInToPCStarted;
        public static Action OnPCStarted;
        public static Action OnPCFinished;
        public static Action OnYouWinStarted;
        public static Action OnYouWinFinished;
        public static Action OnBinStarted;
        public static Action OnBinFinished;
        public static Action OnBSODStarted;
        public static Action OnBIOSStarted;
        public static Action OnBIOSFinished;
        
        public static Action OnFadingInFinished;
        public static Action OnPCBtnPushed;
        public static Action OnFadingOutFinished;
    }
}