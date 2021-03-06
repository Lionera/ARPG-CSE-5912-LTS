using UnityEngine;

namespace LootLabels {
    /// <summary>
    /// Manager returns the correct path string to the resources folder
    /// </summary>
    public class ResourceManager : MonoBehaviour {

        public static ResourceManager singleton = null;

        void Awake() {
            //Check if instance already exists
            if (singleton == null) {
                //if not, set instance to this
                singleton = this;
            }

            //If instance already exists and it's not this:
            else if (singleton != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Returns the path to the model in the resources folder for the given geartype
        /// </summary>
        /// <param name="gearType"></param>
        /// <returns></returns>
        public string GetModelName(GearTypes gearType, Rarity itemRarity) {
            string modelPath = "LootLabels/3D models/";
            Debug.Log(" in GetModelName gearType is " + gearType);
            int armorTypeCount = System.Enum.GetNames(typeof(ArmorEquipment.armorType)).Length;
            int randomIndex = Random.Range(0, armorTypeCount);
            switch (gearType) {
                //case GearTypes.Gloves:
                //    return modelPath + "Gloves";
                //case GearTypes.Shoulders:
                //    return modelPath + "Shoulders";
                //case GearTypes.Belt:
                //    return modelPath + "Belt";
                //case GearTypes.Shoes:
                //    return modelPath + "Shoes";
                //case GearTypes.Lance:
                //    return modelPath + "Lance";
                case GearTypes.Helm:
                    randomIndex = Random.Range(0, armorTypeCount);
                    Debug.Log("armorTypeCount is " + armorTypeCount);
                    Debug.Log("random index is " + randomIndex);
                    switch (randomIndex)
                    {
                        case 0:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary LightHelm";
                                default:
                                    return modelPath + "LightHelm";
                            }
                        case 1:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary Helm";
                                default:
                                    return modelPath + "Helm";
                            }
                        case 2:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary HeavyHelm";
                                default:
                                    return modelPath + "HeavyHelm";
                            }
                        default:
                            return modelPath + "Helm";

                    }
                    //switch (itemRarity)
                    //{
                    //    //case Rarity.Legendary:
                    //    //    return modelPath + "legendaryDagger";
                    //    default:
                    //        return modelPath + "Helm";
                    //}
                case GearTypes.Armor:
                    randomIndex = Random.Range(0, armorTypeCount);
                    switch (randomIndex)
                    {
                        case 0:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary LightArmor";
                                default:
                                    return modelPath + "LightArmor";
                            }
                        case 1:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary Armor";
                                default:
                                    return modelPath + "Armor";
                            }
                        case 2:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary HeavyArmor";
                                default:
                                    return modelPath + "HeavyArmor";
                            }
                        default:
                            return modelPath + "Armor";

                    }
                    //switch (itemRarity)
                    //{
                    //    //case Rarity.Legendary:
                    //    //    return modelPath + "legendaryDagger";
                    //    default:
                    //        return modelPath + "Armor";
                    //}
                case GearTypes.Boots:
                    randomIndex = Random.Range(0, armorTypeCount);
                    switch (randomIndex)
                    {
                        case 0:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary LightBoots";
                                default:
                                    return modelPath + "LightBoots";
                            }
                        case 1:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary Boots";
                                default:
                                    return modelPath + "Boots";
                            }
                        case 2:
                            switch (itemRarity)
                            {
                                case Rarity.Legendary:
                                    return modelPath + "Legendary Heavy Boots";
                                default:
                                    return modelPath + "Heavy Boots";
                            }
                        default:
                            return modelPath + "Boots";

                    }
                    //switch (itemRarity)
                    //{
                    //    //case Rarity.Legendary:
                    //    //    return modelPath + "legendaryDagger";
                    //    default:
                    //        return modelPath + "Boots";
                    //}
                // return modelPath + "Sword";
                case GearTypes.Dagger:
                    switch (itemRarity)
                    {
                        case Rarity.Legendary:
                            return modelPath + "legendaryDagger";
                        default:
                            return modelPath + "Dagger";
                    }
                   // return modelPath + "Sword";

                case GearTypes.HealthPotion:
                   return modelPath + "HealthPotion";
                case GearTypes.ManaPotion:
                    return modelPath + "ManaPotion";
                case GearTypes.DefensePotion:
                    return modelPath + "DefensePotion";
                case GearTypes.SpeedPotion:
                    return modelPath + "SpeedPotion";
                case GearTypes.Sword:
                    switch (itemRarity)
                    {
                        case Rarity.Legendary:
                            return modelPath + "legendarySword";
                        default:
                            return modelPath + "Sword";
                    }
                case GearTypes.Shield:
                    switch (itemRarity)
                    {
                        case Rarity.Legendary:
                            return modelPath + "legendaryShield";
                        default:
                            return modelPath + "Shield";
                    }
                case GearTypes.TwoHandedSword:
                    switch (itemRarity)
                    {
                        case Rarity.Legendary:
                            return modelPath + "Legendary LongSword";
                        default:
                            return modelPath + "LongSword";
                    }
                case GearTypes.Jewelry:
                    return modelPath + "Jewelry";
                default:
                    Debug.Log("Case not implemented");
                    switch (itemRarity)
                    {
                        case Rarity.Legendary:
                            return modelPath + "legendarySword";
                        default:
                            return modelPath + "Sword";
                    }

                    //return modelPath + "HealthPotion";
            }
        }

        /// <summary>
        /// Returns the path to the model in the resources folder for the given currencyType
        /// </summary>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        public string GetModelName(CurrencyTypes currencyType) {
            string modelPath = "LootLabels/3D models/";

            switch (currencyType) {
                case CurrencyTypes.Gold:
                    return modelPath + "Gold";
                default:
                    Debug.Log("Case not implemented");
                    return modelPath + "Gold";
            }
        }

        /// <summary>
        /// Returns the path to the icon in the resources folder for the given geartype
        /// </summary>
        /// <param name="gearType"></param>
        /// <returns></returns>
        public string GetIconName(GearTypes gearType) {
            string iconPath = "LootLabels/Icons/UI_Icon_";

            switch (gearType) {
                //case GearTypes.Gloves:
                //    return iconPath + "InvGloves";
                //case GearTypes.Shoulders:
                //    return iconPath + "InvShoulders";
                //case GearTypes.Belt:
                //    return iconPath + "InvBelt";
                //case GearTypes.Shoes:
                //    return iconPath + "InvBoots";
                default:
                   // Debug.Log("Case not implemented");
                    return iconPath + "QuestionMark";
            }
        }

        /// <summary>
        /// Returns the path to the icon in the resources folder for the given currencyType
        /// </summary>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        public string GetIconName(CurrencyTypes currencyType) {
            string iconPath = "LootLabels/Icons/UI_Icon_";

            switch (currencyType) {
                case CurrencyTypes.Gold:
                    return iconPath + "Coin";
                default:
                    Debug.Log("Case not implemented");
                    return iconPath + "QuestionMark";
            }
        }
    }
}