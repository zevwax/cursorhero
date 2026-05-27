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
                if (o.GetComponent<ItemKey>() != null)
                {
                    var glyph = o.GetComponent<ItemKey>().Glyph;
                    MainCharacter.Instance.weight = glyph.Weight;
                    MainCharacter.Instance.size = glyph.Size;
                    Guns.Library[GunName.Yellow].Glyph = glyph;
                    ClipboardTextbox.Instance.UpdateContents();
                    Spawner.NewDamageNumbers(transform.position, "Copied A.glyph");
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