using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ItemKey : MonoBehaviour
    {
        public float Weight => weight;
        public float Size => size;
        private float weight;
        private float size;
        
        [Header("Settings")]
        [SerializeField] private float drag = 5f;
        private Rigidbody2D rb;

        private void OnEnable() => EventHolder.OnFadingInToPCStarted += Die;
        private void OnDisable() => EventHolder.OnFadingInToPCStarted -= Die;
        public void Init()
        {
            weight = Random.Range(1f, 3f);
            SetSize(Random.Range(1f, 3f));/*key.png\ncontains a glyph\n\n*/
            var tooltip = string.Format("A.glyph\nWeight: {0:F1} / Size: {1:F1}", weight, size);
            GetComponent<TooltipHolder>().SetText(tooltip);
            
            rb = GetComponent<Rigidbody2D>();
            rb.linearDamping = drag;
            rb.angularDamping = drag;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        private void Die() => Destroy(gameObject);
        public void SetSize(float s)
        {
            size = s;
            GetComponent<WorldSpaceCanvasRealtimeScaler>().mult = s/3f;
        }
    }
}