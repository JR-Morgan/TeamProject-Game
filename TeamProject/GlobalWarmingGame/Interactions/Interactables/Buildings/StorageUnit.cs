﻿using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class StorageUnit : Sprite, IInteractable, IBuildable, IReconstructable
    {
        public List<ResourceItem> CraftingCosts { get; } = new List<ResourceItem>() { new ResourceItem(Resource.Stone, 4),
                                                                                      new ResourceItem(Resource.Wood,  8)};

        public List<InstructionType> InstructionTypes { get; private set; }

        [PFSerializable]
        public ResourceItem resourceItem;

        public InstructionType StoreInstruction { get; private set; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public StorageUnit() : base(Vector2.Zero, Textures.Map[TextureTypes.StorageUnit])
        {

        }

        public StorageUnit(Vector2 position, ResourceItem resourceItem = null) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.StorageUnit]
        )
        {
            ResetState();

            if (resourceItem != null)
            {
                this.resourceItem = resourceItem;

                StoreInstruction = new InstructionType(
                id: "storeItems",
                name: $"Store {this.resourceItem.ResourceType.displayName}",
                description: "",
                //requiredResources: new List<ResourceItem> { new ResourceItem(ResourceItem.ResourceType, 1) },
                //checkValidity: (Instruction i) => i.ActiveMember.Inventory.ContainsType(ResourceItem.ResourceType.ResourceID),
                onStart: StoreItem
                );

                InstructionTypes.Add(new InstructionType(
                                id: "takeItems",
                                name: $"Take All {this.resourceItem.ResourceType.displayName}",
                                description: "",
                                onStart: this.TakeItems
                                )
                    );
            }
        }

        public InstructionType TakeItemInstruction(int amount)
        {
            return new InstructionType(
                id: "takeItem",
                name: $"Take {amount}{resourceItem}",
                onComplete: (Instruction i) => TakeItem(i.ActiveMember.Inventory, amount)
                );
        }
            

        private void TakeItem(Inventory destination, int amount)
        {
            if (resourceItem?.Weight >= amount
                && destination.AddItem(new ResourceItem(resourceItem.ResourceType, amount)))
            {
                resourceItem.Weight -= amount;
            }
        }

        private List<InstructionType> CreateSetResourceInstructionTypes()
        {
            return Enum.GetValues(typeof(Resource)).Cast<Resource>()
                .Select(r => new InstructionType(
                    id: r.ToString(),
                    name: $"Set {r.ToString()}",
                    description: $"Set {r.ToString()} as the active resource",
                    //requiredResources: new List<ResourceItem> { new ResourceItem(ResourceTypeFactory.GetResource(r)) },
                    checkValidity: (Instruction i) => resourceItem == null,
                    onComplete: SetResource
                )).ToList();
        }


        private void SetResource(Instruction instruction)
        {
            //if(instruction.Type.RequiredResources.Count != 1) throw new Exception($"{this.GetType().ToString()} expected RequiredResources.Count to be 1, but was {instruction.Type.RequiredResources.Count}");
            //ResourceItem = instruction.Type.RequiredResources[0];
            Resource r = (Resource) Enum.Parse(typeof(Resource), instruction.Type.ID);
            resourceItem = new ResourceItem(r);
            InstructionTypes.Clear();

            StoreInstruction = new InstructionType(
                            id: "storeItems",
                            name: $"Store {resourceItem.ResourceType.displayName}",
                            description: "",
                            //requiredResources: new List<ResourceItem> { new ResourceItem(ResourceItem.ResourceType, 1) },
                            //checkValidity: (Instruction i) => i.ActiveMember.Inventory.ContainsType(ResourceItem.ResourceType.ResourceID),
                            onStart: StoreItem
                            );

            InstructionTypes.Add(new InstructionType(
                            id: "takeItems",
                            name: $"Take All {resourceItem.ResourceType.displayName}",
                            description: "",
                            onStart: TakeItems
                            )
                );
        }

        private void TakeItems(Instruction instruction)
        {
            Inventory inventory = instruction.ActiveMember.Inventory;
            if (inventory.AddItem(resourceItem))
            {
                ResetState();
            }
           
        }
        private void ResetState()
        {
            this.resourceItem = null;
            InstructionTypes = CreateSetResourceInstructionTypes();
        }

        private static void StoreItem(Instruction instruction)
        {
            StorageUnit storage = (StorageUnit)instruction.PassiveMember;

            if(storage.resourceItem != null)
            {
                Inventory inventory = instruction.ActiveMember.Inventory;
                Resource resourceType = storage.resourceItem.ResourceType.ResourceID;

                if (inventory.Resources.ContainsKey(resourceType))
                {
                    ResourceItem item = inventory.Resources[resourceType];
                    storage.resourceItem.Weight += item.Weight;
                    inventory.RemoveItem(item);
                }
            }
            
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(resourceItem != null)
            {
                Texture2D resourceTexture = resourceItem.ResourceType.Texture;
                spriteBatch.Draw(
                    texture: resourceTexture,
                    position: Position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: Rotation,
                    origin: CalculateOrigin(new Vector2(resourceTexture.Width, resourceTexture.Height)),
                    scale: 1f,
                    effects: SpriteEffect,
                    layerDepth: CalculateDepth(Position, 0.1f)
                );
            }
        }

        public object Reconstruct()
        {
            return new StorageUnit(PFSPosition, resourceItem);
        }
    }
}
