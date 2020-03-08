﻿using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    /// <summary>
    /// A nice and easy way to create interactables
    /// </summary>
    public static class InteractablesFactory
    {
        public static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, Texture2D[][]> textureSet;

        public static void LoadContent(ContentManager contentManager)
        {
            textures = new Dictionary<string, Texture2D>
            {
                { "farm", contentManager.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0") },
                { "bushH", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0") },
                { "bushN", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1") },
                { "tree", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0") },
                { "treeStump", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2") },
                { "workBench", contentManager.Load<Texture2D>(@"textures/interactables/buildings/workbench") },
                { "stoneNodeSmall", contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_0") },
                { "stoneNodeBig", contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_1") },
                { "tallGrass", contentManager.Load<Texture2D>(@"textures/interactables/environment/grass/tallgrass") },
                { "towerH", contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/hostile_tower") },
                { "towerC", contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/captured_tower") }
            };

            textureSet = new Dictionary<string, Texture2D[][]>
            {
                {
                    "colonist",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist5"),
                        },
                        new Texture2D[]
                        {
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite1"),
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0"),
                        }
                   }
                },
                {
                    "campFire",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_5"),
                        }
                    }
                },
                {
                    "bear",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite4"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite3"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite6"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/attackingBear"),
                        }
                    }
                },
                {
                    "robot",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite6"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite7"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite8"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite9"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0"),
                        },
                        new Texture2D[]
                        {

                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot2"),

                        }
                    }
                },
                {
                    "rabbit",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite0"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite2"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite4"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite7"),
                        }
                    }
                }
            };

        }

        /// <summary>
        /// Create an Interable given a Enum and a position
        /// </summary>
        /// <param name="interactable"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static IInteractable MakeInteractable(Interactable interactable, Vector2 position)
        {
            switch(interactable)
            {
                case Interactable.Colonist:
                    return new Colonist(position, TextureSetTypes.colonist);
                case Interactable.Farm:
                    return new Farm(position, textures["farm"]);
                case Interactable.Rabbit:
                    return new Rabbit(position, TextureSetTypes.rabbit);
                case Interactable.Bush:
                    return new Bush(position, TextureTypes.BushH, TextureTypes.BushN);
                case Interactable.Tree:
                    return new Tree(position, TextureTypes.Tree, TextureTypes.TreeStump);
                case Interactable.WorkBench:
                    return new WorkBench(position, textures["workBench"]);
                case Interactable.StoneNodeSmall:
                    return new SmallStoneNode(position, TextureTypes.SmallStoneNode);
                case Interactable.StoneNodeBig:
                    return new BigStoneNode(position, TextureTypes.BigStoneNode);
                case Interactable.TallGrass:
                    return new TallGrass(position, TextureTypes.TallGrass);
                case Interactable.CampFire:
                    return new CampFire(position, textureSet["campFire"]);
                case Interactable.Robot:
                    return new Enemy("Robot", 5000, 60, 0, 500, position, textureSet["robot"]);
                case Interactable.Bear:
                    return new Enemy("Bear", 1000, 60, 10, 300, position, textureSet["bear"]);
                case Interactable.Tower:
                    return new Tower(position, TextureTypes.TowerC, TextureTypes.TowerH);
                case Interactable.Storage:
                    return new StorageUnit(position);
                default:
                    throw new NotImplementedException(interactable + " has not been implemented");
            }
        }
    }
}

public enum Interactable
{
    Bear,
    Robot,
    CampFire,
    TallGrass,
    StoneNodeSmall,
    StoneNodeBig,
    WorkBench,
    Tree,
    Bush,
    Rabbit,
    Farm,
    Colonist,
    Tower,
    Storage,
}