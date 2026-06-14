using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ZevWaxGames.CursorHero
{
    public class G : MonoBehaviour
    {
        public static G Instance { get; private set; }
        public Button BtnBin => btnBin;
        public Button BtnPC => btnPC;
        
        private Button btnBin;
        private Button btnPC;
        private GameState currGameStat;
        private bool isTransitionRequested = false;
        
        private void Awake() => Instance = this;

        private async void Start()
        {
            InitHardwareSettings();
            await RunGameLoop();
        }
        private void InitHardwareSettings()
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        private void Setup()
        {
            Spawner.NewEntity<ProjectileWall>(
                new Vector2(-9.5f, 0), "Left Projectile Wall", "SizeHolders/projectile_vertical_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<ProjectileWall>(
                new Vector2(9.5f, 0), "Right Projectile Wall", "SizeHolders/projectile_vertical_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<ProjectileWall>(
                new Vector2(0, -6f), "Bottom Projectile Wall", "SizeHolders/projectile_horizontal_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<ProjectileWall>(
                new Vector2(0, 6f), "Top Projectile Wall", "SizeHolders/projectile_horizontal_wall",
                null, true, "ProjectileWall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(-8.5f, 0), "Left Wall", "SizeHolders/30x480",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(8.5f, 0), "Right Wall", "SizeHolders/30x480",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, -5f), "Bottom Wall", "SizeHolders/480x30",
                null, true, "Wall", RigidbodyType2D.Static);
            Spawner.NewEntity<Wall>(
                new Vector2(0, 5f), "Top Wall", "SizeHolders/480x30",
                null, true, "Wall", RigidbodyType2D.Static);
            
            var mainChar = Spawner.NewEntity<MainCharacter>(
                new Vector2(1.6f, -0.9f), "MainCharacter", Spawner.GetSprite("SizeHolders/35x30"),
                "Pointer", Spawner.GetSprite("main_character_collider"), "MainCharacter");
            mainChar.GetComponent<MainCharacter>().Init();
            
            Spawner.NewZipporah();
            Spawner.NewHumBar();
            
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
            
            //UIManager.Instance.ShowStartGameWindow();
            
            var btnBinObj = Spawner.NewBinButton(new Vector2(-(8f-1f), 4.5f-1f-1f));
            var btnPCObj = Spawner.NewNetButton(new Vector2(-(8f-1f), 4.5f-1f-1f-1f-1f));
            btnBin = btnBinObj.GetComponent<Button>();
            btnPC = btnPCObj.GetComponent<Button>();
            btnBin.DisableButton();
            btnPC.DisableButton();
        }
        private Task WaitAsync(Action<Action> subscribe, Action<Action> unsubscribe)
        {
            var tcs = new TaskCompletionSource<bool>();
            Action handler = null;

            handler = () =>
            {
                unsubscribe(handler);
                tcs.TrySetResult(true);
            };

            subscribe(handler);
            return tcs.Task;
        }
        private async Task RunGameLoop()
        {
            
            currGameStat = GameState.FadingIn;
            
            await WaitAsync(
                handler => EventHolder.OnFadingInFinished += handler,
                handler => EventHolder.OnFadingInFinished -= handler
            );
            
            currGameStat = GameState.HitTheBtn;
            btnPC.EnableButton();
            
            await WaitAsync(
                handler => EventHolder.OnPCBtnPushed += handler,
                handler => EventHolder.OnPCBtnPushed -= handler
            );
            
            currGameStat = GameState.FadingOut;
            btnPC.DisableButton();
            
            await WaitAsync(
                handler => EventHolder.OnFadingOutFinished += handler,
                handler => EventHolder.OnFadingOutFinished -= handler
            );

            Spawner.NewBottle(Vector2.zero, true);
            SpawnGlassPieces();
            
            currGameStat = GameState.FadingIn;
            
            await WaitAsync(
                handler => EventHolder.OnFadingInFinished += handler,
                handler => EventHolder.OnFadingInFinished -= handler
            );
        }

        private void SpawnGlassPieces()
        {
            Spawner.NewLayingPieceOfGlass(new Vector2(-6.66f, -3.77f), new Color(0.2f, 0.4f, 0.15f, 0.75f));
            Spawner.NewLayingPieceOfGlass(new Vector2(-7.1f, -4.2f), new Color(0.2f, 0.4f, 0.15f, 0.75f));
            Spawner.NewLayingPieceOfGlass(new Vector2(-7.5f, -4f), new Color(0.2f, 0.4f, 0.15f, 0.75f));
            Spawner.NewLayingPieceOfGlass(new Vector2(-8f, -4.5f), new Color(0.2f, 0.4f, 0.15f, 0.75f));
        }
    }
}