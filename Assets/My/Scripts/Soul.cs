using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Soul : MonoBehaviour
    {
        private SpriteRenderer sr;
        private void OnEnable()
        {
            EventHolder.OnRunStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnRunStarted -= Die;
        }
        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            StartCoroutine(BlinkRoutine());
            Destroy(gameObject, 5f);
        }
        private IEnumerator BlinkRoutine()
        {
            while (true)
            {
                sr.DOFade(0f, 0.5f);
                yield return new WaitForSeconds(0.5f);
                sr.DOFade(1f, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}