using UnityEngine;
using System.Collections;

namespace ZevWaxGames.CursorHero
{
    public class G : MonoBehaviour
    {
        public static G Instance { get; private set; }
        public Button bin;
        public Button net;
        private void Awake()
        {
            Instance = this;
        }
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
            var b1 = Spawner.NewBinButton(new Vector2(-(8f-0.75f), 4.5f-1f-0.75f));
            var b2 = Spawner.NewNetButton(new Vector2(-(8f-0.75f), 4.5f-1f-0.75f-1f-0.25f));
            bin = b1.GetComponent<Button>();
            net = b2.GetComponent<Button>();
            bin.DisableButton();
            net.DisableButton();
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