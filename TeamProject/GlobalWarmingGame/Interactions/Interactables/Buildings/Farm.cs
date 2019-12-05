﻿
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Farm : InteractableGameObject, IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 4)};
        private InstructionType plant;
        private InstructionType harvest;
        private bool growing;
        private float timeUntilGrown;
        private float growTime = 20000f;

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
            plant = new InstructionType("plant", "Plant", "Plant", Plant);
            harvest = new InstructionType("harvest", "Harvest", "Harvest", new ResourceItem(new Food(), 10), Harvest);
            timeUntilGrown = 20000f;
            InstructionTypes.Add(plant);
        }

        public void Harvest(Colonist colonist)
        {
            //Harvest wheat
            ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value += 10;
            InstructionTypes.Remove(harvest);
            InstructionTypes.Add(plant);
        }

        public void Plant(Colonist colonist)
        {
            //Plant wheat seeds
            InstructionTypes.Remove(plant);
            growing = true;
        }

        public void Update(GameTime gameTime)
        {
            //Grow crops over time
            if (growing == true) 
            {
                timeUntilGrown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUntilGrown < 0f)
                {
                    InstructionTypes.Add(harvest);
                    growing = false;
                    timeUntilGrown = growTime;
                }
            }
        }
    }
}
