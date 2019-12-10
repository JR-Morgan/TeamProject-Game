﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class Camera : IUpdatable
    {
       
        protected Viewport viewport;
        private MouseState mouseState;
        private KeyboardState keyboardState;
        private int scroll;
        

        public float MovementSpeed { get; set; }
        public float ZoomSpeed { get; set; }
        public Vector2 ClampSize { get; set; }
        public Matrix Transform { get; set; }
        public Matrix InverseTransform { get { return Matrix.Invert(Transform); } }

        public float Zoom { get; set; }


        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }


        public Camera(Viewport viewport, Vector2 clampSize)
        {
            this.viewport = viewport;
            this.Position = Vector2.Zero;
            this.scroll = 1;
            this.ClampSize = clampSize;
            ResetCamera();
        }

        private void ResetCamera()
        {
            this.Position = ClampSize / 2;
            this.MovementSpeed = 1f;
            this.ZoomSpeed = 0.1f;
            this.Zoom = 2.0f;
        }

        

        /// <summary>
        /// Fetches Input values and updates Transforms accordingly
        /// </summary>
        public void Update(GameTime gameTime)
        {
            GetInput(gameTime);

            Vector2 clampSize = ClampSize;
            Zoom = MathHelper.Clamp(Zoom, 1f, 5.0f); // Clamps Zoom value
            _position.X = MathHelper.Clamp(Position.X, 0f, clampSize.X ); // Clamps camera position on X
            _position.Y = MathHelper.Clamp(Position.Y, 0f, clampSize.Y ); // Clamps camera position on Y

            Transform = Matrix.CreateTranslation(Position.X, Position.Y, 0) * // Main Translation Matrix
                Matrix.CreateTranslation(-clampSize.X, -clampSize.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * // Scale Matrix
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0)); // Origin Offset Matrix

        }

        /// <summary>
        /// Fetches Mouse and Keyboard Input
        /// </summary>
        private void GetInput(GameTime gameTime)
        {
            GetMouseInput(gameTime);
            GetKeyboardInput(gameTime);
        }

        /// <summary>
        /// Captures Mouse State, sets Zoom value according to the current Mouse Wheel value then updates said value
        /// </summary>
        private void GetMouseInput(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            if (mouseState.ScrollWheelValue > scroll)
            {
                Zoom += ZoomSpeed;
                scroll = mouseState.ScrollWheelValue;
            }

            if (mouseState.ScrollWheelValue < scroll)
            {
                Zoom -= ZoomSpeed;
                scroll = mouseState.ScrollWheelValue;
            }
        }

        /// <summary>
        /// Captures Keyboard State then updates the Viewport's Position based on fetched Input
        /// </summary>
        private void GetKeyboardInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            float distance = MovementSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _position.Y += distance;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X += distance;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _position.Y -= distance;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X -= distance;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                ResetCamera();
            }
        }
    }
}
