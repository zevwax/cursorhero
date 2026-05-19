using System;
using TMPro;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class TooltipHolder : MonoBehaviour
    {
        private BoxCollider2D myCollider;
        private bool isHeld;
        private static bool mainCharIsReadingATooltip;
        private string tooltipText = "Default Tooltip";
        private bool isHovered;
        private GameObject _currentTooltip;
        public void SetText(string text)
        {
            tooltipText = text;
            if (_currentTooltip != null)
            {
                var tooltipTextHolder = _currentTooltip.transform.GetChild(1);
                if (tooltipTextHolder.name != "Tooltip")
                    throw new Exception("Hierarchy mismatch");
                var tooltipTextComp = tooltipTextHolder.GetComponent<TextMeshProUGUI>();
                if (tooltipTextComp.name == null)
                    throw new Exception("An object with name 'Tooltip' does not have a 'TextMeshProUGUI' component");
                tooltipTextComp.text = tooltipText;
            }
        }
        public void Init()
        {
            myCollider = GetComponent<BoxCollider2D>();
        }
        private void Update()
        {
            var collision = myCollider.IsTouching(MainCharacter.Instance.gameObject.GetComponent<BoxCollider2D>());
            
            //OnEnter
            if (collision && !mainCharIsReadingATooltip)
            {
                mainCharIsReadingATooltip = true;
                isHovered = true;
                _currentTooltip = Spawner.NewTooltip(tooltipText);
            }
            
            //OnExit
            if (!collision && isHovered)
            {
                mainCharIsReadingATooltip = false;
                isHovered = false;
                KillTooltip();
            }
        }
        private void OnDestroy()
        {
            if (mainCharIsReadingATooltip && isHovered)
                mainCharIsReadingATooltip = false;
            KillTooltip();
        }
        private void KillTooltip()
        {
            if (_currentTooltip != null)
                _currentTooltip.GetComponent<Tooltip>().Die();
        }
    }
}