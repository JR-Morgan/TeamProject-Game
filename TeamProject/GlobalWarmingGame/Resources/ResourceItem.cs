﻿using GlobalWarmingGame.Resources;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceItem : IReconstructable
    {
        [PFSerializable]
        public ResourceType ResourceType { get; set; }

        [PFSerializable]
        public int Weight { get; set; }

        public ResourceItem()
        {
        }

        public ResourceItem(ResourceType Type, int weight = 0)
        {
            this.ResourceType = Type;
            this.Weight = weight;
        }

        public ResourceItem(Resource TypeID, int weight = 0)
        : this (ResourceTypeFactory.GetResource(TypeID), weight)
        { }

        public ResourceItem Clone()
        {
            return (ResourceItem)MemberwiseClone();
        }

        public override string ToString() 
        {
            return ResourceType.displayName;                    
        }

        public object Reconstruct()
        {
            return new ResourceItem(ResourceType, Weight);
        }
    }
}
