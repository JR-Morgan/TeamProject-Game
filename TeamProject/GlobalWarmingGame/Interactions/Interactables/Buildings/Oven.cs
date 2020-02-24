﻿using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Oven : Sprite, IInteractable, IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.MachineParts), 6),
                                                                                                   new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4),
                                                                                                   new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 2)};

        public List<InstructionType> InstructionTypes { get; }

        //private InstructionType cook;
        //private InstructionType retrieve;

        //private bool cooking;
        //private float timeUntilCooked;
        //private float cockTime = 10000f;

        public Oven(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "Oven",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            //cook = new InstructionType("cook", "Cook", "Cook food", CookFood);
            //retrieve = new InstructionType("retrieve", "Retrieve", "Retrieve food", RetrieveFood);
            //InstructionTypes.Add(cook);
        }

        private void CookFood(IInstructionFollower follower)
        {
            //Take raw food from colonist and place in oven
            //Once cooking don't let the colonist add more food
        }

        private void RetrieveFood(IInstructionFollower follower) { 
            //Take out cooked food from oven

        }

        public void Update(GameTime gameTime)
        {
            //Use game time to cook the food.

        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
