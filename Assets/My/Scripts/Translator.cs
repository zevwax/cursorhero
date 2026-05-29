using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace ZevWaxGames.CursorHero
{
    public enum Language
    {
        English,
        Russian
    }
    public enum Token
    {
        Greetings,
        LetMe,
        Uurgh,
        WeDontHaveTime,
        TheyllRetreat,
        DontGetHurt,
        YouDidIt,
        Select,
        PushThePCShortcut
    }
    public class Translator : MonoBehaviour
    {
        public static Translator Instance { get; private set; }
        private Dictionary<Language, Dictionary<Token, string>> _localizationDb;
        public Language CurrentLanguage { get; private set; } = Language.English;
        public void TranslateTo(Language targetLanguage)
        {
            CurrentLanguage = targetLanguage;
        }
        public string Get(Token token)
        {
            if (_localizationDb.TryGetValue(CurrentLanguage, out var langDict))
            {
                if (langDict.TryGetValue(token, out var translation))
                {
                    return translation;
                }
            }
            return $"[{token} Missing]";
        }
        public void AddTranslation(Language language, Token token, string text)
        {
            if (!_localizationDb.ContainsKey(language))
            {
                _localizationDb[language] = new Dictionary<Token, string>();
            }
            _localizationDb[language][token] = text;
        }
        private void SeedInitialData()
        {
            AddTranslation(Language.English, Token.Greetings, "Hey there!! I’m Zipporah, your personal assistant..");
            AddTranslation(Language.English, Token.LetMe, "Let me..");
            AddTranslation(Language.English, Token.Uurgh, "Uugh..");
            AddTranslation(Language.English, Token.WeDontHaveTime, "We don't have much time, <color=#FF0000>agents</color> are on their way to kill you!!");
            AddTranslation(Language.English, Token.TheyllRetreat, "They'll retreat once the timer runs out.");
            AddTranslation(Language.English, Token.DontGetHurt, "Please, don't get hurt..");
            AddTranslation(Language.English, Token.YouDidIt, "Whew, you did it!! Let me explain glyphs..");
            AddTranslation(Language.English, Token.Select, "A glyph is a projectile. The glyph at the bottom of the screen is your equipped projectile. Hold the LMB and select the new glyph, <color=#FF00FF>like this:</color>");
            AddTranslation(Language.English, Token.PushThePCShortcut, "Good job, boss!! Click the shortcut once you're finished here, okay??");

            AddTranslation(Language.Russian, Token.Greetings, "Приветик!! Я Сепфора, твоя ассистентка..");
            AddTranslation(Language.Russian, Token.LetMe, "Позволь мне..");
            AddTranslation(Language.Russian, Token.Uurgh, "...");
            AddTranslation(Language.Russian, Token.WeDontHaveTime, "У нас мало времени, <color=#FF0000>агенты</color> уже идут за тобой!!");
            AddTranslation(Language.Russian, Token.TheyllRetreat, "Они отступят, как только истечёт таймер.");
            AddTranslation(Language.Russian, Token.DontGetHurt, "Пожалуйста, будь осторожен..");
            AddTranslation(Language.Russian, Token.YouDidIt, "Фуух! Ты сделал это!! Позволь мне рассказать о глифах..");
            AddTranslation(Language.Russian, Token.Select, "Глиф - это снаряд. Глиф внизу экрана - это тот снаряд, которым ты стреляешь. Зажми ЛКМ и выдели новый глиф, <color=#FF00FF>вот так:</color>");
            AddTranslation(Language.Russian, Token.PushThePCShortcut, "Отличная работа, босс!! Кликни на ярлык, когда закончишь здесь, оки??");
        }
        private void Awake() => Instance = this;
        private void Start()
        {
            _localizationDb = new Dictionary<Language, Dictionary<Token, string>>();
            SeedInitialData();
        }
        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
                TranslateTo(Language.Russian);
            
            if (Keyboard.current.eKey.wasPressedThisFrame)
                TranslateTo(Language.English);
        }
    }
}