﻿
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Farm : InteractableGameObject, IBuildable
    {
        public Temperature Temperature { get; private set; } = new Temperature(10);
        private Vector2 size;
        private Vector2 position;
        public new Vector2 Size 
        {
            get 
            {
                return size;
            }                
        }
        public new Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Farm(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Farm",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            this.position = position; 
            size = new Vector2(texture.Width, texture.Height);
            InstructionTypes.Add(new InstructionType("harvest", "Harvest", "Harvest the farm", new ResourceItem(new Food(), 3), Harvest));
        }

        public void Harvest()
        {
            //This is tempory and should be replaced by the resource system
            ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value += 3;
        }
    }
}
