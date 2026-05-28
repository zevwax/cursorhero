using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace ZevWaxGames.CursorHero
{
    public class Tabby : MonoBehaviour
    {
        public static Tabby Instance { get; private set; }
        private Sprite[] blinkSprites;
        private Sprite[] walkSprites;
        private float frameRate = 1/8f;
        private float walkDuration = 4f;

        private Image uiImage;
        private Vector2 offScreenPosition;
        private Vector2 inScreenPosition;
        private bool isWalkingLooping;
        
        private Coroutine waitNGo;
        
        private bool messageIsUpdated = false;
        private int currActionIndex = 0;
        
        private GameObject messageBox;
        private GameObject messageText;
        /*
         * Actions:
         * 0 - off screen
         * on blue face finished
         * 1 - greetings, clock info
         * on LMB pressed
         * 2 - off screen
         * on 1st recycle bin opened
         * 3 - projectile info; copy a glyph
         * on glyph copied
         * 4 - push the switch pc shortcut
         * on switch pc shortcut pushed
         * 5 - off screen
         */
        public void SwitchActionTo(int index)
        {
            if (currActionIndex == index - 1)
                currActionIndex = index;
        }
        private void Awake() => Instance = this;
        private void OnEnable() => EventHolder.OnFadingInToPCStarted += Die;
        private void OnDisable() => EventHolder.OnFadingInToPCStarted -= Die;
        public void Init()
        {
            offScreenPosition = new Vector2(8.5f, -3.5f);
            inScreenPosition = new Vector2(5.5f, -3.5f);
            
            transform.position = offScreenPosition;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            
            uiImage = transform.GetChild(0).GetComponent<Image>();

            Sprite[] allSprites = Resources.LoadAll<Sprite>("My/My/Sprites/tabby");
            
            System.Array.Sort(allSprites, (a, b) => {
                int indexA = int.Parse(a.name.Substring(a.name.LastIndexOf('_') + 1));
                int indexB = int.Parse(b.name.Substring(b.name.LastIndexOf('_') + 1));
                return indexA.CompareTo(indexB);
            });

            blinkSprites = new Sprite[8];
            walkSprites = new Sprite[8];

            System.Array.Copy(allSprites, 0, blinkSprites, 0, 8);
            System.Array.Copy(allSprites, 8, walkSprites, 0, 8);
            
            messageBox = transform.GetChild(1).gameObject;
            messageText = messageBox.transform.GetChild(0).gameObject;
            
            StartCoroutine(PlayAnimation());
        }
        /*yield return StartCoroutine(Say(
            string.Format(
                "Hey there!! I’m Tabby, your personal assistant.\n\n" +
                "Let me onboard you..\n\n" +
                "Look at the clipboard <color=#FF00FF>at the bottom of the screen</color>. " +
                "Your glyph is your projectile. Let's improve its stats!!\n\n" +
                "Try <color=#FF00FF>holding down the Left Mouse Button " +
                "to select the glyph drawn on the keyboard key you found in the recycle bin</color>.."),
            195f));*/
        public IEnumerator PlayAnimation()
        {
            yield return new WaitUntil(() => currActionIndex == 1);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(WalkIn());
            yield return StartCoroutine(Say(
                "Hey there!! I’m Tabby, your personal assistant..",
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(2); SwitchActionTo(3);
            yield return StartCoroutine(Say(
                "Let me..",
                46f));
            yield return new WaitForSeconds(0.33f);
            yield return StartCoroutine(MainCharacter.Instance.DoGlitch());
            yield return new WaitForSeconds(0.75f);
            yield return StartCoroutine(Say(
                "Uugh..",
                46f));
            yield return new WaitForSeconds(0.75f);
            yield return StartCoroutine(Say(
                "It seems we don't have much time.. Agents are trying to kill you!!",
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(4);
            yield return StartCoroutine(Say(
                "They'll retreat once the timer runs out.",
                65f)); var ra = Spawner.NewRedArrow(new Vector2(6.51f, -3.45f), Vector2.down);
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(5);
            Destroy(ra);
            yield return StartCoroutine(Say(
                "Please, don't get hurt..",
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(6);
            yield return StartCoroutine(WalkOut());
            BlueFace.Instance.StartAnim();
            
            yield return new WaitUntil(() => currActionIndex == 7);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(WalkIn());
            yield return StartCoroutine(Say(
                "Whew, you did it! Now, time for loot!",
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(8);
            var keyPos = new Vector2(-2, -1);//Object.FindAnyObjectByType<Projectile>().transform.position;
            var selectionPos = new Vector3(keyPos.x, keyPos.y, 0f);
            yield return StartCoroutine(Say(
                "Try holding down the Left Mouse Button to select the glyph you found in the recycle bin",
                65f));
            var selection = Spawner.NewTabbyPointer(selectionPos);
            
            yield return new WaitUntil(() => currActionIndex == 9);
            Destroy(selection);
            var shortcutPos = Object.FindAnyObjectByType<ButtonSwitchPC>().transform.position;
            var arrow2Pos = new Vector3(shortcutPos.x, shortcutPos.y + 1f, 0f);
            yield return StartCoroutine(Say(
                "Good job, boss!! Hit the shortcut when you finish looting, okay??",
                65f)); var ra2 = Spawner.NewRedArrow(arrow2Pos, Vector2.down);
            yield return new WaitForSeconds(1f);
            G.Instance.net.GetComponent<Button>().EnableButton();
            
            yield return new WaitUntil(() => currActionIndex == 10);
            Destroy(ra2);
            yield return StartCoroutine(WalkOut());
        }
        private IEnumerator WalkIn()
        {
            uiImage.transform.localRotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(LoopWalkAnimation(true));
            yield return transform.DOMove(inScreenPosition, walkDuration).SetEase(Ease.Linear).WaitForCompletion();
            StartCoroutine(LoopWalkAnimation(false));
            yield return StartCoroutine(Blink());
        }
        private IEnumerator WalkOut()
        {
            yield return StartCoroutine(MessageScaleDown());
            uiImage.transform.localRotation = Quaternion.Euler(0, 180, 0);
            StartCoroutine(LoopWalkAnimation(true));
            yield return transform.DOMove(offScreenPosition, walkDuration).SetEase(Ease.Linear).WaitForCompletion();
            StartCoroutine(LoopWalkAnimation(false));
        }
        private IEnumerator Blink()
        {
            for (int i = 0; i < blinkSprites.Length; i++)
            {
                uiImage.sprite = blinkSprites[i];
                yield return new WaitForSeconds(frameRate);
            }
        }

        private IEnumerator LoopWalkAnimation(bool start)
        {
            isWalkingLooping = start;
            int currentFrame = 0;

            while (isWalkingLooping)
            {
                uiImage.sprite = walkSprites[currentFrame];
                currentFrame = (currentFrame + 1) % walkSprites.Length;
                yield return new WaitForSeconds(frameRate);
            }
        }
        private IEnumerator Say(string words, float textboxHeight)
        {
            if (!messageBox.activeInHierarchy)
            {
                messageBox.SetActive(true);
                var messageBoxRt = messageBox.GetComponent<RectTransform>();
                var messageTextComp = messageText.GetComponent<TextMeshProUGUI>();
                var duration = 0.3f;
                yield return messageBoxRt.DOSizeDelta(new Vector2(130f, textboxHeight), duration).SetEase(Ease.OutBack).WaitForCompletion();
                messageTextComp.text = words;
                messageText.SetActive(true);
            }
            else
            {
                yield return StartCoroutine(MessageScaleDown());
                yield return StartCoroutine(Say(words, textboxHeight));
            }
        }
        private IEnumerator MessageScaleDown()
        {
            messageText.SetActive(false);
            var messageBoxRt = messageBox.GetComponent<RectTransform>();
            var duration = 0.3f;
            yield return messageBoxRt.DOSizeDelta(new Vector2(130f, 21f), duration).SetEase(Ease.OutBack).WaitForCompletion();
            messageBox.SetActive(false);
        }
        public IEnumerator UpdateMessage()
        {
            if (messageIsUpdated) yield break;
            yield return StartCoroutine(MessageScaleDown());
            var msg1 = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            //msg1.text = ;
            yield return StartCoroutine(Say(
                string.Format(
                    "Good job, boss!!\n\n" +
                    "<color=#FF00FF>Hit the 'Switch PC' shortcut</color> when you're done looting, okay??"),
                65f)
            );
            yield return new WaitForSeconds(2f);
            G.Instance.net.EnableButton();
            messageIsUpdated = true;
        }
        private void Die() => Destroy(gameObject);
    }
}