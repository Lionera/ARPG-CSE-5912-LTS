using UnityEngine;

namespace LootLabels {
    public class LootGenerator : MonoBehaviour {
        Stats playerStats;
        private void Awake()
        {
            playerStats = GetComponentInParent<GameplayStateController>().GetComponentInChildren<Player>().GetComponent<Stats>();
        }
        /// <summary>
        /// Return a random value for dropped currency
        /// </summary>
        /// <returns></returns>
        public int CalculateCurrencyAmount(LootType type) {
            int randomValue;
            switch (type) {
                case LootType.Poor:
                    randomValue = Random.Range(1, 3);
                    break;
                case LootType.Normal:
                    randomValue = Random.Range(4, 8);
                    break;
                case LootType.Rare:
                    randomValue = Random.Range(10, 16);
                    break;
                case LootType.Epic:
                    randomValue = Random.Range(25, 36);
                    break;
                case LootType.Legendary:
                    randomValue = Random.Range(40, 51);
                    break;
                default:
                    randomValue = 0;
                    break;
            }

            return randomValue;
        }

        /// <summary>
        /// Checks the source of the loot and returns the amount of items it can drop
        /// </summary>
        /// <param name="lootSource"></param>
        /// <returns></returns>
        public int CalculateLootAmount(LootSource lootSource) {
            switch (lootSource) {
                case LootSource.Normal:
                    return Random.Range(2, 4);
                case LootSource.Elite:
                    return Random.Range(4, 7);
                case LootSource.Boss:
                    return Random.Range(7, 11);
                default:
                    Debug.Log("no lootsource");
                    return 0;
            }
        }

        /// <summary>
        /// Check amount of loot types and pick a random one
        /// </summary>
        /// <returns></returns>
        public LootTypes SelectRandomLootType() {
            int lootTypesCount = System.Enum.GetNames(typeof(LootTypes)).Length;
            int randomIndex = Random.Range(0, lootTypesCount);
            return (LootTypes)randomIndex;
        }

        /// <summary>
        /// Check amount of item types and pick a random one
        /// </summary>
        /// <returns></returns>
        public ItemTypes SelectRandomItemType() {
            int itemTypesCount = System.Enum.GetNames(typeof(ItemTypes)).Length;
            int randomIndex = Random.Range(0, itemTypesCount);
            return (ItemTypes)randomIndex;
        }

        /// <summary>
        /// Check amount of gear types and pick a random one
        /// </summary>
        /// <returns></returns>
        public GearTypes SelectRandomGearTypeSeeded(Rarity itemRarity) {
            int randomIndex = Random.Range(0, 8);
            Debug.Log("GearTypes RandomIndex is " + (GearTypes)randomIndex);
            if ((itemRarity == Rarity.Poor || itemRarity == Rarity.Normal))
            {
                 if ((GearTypes)randomIndex == GearTypes.Jewelry)
                {
                    //gearTypeCount = System.Enum.GetNames(typeof(GearTypes)).Length;
                    //Debug.Log("gearTypeCount is " + gearTypeCount);
                    randomIndex = Random.Range(0, 7);
                }
            }
            else if (GearTypesIsPotion(randomIndex))
            {
                while (GearTypesIsPotion(randomIndex))
                {
                    randomIndex = Random.Range(0, 8);
                }
            }

            Debug.Log("GearType chosen: " + (GearTypes)randomIndex);
            return (GearTypes)randomIndex;
        }

        /// <summary>
        /// Check amount of gear types and pick a random one
        /// </summary>
        /// <returns></returns>
        public GearTypes SelectRandomGearTypeUnseeded(Rarity itemRarity)
        {
            int randomIndex;
            //int gearTypeCount = System.Enum.GetNames(typeof(GearTypes)).Length;
            //Debug.Log("gearTypeCount is " + gearTypeCount);
            randomIndex = Random.Range(0, 8);
            Debug.Log("GearTypes RandomIndex is " + (GearTypes)randomIndex);
            if (itemRarity == Rarity.Poor || itemRarity == Rarity.Normal)
            {
                if ((GearTypes)randomIndex == GearTypes.Jewelry)
                {
                    //gearTypeCount = System.Enum.GetNames(typeof(GearTypes)).Length;
                    //Debug.Log("gearTypeCount is " + gearTypeCount);
                    randomIndex = Random.Range(0, 7);
                }
            }
            Debug.Log("GearType chosen: " + (GearTypes)randomIndex + " for item rarity " + itemRarity);
            return (GearTypes)randomIndex;
        }

        /// <summary>
        /// Check amount of rarities pick a random one
        /// </summary>
        /// <returns></returns>
        // <=20 poor Item
        // <= 60 Normal Item
        // <= 80 Rare Item
        // <= 95 Epic Item
        // <= 100 Alpha Item
        public virtual Rarity SelectRandomRarity(LootType type) {
           // int rarityCount = System.Enum.GetNames(typeof(Rarity)).Length;
            
            Rarity rare = Rarity.Poor;
            switch (type)
            {
                case LootType.Poor:
                    if (Random.value < 0.1)
                    {
                        rare = Rarity.Normal;
                    };
                    break;
                case LootType.Normal:
                    if (Random.value < 0.80)
                    {
                        rare = Rarity.Normal;
                    }
                    else if (Random.value > 0.90)
                    {
                        rare = Rarity.Rare;
                    }
                    break;
                case LootType.Rare:
                    rare = Rarity.Rare;
                    break;
                case LootType.Epic:
                    rare = Rarity.Epic;
                    break;
                case LootType.Legendary:
                    rare = Rarity.Legendary;
                    break;
                default:
                    rare = Rarity.Poor;
                    break;
            }
            return rare;
        }

