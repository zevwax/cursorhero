using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening; 
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class LayingPieceOfGlass : MonoBehaviour
    {
        [Header("Settings")]
        private float slideForce = 15f;
        private float torqueForce = 30f;
        private float drag = 5f;
        private Rigidbody2D rb;
        private void OnEnable()
        {
            EventHolder.OnFadingInToPCStarted += Die;
        }
        private void OnDisable()
        {
            EventHolder.OnFadingInToPCStarted -= Die;
        }
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            rb.linearDamping = drag;
            rb.angularDamping = drag; 
            rb.constraints = RigidbodyConstraints2D.None;
            
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDir * slideForce, ForceMode2D.Impulse);
            
            float randomTorque = Random.Range(-torqueForce, torqueForce);
            rb.AddTorque(randomTorque, ForceMode2D.Impulse);
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}