﻿using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.TileGrid
{
    /// <summary>
    /// This class reperesents a Tile Map Tile Game Object
    /// </summary>
    public class Tile : GameObject, Engine.Drawing.IDrawable
    {
        public static bool tempSystem = true; 
        private readonly Texture2D texture;
        public Temperature Temperature { get; set; }
        public bool Heated { get; set; }
        public string Type { get; }
        public bool Walkable { get; }

        private readonly Rectangle sourceRectangle;
        private readonly Rectangle destinationRectangle;
        public Tile(Texture2D texture, Vector2 position, Vector2 size, bool walkable, float initialTemperature) : base(position, size)
        {
            this.Type = texture.Name;

            this.texture = texture;
            this.Walkable = walkable;

            Temperature = new Temperature(initialTemperature);

            sourceRectangle = new Rectangle(
                        location: new Point((int)Position.X % texture.Width, (int)Position.Y % texture.Height),
                        size: Size.ToPoint()
                        );
            destinationRectangle = new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!tempSystem)
            {
                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: destinationRectangle,
                    sourceRectangle: sourceRectangle,
                    color: Color.White
                    );
            }
            else
            {
                spriteBatch.Draw(
                     texture: texture,
                     destinationRectangle: destinationRectangle,
                     sourceRectangle: sourceRectangle,
                     color: new Color(170 - 5 + ((int)Temperature.Value * 2) /* red */, 0 /* green */, 170 - 5 - ((int)Temperature.Value * 2) /*blue*/)
                     ) ;
            }
        }
    }
}