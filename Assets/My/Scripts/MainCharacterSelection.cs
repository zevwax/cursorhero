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
                        Spawner.NewDamageNumbers(transform.position, "Copied A.glyph");

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
                    Spawner.NewDamageNumbers(transform.position, "+2 HP");
                    Destroy(o);
                }
                Tabby.Instance.SwitchActionTo(9);
            }
            MainCharacter.Instance.SetGlove(gameObject);
        }
    }
}