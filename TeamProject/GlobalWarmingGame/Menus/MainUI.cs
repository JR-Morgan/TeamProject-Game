﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;

namespace GlobalWarmingGame.Menus
{
    class MainUI : Entity
    {
        public Panel TopPanel { get; private set; }
        public Panel BottomPanel { get; private set; }
        public DropDown BuildMenu { get; private set; }
        public DropDown SpawnMenu { get; private set; }
        public Icon[] ItemSlots { get; set; }
        public Label[] ItemLabels { get; set; }
        public ProgressBar[] HealthBars { get; set; }
        
        Label foodLabel;
        GameObject[] colonists;

        private float timeToHealthUpdate = 500f;
        private float timeUnitlHealthUpdate;

        bool open;

        public MainUI()
        {
            //Top Panel
            TopPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.TopCenter)
            {
                Opacity = 192
            };

            BuildMenu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(0, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = "Buildings",
                AutoSetListHeight = true
            };
            TopPanel.AddChild(BuildMenu);

            SpawnMenu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(250, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = "Spawn",
                AutoSetListHeight = true
            };
            TopPanel.AddChild(SpawnMenu);

            Icon foodIcon = new Icon(IconType.Apple, Anchor.CenterRight, 1f, false);
            TopPanel.AddChild(foodIcon);

            foodLabel = new Label("Food Counter", Anchor.CenterRight, null, new Vector2(75,0));
            TopPanel.AddChild(foodLabel);

            UserInterface.Active.AddEntity(TopPanel);

            //Bottom Panel
            BottomPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.BottomCenter)
            {
                Opacity = 192
            };

            Icon collectiveInventoryButton = new Icon(IconType.Sack, Anchor.CenterLeft, 1f, true);
            BottomPanel.AddChild(collectiveInventoryButton);

            Panel collectiveInventory = new Panel(new Vector2(282, 400), PanelSkin.Simple, Anchor.TopLeft, new Vector2(-26, -426))
            {
                Opacity = 192,
                Visible = open
            };
            BottomPanel.AddChild(collectiveInventory);

            collectiveInventoryButton.OnClick = (Entity btn) => { open = !open; collectiveInventory.Visible = open; };

            ItemSlots = new Icon[24];
            ItemLabels = new Label[ItemSlots.Length];
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new Icon(IconType.None, Anchor.AutoInline, 0.75f, true);
                collectiveInventory.AddChild(ItemSlots[i]);

                ItemLabels[i] = new Label("0", Anchor.TopLeft, null, new Vector2(7.9f,-20));
                ItemSlots[i].AddChild(ItemLabels[i]);
            }

            UserInterface.Active.AddEntity(BottomPanel);

            TopPanel.Visible = false;
            BottomPanel.Visible = false;
        }

        public void UpdateMainUI(CollectiveInventory collectiveInventory, GameTime gameTime)
        {
            timeUnitlHealthUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Console.WriteLine(timeUnitlHealthUpdate);
            if (timeUnitlHealthUpdate < 0f) 
            {
                colonists = GameObjectManager.GetObjectsByTag("Colonist").ToArray();
                HealthBars = new ProgressBar[colonists.Length];
                for (int i = 0; i < HealthBars.Length; i++)
                {
                    Colonist colonist = (Colonist)colonists[i];
                    HealthBars[i] = new ProgressBar(0, (uint)colonist.MaxHealth, new Vector2(200, 30), Anchor.CenterRight, new Vector2(0, 100 + 35 * i));
                    TopPanel.AddChild(HealthBars[i]);

                    HealthBars[i].Value = (int)colonist.Health;
                }

                foodLabel.Text = collectiveInventory.TotalFood.ToString();
                timeUnitlHealthUpdate = timeToHealthUpdate;
            }
        }
    }
}
