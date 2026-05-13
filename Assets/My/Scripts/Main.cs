using UnityEngine;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            //Screen.SetResolution(300, 900, FullScreenMode.Windowed);
            //Screen.SetResolution(320, 180, FullScreenMode.FullScreenWindow);
            //Screen.fullScreen = true;
            //PlayerPrefs.DeleteAll();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            
            Spawner.NewEntity<Wall>(new Vector2(-8.5f, 0), "Left Wall", "vertical_wall", null, true, "Wall", true);
            Spawner.NewEntity<Wall>(new Vector2(8.5f, 0), "Right Wall", "vertical_wall", null, true, "Wall", true);
            Spawner.NewEntity<Wall>(new Vector2(0, -5f), "Bottom Wall", "horizontal_wall", null, true, "Wall", true);
            Spawner.NewEntity<Wall>(new Vector2(0, 5f), "Top Wall", "horizontal_wall", null, true, "Wall", true);
            Spawner.NewEntity<MainCharacter>(new Vector2(0, 0), "MainCharacter", "glove", "Pointer", true, "MainCharacter", false);
            
            //StartCoroutine(CreateABottleWDelay());
            
            UIManager.Instance.ShowStartGameWindow();
            Spawner.NewBinButton(new Vector2(-8f+0.75f, 4.5f-1f-0.75f));
        }

        private IEnumerator CreateABottleWDelay()
        {
            yield return new WaitForSeconds(2);
            Spawner.NewBottle(new Vector2(-3, -2));
            Spawner.NewBottle(new Vector2(3, -2));
            Spawner.NewBottle(new Vector2(-3, 2));
            Spawner.NewBottle(new Vector2(3, 2));
        }
    }
}