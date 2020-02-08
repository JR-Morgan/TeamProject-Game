﻿
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Forge : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new MachineParts(), 10),
                                                                                                   new ResourceItem(new Stone(), 6) };
        public List<InstructionType> InstructionTypes { get; }

        public Forge(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Forge",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("forge", "Forge", "Forge iron item", ForgeItem));
        }

        private void ForgeItem(IInstructionFollower follower)
        {
            //Open craft menu
            //Force the colonist to wait at the station until job is done
        }

        //Other methods for selected crafting recipe
    }
}
