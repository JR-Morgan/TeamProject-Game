﻿namespace Engine
{
    public interface IClickable
    {
        void OnClick(Microsoft.Xna.Framework.Input.MouseState mouseState);
        void OnClick(Microsoft.Xna.Framework.Point clickPos);
    }
}
