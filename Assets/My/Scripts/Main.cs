using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class Main : MonoBehaviour
    {
        void Start()
        {
            //Screen.SetResolution(300, 900, FullScreenMode.Windowed);
            //Screen.SetResolution(320, 180, FullScreenMode.FullScreenWindow);
            //Screen.fullScreen = true;
            //PlayerPrefs.DeleteAll();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            
            Spawner.NewWall(Wall.WallType.Left);
            Spawner.NewWall(Wall.WallType.Right);
            Spawner.NewWall(Wall.WallType.Top);
            Spawner.NewWall(Wall.WallType.Bottom);
            Spawner.NewMainCharacter(new Vector2(0, 0));
            
            Spawner.NewBottle(new Vector2(3, -2));
            
            UIManager.Instance.ShowStartGameWindow();
        }
    }
}