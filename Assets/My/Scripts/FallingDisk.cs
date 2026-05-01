using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class FallingDisk : MonoBehaviour
    {
        private float fallSpeed;
        private float shrinkSpeed = 0.4f;
        private float rotationSpeed;
        private float destroyY = -7f;

        private SpriteRenderer sr;
        private Vector3 initialScale;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            
            float randomStartScale = Random.Range(1.5f, 2.5f);
            transform.localScale = Vector3.one * randomStartScale;
            initialScale = transform.localScale;

            fallSpeed = Random.Range(2f, 4f);
            rotationSpeed = Random.Range(-150f, 150f);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        private void Update()
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            float newScale = transform.localScale.x - (shrinkSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * Mathf.Max(newScale, 0);

            Color c = sr.color;
            c.a = transform.localScale.x / initialScale.x;
            sr.color = c;

            if (transform.position.y < destroyY || transform.localScale.x <= 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }
}