﻿using Engine;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class GlobalCombatDetector
    {
        public static List<Colonist> colonists=GameObjectManager.Filter<Colonist>().ToList();
        public static List<Enemy> enemies = GameObjectManager.Filter<Enemy>().ToList();
        public static Enemy FindColonistThreat (Colonist col)
        {
            foreach (Enemy enemy in enemies) {

                if (col.AttackRange>Vector2.Distance(enemy.Position,col.Position) && enemy.notDefeated)
                {
                    return enemy;
                }
            }
            return null;
        }


        internal static void UpdateParticipants()
        {
            GameObjectManager.ObjectAdded += ObjectAddedEventHandler;
            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;
        }

        private static void ObjectRemovedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                colonists.Remove(colonist);
            }

            if (GameObject is Enemy enemy)
            {
                enemies.Remove(enemy);
            }
        }

        public static void ObjectAddedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                colonists.Add(colonist);
            }
            if (GameObject is Enemy enemy)
            {
                enemies.Add(enemy);
            }
        }

        public static Colonist GetClosestColonist(Vector2 position)
        {
            Colonist closestColonist = null;
            double shortestDistance = double.MaxValue;

            foreach (Colonist col in colonists)
            {
                double distance = Vector2.Distance(position, col.Position);

                if (shortestDistance > distance) {
                    shortestDistance = distance;
                    closestColonist = col;
                }
            }
            return closestColonist;
        }
    }
}
