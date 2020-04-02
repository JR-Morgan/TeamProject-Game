﻿using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Forge : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.MachineParts, 10),
                                                                                                   new ResourceItem(Resource.Stone, 6) };
        public List<InstructionType> InstructionTypes { get; }

        public Forge(Vector2 position, Texture2D texture) : base
        (
            position: position,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("forge", "Forge", "Forge iron item", onComplete: ForgeItem)
            };
        }

        private void ForgeItem(Instruction instruction)
        {
            //Open craft menu
            //Force the colonist to wait at the station until job is done
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }

        //Other methods for selected crafting recipe
    }
}
