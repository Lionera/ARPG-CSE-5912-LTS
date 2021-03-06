namespace LootLabels {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// Base class for gear objects
    /// </summary>
    [System.Serializable]
    public class BaseGear : BaseItem {
        public GearTypes GearType;
        //List of stats, ...

        //public BaseGear() {
        //    ItemType = ItemTypes.Gear;
        //    GearType = GearTypes.Gloves;
        //    ItemRarity = Rarity.Poor;
        //    ItemName = ItemRarity + " " + GearType;
        //    ModelName = "LootLabels/3D models/Gloves1";
        //    IconName = "LootLabels/Icons/UI_Icon_InvGloves";
        //}

        public BaseGear(Rarity rarity, GearTypes gearType, string modelName, string iconName) {
            ItemType = ItemTypes.Gear;
            GearType = gearType;
            ItemRarity = rarity;
            if (gearType == GearTypes.TwoHandedSword)
                ItemName = "Two-Handed Sword";
            else if (gearType == GearTypes.HealthPotion)
                ItemName = "Health Potion";
            else if (gearType == GearTypes.ManaPotion)
                ItemName = "Mana Potion";
            else if (gearType == GearTypes.DefensePotion)
                ItemName = "Defense Potion";
            else if (gearType == GearTypes.SpeedPotion)
                ItemName = "Speed Potion";
            else
                ItemName = GearType.ToString();
            ModelName = modelName;
            IconName = iconName;
            Debug.Log("ItemName is " + ItemName);
        }
    }
}