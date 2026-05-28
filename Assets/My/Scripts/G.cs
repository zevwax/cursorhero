using UnityEngine;
using System.Linq;
using System.Collections;
namespace ZevWaxGames.CursorHero
{
    public class G : MonoBehaviour // reminder: implement a game sequencer w/ async/await
    {
        public static G Instance { get; private set; }
        public Button bin;
        public Button net;
        private void Awake() => Instance = this;
        private void Start()
        {
            //Screen.SetResolution(300, 900, FullScreenMode.Windowed);
            //Screen.SetResolution(320, 180, FullScreenMode.FullScreenWindow);
            //Screen.fullScreen = true;
            //PlayerPrefs.DeleteAll();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            
            Spawner.NewEntity<Wall>(
                new Vector2(-9.5f, 0), "Left Projectile Wall", "SizeHolders/projectile_vertical_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(9.5f, 0), "Right Projectile Wall", "SizeHolders/projectile_vertical_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, -6f), "Bottom Projectile Wall", "SizeHolders/projectile_horizontal_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, 6f), "Top Projectile Wall", "SizeHolders/projectile_horizontal_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(-8.5f, 0), "Left Wall", "SizeHolders/vertical_wall",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(8.5f, 0), "Right Wall", "SizeHolders/vertical_wall",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, -5f), "Bottom Wall", "SizeHolders/horizontal_wall",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, 5f), "Top Wall", "SizeHolders/horizontal_wall",
                null, true, "Wall", RigidbodyType2D.Static);
            
            var mainChar = Spawner.NewEntity<MainCharacter>(
                new Vector2(1.6f, -0.9f), "MainCharacter", "idle",
                "Pointer", true, "MainCharacter");
            mainChar.GetComponent<MainCharacter>().Init();
            
            Spawner.NewTabby();
            
            var path00 = "My/My/Sprites/map_line";
            var allSprites00 = Resources.LoadAll<Sprite>(path00);
            var targetName00 = "map_line_0";
            var targetSprite00 = allSprites00.FirstOrDefault(s => s.name == targetName00);
            
            var path = "My/My/Sprites/mineswapper";
            var allSprites = Resources.LoadAll<Sprite>(path);
            
            var targetName5 = "mineswapper_5";
            var targetSprite5 = allSprites.FirstOrDefault(s => s.name == targetName5);
            
            var targetName4 = "mineswapper_4";
            var targetSprite4 = allSprites.FirstOrDefault(s => s.name == targetName4);
            
            var targetName3 = "mineswapper_3";
            var targetSprite3 = allSprites.FirstOrDefault(s => s.name == targetName3);
            
            var targetName2 = "mineswapper_2";
            var targetSprite2 = allSprites.FirstOrDefault(s => s.name == targetName2);
            
            var targetName1 = "mineswapper_1";
            var targetSprite1 = allSprites.FirstOrDefault(s => s.name == targetName1);
            
            var targetName6 = "mineswapper_6";
            var targetSprite6 = allSprites.FirstOrDefault(s => s.name == targetName6);
            
            Spawner.NewMapLine(new Vector2(7.35f, 0), targetSprite00);
            Spawner.NewMapNode(new Vector2(7.35f, 2.5f), targetSprite5);
            Spawner.NewMapNode(new Vector2(7.35f, 2.5f/2f), targetSprite4);
            Spawner.NewMapNode(new Vector2(7.35f, 0), targetSprite3);
            Spawner.NewMapNode(new Vector2(7.35f, -2.5f/2f), targetSprite2);
            Spawner.NewMapNode(new Vector2(7.35f, -2.5f), targetSprite1);
            Spawner.NewMapChar(new Vector2(7.35f, -2.5f), targetSprite6);
            
            UIManager.Instance.ShowStartGameWindow();
            var b1 = Spawner.NewBinButton(new Vector2(-(8f-1f), 4.5f-1f-1f));
            var b2 = Spawner.NewNetButton(new Vector2(-(8f-1f), 4.5f-1f-1f-1f-1f));
            bin = b1.GetComponent<Button>();
            net = b2.GetComponent<Button>();
            bin.DisableButton();
            net.DisableButton();
        }
    }
}