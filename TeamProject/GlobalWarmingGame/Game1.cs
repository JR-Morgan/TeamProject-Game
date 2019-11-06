﻿using Engine;
using Engine.TileGrid;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

using System.Collections.Generic;

namespace GlobalWarmingGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseSelectionManager mouseSelectionManager;
        PathFindable tpf;

        TileSet tileSet;
        TileMap tileMap;

        Point clickPos;

        private Desktop _desktop;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mouseSelectionManager = new MouseSelectionManager();
        }
        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        private void onClick()
        {
            clickPos = _desktop.TouchPosition;

            if (_desktop.ContextMenu != null)
            {
                return;
            }

            var container = new VerticalStackPanel
            {
                Spacing = 4
            };

            var titleContainer = new Panel
            {
                Background = DefaultAssets.UISpritesheet["button"]
            };

            var titleLabel = new Label
            {
                Text = "Choose Action",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            titleContainer.Widgets.Add(titleLabel);
            container.Widgets.Add(titleContainer);

            var menuItem1 = new MenuItem();
            menuItem1.Id = "";
            menuItem1.Text = "Move Here";
            menuItem1.Selected += (s, a) =>
            {
                foreach (IClickable o in GameObjectManager.GetObjectsByTag("PathFindable")) {
                    o.OnClick(clickPos);
                }
            };

            var menuItem2 = new MenuItem();
            menuItem2.Id = "";
            menuItem2.Text = "Do Nothing";

            var verticalMenu = new VerticalMenu();
            verticalMenu.Items.Add(menuItem1);
            verticalMenu.Items.Add(menuItem2);

            container.Widgets.Add(verticalMenu);

            _desktop.ShowContextMenu(container, _desktop.TouchPosition);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            {
                //TODO this code should be loaded from a file
                var textureSet = new Dictionary<string, Texture2D>();


                Texture2D water = this.Content.Load<Texture2D>(@"tileset/test_tileset-1/water");
                water.Name = "Non-Walkable";

                textureSet.Add("0", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/error"));
                textureSet.Add("1", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/dirt"));
                textureSet.Add("2", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/grass"));
                textureSet.Add("3", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/snow"));
                textureSet.Add("4", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/stone"));
                textureSet.Add("5", water);


                tileSet = new TileSet(textureSet, new Vector2(16));
                tileMap = TileMapParser.parseTileMap(@"Content/testmap.csv", tileSet);

                
                ZoneManager.CurrentZone = new Zone() { TileMap = tileMap };


                tpf = new PathFindable(
                    position:   new Vector2(0),
                    size:       new Vector2(50),
                    rotation:   0, 
                    rotationOrigin: new Vector2(0),
                    tag:        "PathFindable",
                    depth:      0,
                    texture:    tileSet.tileSetTextures["0"],
                    speed:      1f);;

                GameObjectManager.Add(tpf) ;

                //tpf.AddGoal(new Vector2(100, 100));
                //tpf.AddGoal(new Vector2(100, 50));
                //tpf.AddGoal(new Vector2(25,75));
                //tpf.AddGoal(new Vector2(0));

                MyraEnvironment.Game = this;

                _desktop = new Desktop();
                _desktop.TouchDown += (s, a) => onClick();

            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // mouseSelectionManager.Update(GameObjectManager.Objects);

            foreach (IUpdatable updatable in GameObjectManager.Updatable)
                updatable.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            tileMap.Draw(spriteBatch);

            


            foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
                drawable.Draw(spriteBatch);

            spriteBatch.End();

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}