using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class Apple : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Die;
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}