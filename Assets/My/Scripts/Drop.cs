using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Drop : MonoBehaviour
    {
        [SerializeField] private float attractionRadius = 3f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float slideForce = 1.5f;
        [SerializeField] private float drag = 5f;
        
        private Rigidbody2D rb;
        private CanvasGroup cg;
        private BoxCollider2D col;

        private void OnEnable()
        {
            EventHolder.OnBIOSStarted += Refresh;
            EventHolder.OnRunFinished += Disable;
            EventHolder.OnYouWinStarted += Disable;
            EventHolder.OnYouWinFinished += Enable;
        }

        private void OnDisable()
        {
            EventHolder.OnBIOSStarted -= Refresh;
            EventHolder.OnRunFinished -= Disable;
            EventHolder.OnYouWinStarted -= Disable;
            EventHolder.OnYouWinFinished -= Enable;
        }
        
        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Disk");
            rb = GetComponent<Rigidbody2D>();
            cg = GetComponent<CanvasGroup>();
            col = GetComponent<BoxCollider2D>();

            rb.linearDamping = drag;

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDir * slideForce, ForceMode2D.Impulse);

            StartCoroutine(BlinkRoutine());
        }

        private IEnumerator BlinkRoutine()
        {
            cg.DOFade(0.2f, 0.2f).SetLoops(10, LoopType.Yoyo).OnComplete(() => {
                cg.DOFade(1f, 0.1f);
            });
            yield return null;
        }

        private void FixedUpdate()
        {
            if (MainCharacter.Instance == null || !col.enabled) return;

            float distance = Vector2.Distance(transform.position, MainCharacter.Instance.transform.position);
            
            if (distance <= attractionRadius)
            {
                Vector2 direction = (MainCharacter.Instance.transform.position - transform.position).normalized;
                rb.AddForce(direction * acceleration);
            }
        }


        private void Enable() => col.enabled = true;
        private void Disable() => col.enabled = false;

        private void Refresh()
        {
            Die();
        }
        protected void Die()
        {
            Destroy(gameObject);
        }
        
        protected virtual void OnTriggerEnter2D(Collider2D collision) => throw new System.NotImplementedException();
        protected virtual float Collect() => throw new System.NotImplementedException();
    }
}