using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    public class Glitch : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Die());
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }
    }
}