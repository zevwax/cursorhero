using DG.Tweening;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class MainCharacterSelection : Selection
    {
        protected override bool CanStartSelection()
        {
            return MainCharacter.Instance.IsAbleForReskinBy(gameObject);
        }

        protected override Vector3 GetPointerPos()
        {
            return MainCharacter.Instance.transform.position + new Vector3(0, 0.1f, 0);
        }

        protected override void OnStartSelecting()
        {
            MainCharacter.Instance.SetCross(gameObject);
        }

        protected override void OnFinishSelecting()
        {
            var o = GetSelectedObject();
            if (o != null)
            {
                if (o == MainCharacter.Instance.ClipboardGlyph)
                {
                    var clipboardPos = new Vector2(-2f, -4f);
                    o.transform.DOMove(clipboardPos, 1);
                }
                if (o.GetComponent<Projectile>() != null)
                {
                    if (o != MainCharacter.Instance.ClipboardGlyph)
                    {
                        var glyph = o.GetComponent<Projectile>();
                        MainCharacter.Instance.weight = glyph.Weight;
                        MainCharacter.Instance.size = glyph.Size;
                        Guns.Library[GunName.Yellow].Glyph = new Glyph
                        (
                            true,
                            glyph.Weight,
                            glyph.Size,
                            glyph.IsBouncyV,
                            glyph.IsPiercingV,
                            glyph.Speed
                        );
                        ClipboardTextbox.Instance.UpdateContents();
                        var currChar = glyph.CurrChar();
                        Spawner.NewPopUpText(transform.position, string.Format("Copied {0}.glyph", currChar), new Color(0, 1, 0, 1), 1f);

                        var clipboardObj = MainCharacter.Instance.ClipboardGlyph;
                        var keyObj = o;
                        var clipboardPos = new Vector2(-2f, -4f);
                        var keyPos = keyObj.transform.position;
                        clipboardObj.GetComponent<Projectile>().MakeItBeAKey();
                        keyObj.GetComponent<Projectile>().MakeItBeAClipboardItem();
                        clipboardObj.transform.DOMove(keyPos, 1);
                        keyObj.transform.DOMove(clipboardPos, 1);
                        keyObj.transform.DORotate(Vector3.zero, 1);
                    }
                }
                else if (o.GetComponent<ItemApple>() != null)
                {
                    MainCharacter.Instance.IncreaseCurrHPByValue(2);
                    Spawner.NewPopUpText(transform.position, "+2 HP", new Color(0, 1, 0, 1), 1f);
                    Destroy(o);
                }
                Zipporah.Instance.SwitchActionTo(9);
            }
            else
            {
                var rt = GetComponent<RectTransform>();
                if (rt.sizeDelta.x > 30 && rt.sizeDelta.y > 30)
                {
                    var clipboardObj = MainCharacter.Instance.ClipboardGlyph;
                    var targetPos = transform.position;
                    clipboardObj.GetComponent<Projectile>().MakeItBeAKey();
                    clipboardObj.transform.DOMove(targetPos, 1);
                    Spawner.NewPopUpText(transform.position, "Pasted", new Color(0, 1, 0, 1), 1f);
                }
            }
            MainCharacter.Instance.SetGlove(gameObject);
        }
    }
}