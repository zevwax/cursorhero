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
                    var w = o.GetComponent<ItemKey>().Weight;
                    var s = o.GetComponent<ItemKey>().Size;
                    MainCharacter.Instance.edge = w;
                    MainCharacter.Instance.size = s;
                    Guns.Library[GunName.Yellow].Weight = w;
                    Guns.Library[GunName.Yellow].Size = s;
                    ClipboardTextbox.Instance.UpdateContents();
                    Spawner.NewDamageNumbers(transform.position, "Copied A.glyph");
                }
                else if (o.GetComponent<ItemApple>() != null)
                {
                    MainCharacter.Instance.HP += 3;
                    Spawner.NewDamageNumbers(transform.position, "+3 HP");
                    Destroy(o);
                }
                Tabby.Instance.SwitchActionTo(9);
            }
            MainCharacter.Instance.SetGlove(gameObject);
        }
    }
}