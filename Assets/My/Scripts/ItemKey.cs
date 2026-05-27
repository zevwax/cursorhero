using UnityEngine;
namespace ZevWaxGames.CursorHero
{
    public class ItemKey : MonoBehaviour
    {
        public Glyph Glyph;
        
        [Header("Settings")]
        [SerializeField] private float drag = 5f;
        private Rigidbody2D rb;

        private void OnEnable() => EventHolder.OnFadingInToPCStarted += Die;
        private void OnDisable() => EventHolder.OnFadingInToPCStarted -= Die;
        public void Init()
        {
            Glyph = new Glyph(true, Random.Range(1f, 3f), Random.Range(1f, 3f), false, false, Guns.Library[GunName.Yellow].Glyph.Speed);
            SetSize(Glyph.Size);
            var tooltip = string.Format("A.glyph\nWeight: {0:F1} / Size: {1:F1}", Glyph.Weight, Glyph.Size);
            GetComponent<TooltipHolder>().SetText(tooltip);
            
            rb = GetComponent<Rigidbody2D>();
            rb.linearDamping = drag;
            rb.angularDamping = drag;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        private void Die() => Destroy(gameObject);
        public void SetSize(float s) => GetComponent<WorldSpaceCanvasRealtimeScaler>().mult = s/3f;
    }
}