using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class Apple : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHolder.OnLimbo += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnLimbo -= Die;
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}