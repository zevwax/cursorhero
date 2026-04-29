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
            PlayerPrefs.DeleteAll();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            
            PrefabCreator.NewBackground();
            PrefabCreator.NewWall(Wall.WallType.Left);
            PrefabCreator.NewWall(Wall.WallType.Right);
            PrefabCreator.NewWall(Wall.WallType.Top);
            PrefabCreator.NewWall(Wall.WallType.Bottom);
            PrefabCreator.NewCursorHero();
        }
    }
}