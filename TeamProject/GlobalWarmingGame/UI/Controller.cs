﻿using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// The controller class controlls the logic behind the UI<br/>
    /// Communicates with the <see cref="View"/> to display menus<br/>
    /// eg.<br/>
    /// When the user clicks, this class will tell View to create a new menu, view is in charge of the GUI specific logic.<br/>
    /// This class defines what UI needs to exist.<br/>
    /// </summary>
    class Controller : IUpdatable
    {

        private Texture2D colonistInventoryIcon;
        
        /// <summary>Reference to the camera for inverse transforms</summary>
        private readonly Camera camera;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private readonly List<Inventory> openInventories;

        public Controller(Camera camera)
        {
            this.camera = camera;
            openInventories = new List<Inventory>();
            AddDropDowns();


        }


        public void LoadContent(ContentManager content)
        {
            colonistInventoryIcon = content.Load<Texture2D>("textures/icons/colonist");
            UpdateColonists();
        }



        public void UpdateColonists()
        {
            foreach (Colonist colonist in GameObjectManager.Filter<Colonist>())
            {
                if(SelectedColonist == null)
                {
                    SelectedColonist = colonist;
                }

                AddInventoryMenu(colonist);
            }
        }

        #region Instruction Menu


        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");
        private static readonly InstructionType COLONIST_INSTRUCTION_TYPE = new InstructionType("selectColonist", "Select", "Selects the colonist");
        private static readonly InstructionType VIEW_INVENTORY = new InstructionType("viewInventory", "View Inventory", "Opens the inventory");

        /// <summary>The currently selected colonist that instructions will be given</summary>
        public Colonist SelectedColonist { get; set; }

        /// <summary>
        /// Takes an input GameObject 
        /// </summary>
        /// <param name="objectClicked">the object that was </param>
        /// <returns></returns>
        private List<ButtonHandler<Instruction>> GenerateInstructionOptions(GameObject objectClicked, Colonist colonist)
        {
            List<ButtonHandler<Instruction>> options = new List<ButtonHandler<Instruction>>();

            if (objectClicked != null)
            {
                options.Add(new ButtonHandler<Instruction>(new Instruction(WALK_INSTRUCTION_TYPE, colonist, objectClicked), IssueInstruction));

                if (objectClicked is IInteractable)
                {
                    foreach (InstructionType type in ((IInteractable)objectClicked).InstructionTypes)
                    {
                        options.Add(new ButtonHandler<Instruction>(new Instruction(type, colonist, objectClicked), IssueInstruction));
                    }
                }

                if (objectClicked is Colonist)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(COLONIST_INSTRUCTION_TYPE, colonist, objectClicked), SelectColonist));
                } else if (objectClicked is IStorage)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(VIEW_INVENTORY, null, objectClicked), ViewInventory));
                }


                //If the colonist is allowed to start constructing a building
                if (constructing)
                {
                    //Building gets fed a Interactable that is also a buildable
                    building = (IBuildable)InteractablesFactory.MakeInteractable(SelectedBuildable, objectClicked.Position);

                    //If Colonist has the resources build
                    if (SelectedColonist.Inventory.ContainsAll(building.CraftingCosts))
                    {
                        InstructionType construct = new InstructionType("build", "Build", "Build the " + SelectedBuildable.ToString(), onStart: Build);
                        options.Add(new ButtonHandler<Instruction>(new Instruction(construct, colonist, (GameObject)building), IssueInstruction));
                    }
                    //TODO Else show notification that the colonist can't craft the building
                }
            }

            return options;
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstruction(Instruction instruction)
        {
            instruction.ActiveMember.AddInstruction(instruction, 0);
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private void ViewInventory(Instruction instruction)
        {
            Inventory inventory = ((IStorage)instruction.PassiveMember).Inventory;
            if (!openInventories.Contains(inventory))
            {
                AddInventoryMenu((IStorage)instruction.PassiveMember);
            } else
            {
                ToggleInventoryVisibility(inventory);
            }
        }

        /// <summary>
        /// Takes the passive member of the instruction and sets that as the active colonist<br>
        /// This is meant to be used to select a colonist
        /// </summary>
        /// <param name="instruction">The instruction that has been selected</param>
        private void SelectColonist(Instruction instruction)
        {
            SelectedColonist = (Colonist)instruction.PassiveMember;
            ViewInventory(instruction);
        }


        #endregion

        #region Drop-Down Menu

        private static bool constructing = false;
        private static IBuildable building;
        public static Interactable SelectedBuildable { get; set; }

        /// <summary>
        /// Adds the Building and Spawn dropdown menus to the view
        /// </summary>
        private void AddDropDowns()
        {
            //Buildings drop down
            View.CreateDropDown("Building", new List<ButtonHandler<Interactable>>
            {
                new ButtonHandler<Interactable>(Interactable.CampFire,  this.SelectBuildable),
                new ButtonHandler<Interactable>(Interactable.Farm,      this.SelectBuildable),
                new ButtonHandler<Interactable>(Interactable.WorkBench, this.SelectBuildable)
            });

            //Spawnables drop down
            View.CreateDropDown("Spawn", Enum.GetValues(typeof(Interactable)).Cast<Interactable>()
                .Select(i => new ButtonHandler<Interactable>(i, SpawnInteractable)).ToList());
        }


        /// <summary>
        /// Delegate method to select the right building
        /// </summary>
        /// <param name="buildable"></param>
        private void SelectBuildable(Interactable interactable)
        {
            SelectedBuildable = interactable;
            constructing = true;
        }

        /// <summary>
        /// Delegate method to spawn the right interactable
        /// </summary>
        /// <param name="interactable"></param>
        private void SpawnInteractable(Interactable interactable)
        {
            Vector2 position = ZoneManager.CurrentZone.TileMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size - camera.Position;
            //Map the position onto the nearest tile and then get that tiles position
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(interactable, ZoneManager.CurrentZone.TileMap.GetTileAtPosition(position).Position));
        }

        /// <summary>
        /// Called when colonist is at the building site and then the building is made visible
        /// </summary>
        /// <param name="follower"></param>
        private static void Build(IInstructionFollower follower)
        {
            List<ResourceItem> buildingCosts = new List<ResourceItem>();

            //If Colonist has the resources build
            if (follower.Inventory.ContainsAll(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                    follower.Inventory.RemoveItem(item);

                building.Build();
            }
            //Else show notification that the colonist can't craft the building
            constructing = false;
        }
        #endregion

        #region Update loop

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Called on a mouse click
        /// </summary>
        private void OnClick()
        {
            if (!View.Hovering)
            {
                Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                List<ButtonHandler<Instruction>> options = GenerateInstructionOptions(objectClicked, SelectedColonist);
                if (options != null)
                {
                    View.CreateMenu(currentMouseState.Position, options);
                }
            }
        }


        /// <summary>
        /// Checks to see if a <see cref="Interactions.IInteractable"/> <see cref="GameObject"/> was clicked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static GameObject ObjectClicked(Point position)
        {
            foreach (GameObject o in GameObjectManager.Interactable)
            {
                if (new Rectangle(o.Position.ToPoint(), o.Size.ToPoint()).Contains(position))
                {
                    return o;
                }
            }
            return ZoneManager.CurrentZone.TileMap.GetTileAtPosition(position.ToVector2());
        }
        #endregion


        private static void UpdateInventoryMenu(Inventory inventory)
        {
            IEnumerable<ItemElement> ItemElements = inventory.Resources.Values.Select(i => new ItemElement(i.ResourceType.Texture, i.Weight.ToString()));
            View.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);
        }

        private void AddInventoryMenu(IStorage storage)
        {
            if (!openInventories.Contains(storage.Inventory));
            {
                Texture2D icon = storage is Colonist ? colonistInventoryIcon : null;
                View.AddInventory(new ButtonHandler<Inventory>(storage.Inventory, ToggleInventoryVisibility), icon: icon);
                openInventories.Add(storage.Inventory);
                storage.Inventory.InventoryChange += Inventory_InventoryChange;
                UpdateInventoryMenu(storage.Inventory);
            }
        }

        private void RemoveInventoryMenu(Inventory inventory)
        {
            if(openInventories.Contains(inventory))
            {
                openInventories.Remove(inventory);
                inventory.InventoryChange -= Inventory_InventoryChange;
                View.RemoveInventory(inventory.GetHashCode());
            }
            
        }

        /// <summary>
        /// Event handler for <see cref="Inventory.InventoryChange"/> Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inventory_InventoryChange(object sender, EventArgs e)
        {
            UpdateInventoryMenu((Inventory)sender);
        }

        private void ToggleInventoryVisibility(Inventory inventory)
        {
            View.ToggleInventoryMenuVisibility(inventory.GetHashCode());
        }

    }
}
