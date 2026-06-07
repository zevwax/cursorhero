using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class FloppyDisk : Drop
    {
        private const float initXP = 0.333f;
        private static float currXP = 0.333f;
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("MainCharacter"))
            {
                var pbar = ProgressBar.Instance;
                pbar.SetValue(pbar.Value + Collect());
                Die();
            }
        }
        protected override float Collect()
        {
            DJ.PlayDisk();
            Spawner.NewFloppyDiskTweenEntity();
            return currXP;
        }
        public static void Inflate() => currXP *= 0.75f;
        public static void Normalize() => currXP = 1f/20f;
        public static void HyperInflate() => currXP = 0;
    }
}