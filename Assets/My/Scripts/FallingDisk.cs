using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ZevWaxGames.CursorHero
{
    public class FallingDisk : MonoBehaviour
    {
        private Vector3 initialScale;
        private float fallSpeed;
        private float rotationSpeed;
        
        private const float safeSpace = 0.25f;
        private float posXMin;
        private float posXMax;
        private float posYMin;
        private float posYMax;
        private const float fakePosZMin = 1f;
        private const float fakePosZMax = 2f;
        
        private float scaleProgress;
        private float shrinkSpeed;
        private void Start()
        {
            posXMin = -8f;
            posXMax = 8f;
            posYMin = -4.5f + safeSpace;
            posYMax = 4f;
            Refresh();
        }
        private void Update()
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            
            scaleProgress += shrinkSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, scaleProgress);

            if (transform.position.y <= posYMin || transform.localScale.x <= 0.00001f)
                Die();
        }
        public void Refresh()
        {
            scaleProgress = 0f;
            shrinkSpeed = 0.2f;
            GetComponent<WorldSpaceCanvasOnInitScaler>().Scale();
            
            var fakePosZ = Random.Range(fakePosZMin, fakePosZMax);
            float randomX = Random.Range(posXMin, posXMax);
            transform.position = new Vector3(randomX, posYMax, 0);
            
            var distanceToFall = posYMax - posYMin;
            fallSpeed = 2.5f;
            var timeToReachBottom = distanceToFall / fallSpeed;
            shrinkSpeed = 1f / timeToReachBottom;
            
            fallSpeed /= fakePosZ;
            
            rotationSpeed = Random.Range(50f, 150f);
            if (Random.Range(0f, 1f) < 0.5f)
                rotationSpeed *= -1;
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            
            transform.localScale = new Vector3(transform.localScale.x / fakePosZ, transform.localScale.y / fakePosZ, 1f);
            initialScale = transform.localScale;
            
            gameObject.SetActive(true);
        }
        public void Die()
        {
            gameObject.SetActive(false);
        }
    }
}