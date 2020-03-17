﻿using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Enemies
{
    public class Bear : Enemy
    {
        
        public Bear ( Vector2 position, Texture2D[][] textureSet)
        : base ("Bear",2000, 70, 10, 300, position,textureSet)
        { }

        public override void AnimateAttack()
        {
            isAnimated = true;
            this.TextureGroupIndex = 3;
            
        }

        public override void Update(GameTime gameTime)
        {   
            base.Update(gameTime);
            
            if (this.Health <= 0)
            {
                GameObjectManager.Remove(this);
            }
        }

        protected override void ChaseColonist(Colonist colonist)
        {
            Vector2 fakeLeftXcoordinate = new Vector2(colonist.Position.X - 60, colonist.Position.Y);
            Vector2 fakeRightXcoordinate = new Vector2(colonist.Position.X + 40, colonist.Position.Y);
            if (this.Position.X < colonist.Position.X)
            {
                Goals.Enqueue(fakeLeftXcoordinate);
            }
            else
            {
                Goals.Enqueue(fakeRightXcoordinate);
            }
        }

        internal override void AttackingSound()
        {
            SoundFactory.PlaySoundEffect(Sound.roaringBear);
        }

        internal override void DeathSound()
        {
            SoundFactory.PlaySoundEffect(Sound.bearDying);
        }
        internal override List<ResourceItem> Loot()
        {
            List<ResourceItem> loot = new List<ResourceItem>();
            loot.Add(new ResourceItem(Resource.Food, 2));
            loot.Add(new ResourceItem(Resource.Axe, 2));
            return loot;
        }
    }
}
