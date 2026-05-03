using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public abstract class Upgrade : Button
    {
        public override void ButtonAction()
        {
            EventHolder.OnChoosingFinished?.Invoke();
        }
    }
}