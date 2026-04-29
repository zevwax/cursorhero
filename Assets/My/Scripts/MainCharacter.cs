using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class MainCharacter : Cursor
    {
        public static MainCharacter Instance { get; private set; }

        protected override void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("MainCharacter");
            
            base.Start();
            Instance = this;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}