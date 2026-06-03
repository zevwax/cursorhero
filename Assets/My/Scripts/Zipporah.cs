using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
namespace ZevWaxGames.CursorHero
{
    public class Zipporah : MonoBehaviour
    {
        public static Zipporah Instance { get; private set; }
        private Sprite hi;
        private Sprite mouth0_hand0;
        private Sprite mouth0_hand1;
        private Sprite mouth1_hand0;
        private Sprite mouth1_hand1;
        private float transitionDuration = 1f;

        private Image uiImage;
        private Vector2 offScreenPosition;
        private Vector2 inScreenPosition;
        private bool isWalkingLooping;
        
        private Coroutine waitNGo;
        
        private bool messageIsUpdated = false;
        private int currActionIndex = 0;
        
        private GameObject messageBox;
        private GameObject messageText;
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
            offScreenPosition = new Vector2(16f, 0);
            inScreenPosition = new Vector2(0, 0);
            
            transform.position = offScreenPosition;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            
            uiImage = transform.GetChild(0).GetComponent<Image>();
            
            var i = 0;
            hi = NextSprite(ref i);
            mouth0_hand0 = NextSprite(ref i);
            mouth0_hand1 = NextSprite(ref i);
            mouth1_hand0 = NextSprite(ref i);
            mouth1_hand1 = NextSprite(ref i);
            
            messageBox = transform.GetChild(1).gameObject;
            messageText = messageBox.transform.GetChild(0).gameObject;
            
            StartCoroutine(PlayAnimation());
        }
        private Sprite NextSprite(ref int index)
        {
            var spriteSheetName = "zipporah";
            var path = "My/My/Sprites/" + spriteSheetName;
            var allSprites = Resources.LoadAll<Sprite>(path);
            var targetName = spriteSheetName + "_" + index++;
            var targetSprite = allSprites.FirstOrDefault(s => s.name == targetName);
            return targetSprite;
        }
        public IEnumerator PlayAnimation()
        {
            yield return new WaitUntil(() => currActionIndex == 1);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(WalkIn());
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.Greetings),
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(2); SwitchActionTo(3);
            uiImage.sprite = mouth1_hand0;
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.LetMe),
                46f));
            yield return new WaitForSeconds(0.33f);
            yield return StartCoroutine(MainCharacter.Instance.DoGlitch());
            yield return new WaitForSeconds(0.75f);
            uiImage.sprite = mouth0_hand0;
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.Uurgh),
                46f));
            yield return new WaitForSeconds(0.75f);
            uiImage.sprite = mouth1_hand0;
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.WeDontHaveTime),
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(4);
            uiImage.sprite = mouth1_hand1;
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.TheyllRetreat),
                65f)); var ra = Spawner.NewRedArrow(new Vector2(7.3f, -4.03f), Vector2.left);
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(5);
            uiImage.sprite = mouth0_hand0;
            Destroy(ra);
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.DontGetHurt),
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(6);
            yield return StartCoroutine(WalkOut());
            BlueFace.Instance.StartAnim();
            
            yield return new WaitUntil(() => currActionIndex == 7);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(WalkIn());
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.YouDidIt),
                65f));
            
            yield return new WaitUntil(() => Mouse.current.leftButton.isPressed);
            SwitchActionTo(8);
            uiImage.sprite = mouth1_hand1;
            var keyPos = new Vector2(-2, -1);//Object.FindAnyObjectByType<Projectile>().transform.position;
            var selectionPos = new Vector3(keyPos.x, keyPos.y, 0f);
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.Select),
                65f));
            var selection = Spawner.NewTabbyPointer(selectionPos);
            
            yield return new WaitUntil(() => currActionIndex == 9);
            uiImage.sprite = mouth0_hand1;
            Destroy(selection);
            var shortcutPos = Object.FindAnyObjectByType<ButtonSwitchPC>().transform.position;
            var arrow2Pos = new Vector3(shortcutPos.x, shortcutPos.y + 1f, 0f);
            yield return StartCoroutine(Say(
                Translator.Instance.Get(Token.PushThePCShortcut),
                65f)); var ra2 = Spawner.NewRedArrow(arrow2Pos, Vector2.down);
            yield return new WaitForSeconds(1f);
            G.Instance.net.GetComponent<Button>().EnableButton();
            
            yield return new WaitUntil(() => currActionIndex == 10);
            Destroy(ra2);
            yield return StartCoroutine(WalkOut());
        }
        private IEnumerator WalkIn()
        {
            uiImage.sprite = hi;
            yield return transform.DOMove(inScreenPosition, transitionDuration).SetEase(Ease.OutQuad).WaitForCompletion();
        }
        private IEnumerator WalkOut()
        {
            yield return transform.DOMove(offScreenPosition, transitionDuration).SetEase(Ease.OutQuad).WaitForCompletion();
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
        private void Die() => Destroy(gameObject);
    }
}