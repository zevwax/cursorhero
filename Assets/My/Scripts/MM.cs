using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class MM : Drop
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("MainCharacter"))
            {
                DJ.PlayDisk();
                MainCharacter.Instance.GainMMs();
                Spawner.NewMMTweenEntity();
                Die();
            }
        }
    }
}