﻿using GlobalWarmingGame.Interactions.Event.Events;
using GlobalWarmingGame.Interactions.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event
{
    /// <summary>
    /// A factory that can create events for clients without them having to know specifics about creation.
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// Create an event given a enum.
        /// </summary>
        /// <param name="eventEnum"></param>
        /// <returns>An event related to the given enum.</returns>
        public static IEvent CreateEvent(Event eventEnum) 
        {
            switch (eventEnum)
            {
                case Event.RobotSiege:
                    return new EventRobotSiege("A force of robots are trying to take the tower.", GameObjectManager.ZoneMap);
                case Event.Colonist:
                    return new EventColonist("A colonist has joined.", GameObjectManager.ZoneMap);
                case Event.Merchant:
                    return new EventMerchant("A merchant has arrived.", GameObjectManager.ZoneMap);
                case Event.BearAttack:
                    return new EventSpawnNPCs($"Some angry {Interactable.Bear}s are nearby.", GameObjectManager.ZoneMap, Interactable.Bear, 3);
                case Event.BanditAmbush:
                    return new EventSpawnNPCs($"{Interactable.Bandit}s have sprung an ambush.", GameObjectManager.ZoneMap, Interactable.Bandit, 4, false, false, 12);
                case Event.SmallRobot:
                    return new EventSpawnNPCs($"{Interactable.SmallRobot}s are scounting nearby.", GameObjectManager.ZoneMap, Interactable.SmallRobot, 3, false, true, 1);
                case Event.Robot:
                    return new EventSpawnNPCs($"{Interactable.Robot}s are patrolling nearby.", GameObjectManager.ZoneMap, Interactable.Robot, 3, false, false, 1);
                case Event.Rabbit:
                    return new EventSpawnNPCs($"A group of {Interactable.Rabbit}s are wondering nearby.", GameObjectManager.ZoneMap, Interactable.Rabbit, 6, true, true);
                default:
                    throw new NotImplementedException(eventEnum + " has not been implemented");
            }
        }
    }

    public enum Event
    {
        RobotSiege,
        BearAttack,
        BanditAmbush,
        SmallRobot,
        Robot,
        Colonist,
        Merchant,
        Rabbit
    }
}
