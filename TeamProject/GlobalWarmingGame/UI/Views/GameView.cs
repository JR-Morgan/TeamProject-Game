﻿using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI.Views
{
    /// <summary>
    /// This class is for implementation specific UI, in this case <see cref="GeonBit.UI"/> <br/>
    /// This class is for creating buttons and panels based on the information provided by <see cref="GameUIController"/>.<br/>
    /// This class should not reference any <see cref="GlobalWarmingGame"/> specific classes, and should be the only class referencing <see cref="GeonBit.UI"/> specific classes.<br/>
    /// </summary>
    class GameView
    {
        private PauseMenu PauseMenu;
        private SettingsMenu SettingsMenu;

        private Paragraph temperatureReadout;
        private Panel topPanel;
        private Panel bottomPanel;
        private Icon temperatureButton;
        private Panel menu;
        private readonly Dictionary<int, Panel> inventories;
        private readonly Dictionary<int, Icon> inventoryButtons;
        private readonly Dictionary<int, Icon> temperatureWarnings;
        private readonly Dictionary<int, Icon> foodWarnings;
        private readonly Dictionary<int, Icon> combatWarnings;

        /// <summary>True if the current mouse position is over a UI entity</summary>
        internal bool Hovering { get; set; }

        public GameView()
        {
            inventories = new Dictionary<int, Panel>();
            inventoryButtons = new Dictionary<int, Icon>();
            temperatureWarnings = new Dictionary<int, Icon>();
            foodWarnings = new Dictionary<int, Icon>();
            combatWarnings = new Dictionary<int, Icon>();
        }

        internal void Initalise(ContentManager content)
        {
            UserInterface.Initialize(content, "main");
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { Hovering = true; };
        }

        /// <summary>
        /// Resets currently active UI elements
        /// </summary>
        internal void Clear()
        {
            UserInterface.Active.Clear();
            inventories.Clear();
            inventoryButtons.Clear();
            temperatureWarnings.Clear();
            foodWarnings.Clear();
            combatWarnings.Clear();
            menu = null;
        }

        internal void CreateUI()
        {
            UserInterface.Active.UseRenderTarget = false;
            PauseMenu = new PauseMenu
            {
                Visible = false
            };

            UserInterface.Active.AddEntity(PauseMenu);

            SettingsMenu = new SettingsMenu()
            {
                Visible = false
            };
            UserInterface.Active.AddEntity(SettingsMenu);

            temperatureReadout = new Paragraph("", Anchor.TopLeft);
            UserInterface.Active.AddEntity(temperatureReadout);

            #region topPanel
            topPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.TopCenter)
            {
                Opacity = 192,
                Visible = true,
            };
            UserInterface.Active.AddEntity(topPanel);
            #endregion


            temperatureButton = new Icon(IconType.PotionRed, Anchor.BottomRight, background: true, offset: new Vector2(+30, +120))
            {
                OnClick = d => { GameObjectManager.ZoneMap.TemperatureMode = !GameObjectManager.ZoneMap.TemperatureMode; }
            };

            UserInterface.Active.AddEntity(temperatureButton);

            #region bottomPanel
            bottomPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.BottomCenter)
            {
                Opacity = 192,
                Visible = true,
            };
            UserInterface.Active.AddEntity(bottomPanel);
            #endregion

            
        }

        

        internal void SetUIScale(float scale)
        {
            UserInterface.Active.GlobalScale = scale;
        }

        internal void Update(GameTime gameTime)
        {
            Hovering = false;
            UserInterface.Active.Update(gameTime);
        }

        internal void SetPauseMenuVisiblity(bool show)
        {
            if (menu != null)
            {
                UserInterface.Active.RemoveEntity(menu);
                menu = null;
            }
            PauseMenu.Visible = show;
        }
        internal void SetSettingsMenuVisiblity(bool show) => SettingsMenu.Visible = show;

        internal void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }

        /// <summary>
        /// Creates a button list menu.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="text">The label of the menu</param>
        /// <param name="location">the screenspace location of the menu</param>
        /// <param name="options">the elements of the menu</param>
        internal void CreateMenu<T>(string text, Point location, List<ButtonHandler<T>> options)
        {
            if (menu != null) UserInterface.Active.RemoveEntity(menu);

            menu = new Panel(new Vector2(190, 80f + (options.Count * 40f)), PanelSkin.Simple, Anchor.TopLeft, location.ToVector2() / UserInterface.Active.GlobalScale)
            {
                Opacity = 200
            };
            UserInterface.Active.AddEntity(menu);

            Label label = new Label(text, Anchor.TopCenter, new Vector2(190f, 20f))
            {
                Scale = 0.8f
            };

            menu.AddChild(label);
            int counter = 0;
            foreach (ButtonHandler<T> option in options)
            {
                Button newButton = new Button(option.ToString(), ButtonSkin.Default, Anchor.TopCenter, new Vector2(175f, 30f), new Vector2(0f, (counter + 1f) * 40f));
                newButton.ButtonParagraph.Scale = 0.85f;

                newButton.Padding = Vector2.Zero;
                menu.AddChild(newButton);

                newButton.OnClick = (Entity btn) =>
                {
                    option.action(option.Tag);
                    menu.Dispose();
                    UserInterface.Active.RemoveEntity(menu);
                    menu = null;
                };
                counter++;
            }

        }

        /// <summary>
        /// Creates a drop down menu in the top panel.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="text">The label of the dropdown button</param>
        /// <param name="options">The elements of the drop down</param>
        internal void CreateDropDown<T>(string text, List<ButtonHandler<T>> options)
        {
            DropDown menu = new DropDown(new Vector2(225f, 75f), Anchor.CenterLeft, new Vector2(250f * topPanel.Children.Count, 4f), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = text,
                AutoSetListHeight = true,
                DontKeepSelection = true,
            };

            foreach (ButtonHandler<T> option in options)
            {
                menu.AddItem(option.ToString());
            }

            topPanel.AddChild(menu);

            menu.OnValueChange = (Entity e) =>
            {
                foreach (ButtonHandler<T> option in options)
                {
                    if (option.ToString().Equals(menu.SelectedValue))
                    {
                        option.action(option.Tag);
                        break;
                    }

                }
            };

        }


        internal void ClearDropDown()
        {
            topPanel.ClearChildren();
        }

        /// <summary>
        /// Creates a notification for the user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">Common notification text</param>
        /// <param name="list">List of objects of type T that will be appended to the notification text</param>
        internal void Notification<T>(string text, int secondDelay, IEnumerable<T> list = null) 
        {
            string notificatonText = text;

            Panel Notification = new Panel(new Vector2(325f, 100f), PanelSkin.Default, Anchor.TopCenter, new Vector2(0, 100f))
            {
                Padding = Vector2.Zero,
                Visible = false
            };

            UserInterface.Active.AddEntity(Notification);

            if (list != null) 
            {
                foreach(T item in list) 
                {
                    Notification.Size += new Vector2(0f, 10f);
                    notificatonText += "\n " + item.ToString();
                }
            }

            Notification.AddChild(new Label(notificatonText, Anchor.Center));
            Notification.Visible = true;

            Task.Delay(new TimeSpan(0, 0, secondDelay)).ContinueWith(o =>
            {
                Notification.Dispose();
                UserInterface.Active.RemoveEntity(Notification);
            });
        }


        /// <summary>
        /// Adds an inventory button and menu to the bottom panel.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="buttonHandler">The <see cref="ButtonHandler{T}"/> that handles the OnClick event</param>
        /// <param name="visible">whether the inventory menu should be visible on creation</param>
        /// <param name="icon">A custom <see cref="Texture2D"/> that is to be used, if null then <see cref="IconType.Sack"/> will be used</param>
        internal void AddInventory<T>(ButtonHandler<T> buttonHandler, bool visible = false, Texture2D icon = default)
        {
            bool customIcon = icon != default;
            Icon inventoryButton = new Icon(customIcon ? IconType.None : IconType.Sack, Anchor.BottomLeft, 1f, true, new Vector2(64f * inventories.Count, 0f)) ;
            if(customIcon) inventoryButton.Texture = icon;

            Icon tempWarning = new Icon(IconType.OrbBlue)
            {
                Locked = true,
                Scale = 0.75f
            };
            inventoryButton.AddChild(tempWarning);
            temperatureWarnings.Add(buttonHandler.Tag.GetHashCode(), tempWarning);

            Icon foodWarning = new Icon(IconType.Apple)
            {
                Locked = true,
                Scale = 0.75f
            };
            inventoryButton.AddChild(foodWarning);
            foodWarnings.Add(buttonHandler.Tag.GetHashCode(), foodWarning);

            Icon combatWarning = new Icon(IconType.ShieldAndSword)
            {
                Locked = true,
                Scale = 0.75f
            };
            inventoryButton.AddChild(combatWarning);
            combatWarnings.Add(buttonHandler.Tag.GetHashCode(), combatWarning);

            inventoryButtons.Add(buttonHandler.Tag.GetHashCode(), inventoryButton);
            bottomPanel.AddChild(inventoryButton);

            inventoryButton.OnClick = (Entity btn) => {
                buttonHandler.action(buttonHandler.Tag);
            };

            Panel inventory = new Panel(new Vector2(282f, 400f), PanelSkin.Simple, Anchor.BottomLeft, new Vector2(-26f, 75f))
            {
                Opacity = 192,
                Visible = visible,
            };

            inventories.Add(buttonHandler.Tag.GetHashCode(), inventory);

            bottomPanel.AddChild(inventory);
        }

        /// <summary>
        /// Sets the elements of the inventory menu.
        /// </summary>
        /// <param name="id">The unique ID of the inventory menu (eg hashcode)</param>
        /// <param name="items">Items that are to be added to the inventory menu</param>
        /// <example><c>View.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);</c></example>
        internal void UpdateInventoryMenu(int id, IEnumerable<ItemElement> items)
        {
            inventories[id].ClearChildren();
            foreach (ItemElement i in items)
            {
                inventories[id].AddChild(CreateInventoryElement(i));
            }

            for (int i = inventories[id].Children.Count; i < 24; i++)
            {
                inventories[id].AddChild(CreateInventoryElement(new ItemElement(null, "0")));
            }
        }

        internal void SetActiveInventory(int id)
        {
            foreach(Icon i in inventoryButtons.Values)
            {
                i.FillColor = new Color(255,255,255,255);
            }

            inventoryButtons[id].FillColor = new Color(255, 255, 255, 129);

            SetInventoryVisiblity(id);
        }

        private Entity CreateInventoryElement(ItemElement i)
        {
            Icon slot = new Icon(IconType.None, Anchor.AutoInline, 0.75f, true);
            if (i.Texture != null) slot.Texture = i.Texture;

            
            slot.AddChild(new Label(i.Label, Anchor.TopLeft, null, new Vector2(7.9f, -20f)));
            return slot;
        }

        /// <summary>
        /// Sets the given inventory as the currently selected inventory
        /// </summary>
        /// <param name="id">The unique id (eg hashcode) of the inventory  that is to be made visible</param>
        /// <example><c>View.SetInventoryVisible(inventory.GetHashCode());</c></example>
        internal void SetInventoryVisiblity(int id)
        {
            foreach (Entity panel in inventories.Values)
            {
                panel.Visible = false;
            }
            inventories[id].Visible = true;
        }


        /// <summary>
        /// Removes an inventory menu
        /// </summary>
        /// <param name="id">The unique id (eg hashcode) of the inventory  that is to be removed</param>
        /// <example><c>View.RemoveInventory(inventory.GetHashCode());</c></example>
        internal void RemoveInventory(int id)
        {
            bottomPanel.RemoveChild(inventoryButtons[id]);
            inventoryButtons.Remove(id);
            combatWarnings.Remove(id);
            foodWarnings.Remove(id);
            temperatureWarnings.Remove(id);
            bottomPanel.RemoveChild(inventories[id]);
            inventories[id].Dispose();
            inventories.Remove(id);
            UpdateInventoryButtonPositions();
        }

        private void UpdateInventoryButtonPositions()
        {
            int counter = 0;
            foreach(Entity e in inventoryButtons.Values)
            {
                e.Offset = new Vector2(64f * counter++, 0f);
            }
        }


        internal void UpdateTemp(string text, Vector2 position)
        {
            temperatureReadout.Text = text;
            temperatureReadout.Offset = position + new Vector2(20,30);
        }

        internal void UpdateHungerColonistWarning(int id, bool display)
        {
            if (foodWarnings.ContainsKey(id)) foodWarnings[id].Visible = display;
        }
        internal void UpdateTemperatureColonistWarning(int id, bool display)
        {
            if (temperatureWarnings.ContainsKey(id)) temperatureWarnings[id].Visible = display;
        }
        internal void UpdateCombatColonistWarning(int id, bool display)
        {
            if (combatWarnings.ContainsKey(id)) combatWarnings[id].Visible = display;
        }
    }
}
