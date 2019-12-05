﻿using Engine;
using Engine.Lighting;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implemntation. 
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SelectionManager selectionManager;

        TileSet tileSet;
        TileMap tileMap;

        private Desktop _desktop;

        Camera camera;
        MainMenu mainMenu;
        PauseMenu pauseMenu;

        PeformanceMonitor peformanceMonitor;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        bool isPaused;
        bool isPlaying;

        List<Light> lightObjects;

        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;
        Texture2D ambiantLight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,  // set this value to the desired width of your window
                PreferredBackBufferHeight = 1080   // set this value to the desired height of your window
            };
            
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            selectionManager = new SelectionManager();
            peformanceMonitor = new PeformanceMonitor();

            this.IsMouseVisible = true;
            
            //Removes 60 FPS limit
            this.graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;


            base.Initialize();     
        }

        #region Load Content
        protected override void LoadContent()
        {
            //INITALISING GAME COMPONENTS
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);

                _desktop = new Desktop();
                MyraEnvironment.Game = this;

                mainMenu = new MainMenu();
                pauseMenu = new PauseMenu();
                ambiantLight = new Texture2D(GraphicsDevice, 1, 1);
                ambiantLight.SetData(new Color[] { Color.DimGray });
            }

            //LIGHTING
            {
                quadRender = new QuadRenderComponent(this);

                shadowmapResolver = new ShadowmapResolver(GraphicsDevice, quadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
                shadowmapResolver.LoadContent(Content);

                lightObjects = new List<Light>() //This code will be replaced
                {
                    //new Light(Vector2.Zero,         GraphicsDevice, 128f, new Color(201,226,255,32), "Light" ),
                    new Light(new Vector2(256,224), GraphicsDevice, 256f, new Color(255,0  ,0  ,255), "Light" ),
                    new Light(new Vector2(224,512), GraphicsDevice, 256f, new Color(0,0  ,255  ,255), "Light" )
                };

                screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }


            //LOADING TILEMAP AND ZONES
            {
                //TODO textures should be loaded from a file
                var textureSet = new Dictionary<string, Texture2D>();

                Texture2D water = this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/water");
                water.Name = "Non-Walkable";

                textureSet.Add("1", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/error"));
                textureSet.Add("2", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/dirt"));
                textureSet.Add("3", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/grass"));
                textureSet.Add("4", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/snow"));
                textureSet.Add("5", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/stone"));
                textureSet.Add("6", water);

                tileSet = new TileSet(textureSet, new Vector2(16));
                //                                                  map0/00.csv  //50x50 tilemap
                //                                                  map1/00.csv  //100x100 tilemap
                tileMap = TileMapParser.parseTileMap(@"Content/maps/map1/00.csv", tileSet);

                ZoneManager.CurrentZone = new Zone() { TileMap = tileMap };
                camera = new Camera(GraphicsDevice.Viewport, tileMap.Size * 16f);
                selectionManager.InputMethods.Add(new MouseInputMethod(camera, _desktop, selectionManager.CurrentInstruction));
            }

            //CREATING GAME OBJECTS
            {
                //All this code below is for testing and will eventually be replaced.

                Texture2D colonist = this.Content.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0");
                Texture2D farm = this.Content.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0");
                Texture2D bushH = this.Content.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0");
                Texture2D bushN = this.Content.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1");
                Texture2D rabbit = this.Content.Load<Texture2D>(@"textures/interactables/animals/rabbit/sprite0");
                Texture2D tree = this.Content.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0");
                Texture2D treeStump = this.Content.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2");

                var c1 = new Colonist(
                    position: new Vector2(480, 200),
                    texture: colonist,
                    inventoryCapacity: 100f);

                selectionManager.CurrentInstruction.ActiveMember = (c1);

                GameObjectManager.Add(c1);

                GameObjectManager.Add(new Colonist(
                    position: new Vector2(256, 512),
                    texture: colonist,
                    inventoryCapacity: 100f));

                GameObjectManager.Add(new Colonist(
                    position: new Vector2(450, 450),
                    texture: colonist,
                    inventoryCapacity: 100f));

                GameObjectManager.Add(new Farm(
                    position: new Vector2(256, 256),
                    texture: farm));

                GameObjectManager.Add(new Bush(
                    position: new Vector2(312, 512),
                    harvestable: bushH,
                    harvested: bushN));

                GameObjectManager.Add(new Interactions.Interactables.Tree(
                    position: new Vector2(312, 612),
                    textureTree: tree,
                    textureStump: treeStump));

                GameObjectManager.Add(new Rabbit(
                    position: new Vector2(575, 575),
                    texture: rabbit));

                GameObjectManager.Add(new DisplayLabel(Vector2.Zero, 0, "Food", _desktop, "lblFood"));


                //Comment out the line below to dissable FPS counter
                GameObjectManager.Add(new DisplayLabel(new Vector2(100, 0), 0, "", _desktop, "lblPerf"));
                
            }
 
        }

        protected override void UnloadContent()
        {

        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            ShowMainMenu();
            ProcessMenuSelection();
            SuspendContextMenuClick();

            if (!isPaused && isPlaying)
            {
                camera.Update(gameTime);
                
                tileMap.Update(gameTime);
                
                foreach (IUpdatable updatable in GameObjectManager.Updatable)
                    updatable.Update(gameTime);

                CollectiveInventory.UpdateCollectiveInventory();

                //Uncomment this line for a light around the cursor (uses the first item in lightObjects)
                //lightObjects[0].Position = Vector2.Transform(Mouse.GetState().Position.ToVector2(), camera.InverseTransform);

                base.Update(gameTime);
            }
            
            if (isPlaying)
            {
                currentKeyboardState = Keyboard.GetState();

                if (CheckKeypress(Keys.Escape))
                    ShowPauseMenu();

                if (CheckKeypress(Keys.NumPad1)) 
                {

                }


                previousKeyboardState = currentKeyboardState;
            }

            peformanceMonitor.Update(gameTime);
            foreach(DisplayLabel lbl in GameObjectManager.GetObjectsByTag("lblPerf"))
            {
                lbl.Message = "\n\n\n" + peformanceMonitor.GetPrintString();
                
            }
        }

        #region Drawing and Lighting
        protected override void Draw(GameTime gameTime)
        {

            //CALCULATE SHADOWS
            foreach (Light light in lightObjects)
            {
                GraphicsDevice.SetRenderTarget(light.RenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                DrawShadowCasters(light);

                shadowmapResolver.ResolveShadows(light.RenderTarget, light.RenderTarget, light.Position);
            }

            //DRAW LIGHTS
            {
                GraphicsDevice.SetRenderTarget(screenShadows);
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: camera.Transform);
                foreach (Light light in lightObjects)
                {
                    light.Draw(spriteBatch);
                }

                spriteBatch.End();


                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                spriteBatch.Draw(ambiantLight, new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.End();

                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);
            }

            //DRAW BACKGROUND
            {
                spriteBatch.Begin(
                    sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.Opaque,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: camera.Transform
                );

                tileMap.Draw(spriteBatch);

                spriteBatch.End();
            }

            //DRAW SHADOWS
            {
                BlendState blendState = new BlendState()
                {
                    ColorSourceBlend = Blend.DestinationColor,
                    ColorDestinationBlend = Blend.SourceColor
                };

                spriteBatch.Begin(
                    sortMode: SpriteSortMode.Immediate,
                    blendState: blendState,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: null
                );
                spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
                spriteBatch.End();
            }

            //DRAW FORGROUND
            {
                spriteBatch.Begin(
                    sortMode: SpriteSortMode.FrontToBack,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: camera.Transform
                );

                foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
                    drawable.Draw(spriteBatch);
                spriteBatch.End();
            }

            //DRAW UI
            {
                _desktop.Render();
            }

            base.Draw(gameTime);

        }

        private void DrawShadowCasters(Light light)
        {
            Matrix transform = Matrix.CreateTranslation(
                -light.Position.X + light.Radius,
                -light.Position.Y + light.Radius,
               0);

            spriteBatch.Begin(
                        sortMode: SpriteSortMode.Deferred,
                        blendState: BlendState.AlphaBlend,
                        samplerState: SamplerState.PointClamp,
                        depthStencilState: DepthStencilState.Default,
                        rasterizerState: RasterizerState.CullNone,
                        effect: null,
                        transformMatrix: transform
                    );

            foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
            {
                drawable.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion

        #region Pause and Main Menu
        void ShowMainMenu()
        {
            if (!isPlaying)
            {
                Point position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 75f, GraphicsDevice.Viewport.Height / 2 - 50f).ToPoint();
                mainMenu.DrawMainMenu(_desktop, position);
            }
        }

        void ShowPauseMenu()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Point position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 75f, GraphicsDevice.Viewport.Height / 2 - 50f).ToPoint();
                pauseMenu.DrawPauseMenu(_desktop, position);
            }

            else
                _desktop.HideContextMenu();
        }

        /// <summary>
        /// Checks if the currently released key had been pressed the previous frame
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckKeypress(Keys key)
        {
            if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                return true;

            return false;
        }

        /// <summary>
        /// Suspends mouse input during Main and Pause Menus, otherwise resumes it.
        /// </summary>
        void SuspendContextMenuClick()
        {
            _desktop.ContextMenuClosing += (s, a) =>
            {
                if (!_desktop.ContextMenu.Bounds.Contains(_desktop.TouchPosition))
                {
                    if (!isPaused || isPlaying)
                        a.Cancel = false;
                    else
                        a.Cancel = true;
                }
            };
        }

        /// <summary>
        /// Executes selected commands on the Main and Pause Menus
        /// </summary>
        void ProcessMenuSelection()
        {
            mainMenu.MainToGame.Selected += (s, a) => { isPaused = false; isPlaying = true; };
            mainMenu.MainToQuit.Selected += (s, a) => Exit();

            pauseMenu.PauseToGame.Selected += (s, a) => { isPaused = false; };
            pauseMenu.PauseToMain.Selected += (s, a) => { isPlaying = false; ShowMainMenu(); };
            pauseMenu.PauseToQuit.Selected += (s, a) => Exit();
        }


        #endregion
    }
}