        public virtual Rarity SelectRandomRarityUnseeded()
        {
            float chanceForLegendary = playerStats[StatTypes.LVL] * 0.01f;
            float chanceForEpic = playerStats[StatTypes.LVL] * 0.25f;
            float chanceForRare = playerStats[StatTypes.LVL] * 0.8f;
            float chanceForNormal = playerStats[StatTypes.LVL] * 0.95f;

            Rarity rarity;
            int randomNum = Random.Range(0, 101);
            if (randomNum < chanceForLegendary)
            {
                rarity = Rarity.Legendary;
            }
            else if (randomNum < chanceForEpic)
            {
                rarity = Rarity.Epic;
            }
            else if (randomNum < chanceForRare)
            {
                rarity = Rarity.Rare;
            }
            else if (randomNum < chanceForNormal)
            {
                rarity = Rarity.Normal;
            }
            else
            {
                rarity = Rarity.Poor;
            }

            if ((rarity == Rarity.Poor || rarity == Rarity.Normal) && playerStats[StatTypes.MonsterType] != 1)
            {
                rarity = Rarity.Rare;
            }

            return rarity;
        }

        /// <summary>
        /// Check amount of currency types and pick a random one
        /// </summary>
        /// <returns></returns>
        public CurrencyTypes SelectRandomCurrency() {
            int currencyTypesCount = System.Enum.GetNames(typeof(CurrencyTypes)).Length;
            int randomIndex = Random.Range(0, currencyTypesCount);

            return (CurrencyTypes)randomIndex;
        }

        /// <summary>
        /// Example of a stat roll based on the rarity of an item
        /// </summary>
        /// <param name="itemRarity"></param>
        /// <returns></returns>
        public double RollAmountOfStats(Rarity itemRarity, Ite item, GearTypes gearType) {
            double statAmount = 1;

            switch (itemRarity)
            {
                case Rarity.Poor:
                    statAmount = -2;
                    break;
                case Rarity.Normal:
                    statAmount = 0;
                    break;
                case Rarity.Rare:
                    statAmount = 2;
                    break;
                case Rarity.Epic:
                    statAmount = 4;
                    break;
                case Rarity.Legendary:
                    statAmount = 0;
                    break;
                //case Rarity.Set:
                //    statAmount = 6;
                    //break;
                //case Rarity.SuperUltraHyperExPlusAlpha:
                //    statAmount = 7;
                //    break;
                default:
                    break;
            }

            return statAmount;
        }

        /// <summary>
        /// Create a randomized gear item with a seeded loot type
        /// </summary>
        /// <returns></returns>
        public BaseGear CreateGear(LootType type) {
            Debug.Log("Create Gear with GetModelName has been run");
            Rarity itemRarity = SelectRandomRarity(type);
            GearTypes gearType = SelectRandomGearTypeSeeded(itemRarity);
            string modelName = ResourceManager.singleton.GetModelName(gearType, itemRarity);
            string iconName = ResourceManager.singleton.GetIconName(gearType);

            BaseGear gear = new BaseGear(itemRarity, gearType, modelName, iconName);
            Debug.Log("creategear is making " + itemRarity + " " + modelName);
            return gear;
        }

        /// <summary>
        /// Create a randomized gear item
        /// </summary>
        /// <returns></returns>
        public BaseGear CreateGearUnseeded()
        {
            Debug.Log("Create Gear with GetModelName has been run");
            Rarity itemRarity = SelectRandomRarityUnseeded();
            GearTypes gearType = SelectRandomGearTypeSeeded(itemRarity);
            string modelName = ResourceManager.singleton.GetModelName(gearType, itemRarity);
            string iconName = ResourceManager.singleton.GetIconName(gearType);

            BaseGear gear = new BaseGear(itemRarity, gearType, modelName, iconName);
            Debug.Log("creategear is making " + itemRarity + " " + modelName);
            return gear;
        }

        /// <summary>
        /// Create a randomized currency item
        /// </summary>
        /// <returns></returns>
        public BaseCurrency CreateCurrency(LootType type) {
            CurrencyTypes currencyType = SelectRandomCurrency();
            int amount = CalculateCurrencyAmount(type);
            string modelName = ResourceManager.singleton.GetModelName(currencyType);
            string iconName = ResourceManager.singleton.GetIconName(currencyType);

            BaseCurrency currency = new BaseCurrency(currencyType, amount, modelName, iconName);
            return currency;
        }

        private bool GearTypesIsPotion(int randomIndex)
        {
            return (GearTypes)randomIndex == GearTypes.DefensePotion || (GearTypes)randomIndex == GearTypes.HealthPotion ||
                (GearTypes)randomIndex == GearTypes.ManaPotion || (GearTypes)randomIndex == GearTypes.SpeedPotion ||
                (GearTypes)randomIndex == GearTypes.TeleportPotion;
        }
    }
}