using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment/Weapon")]

public class WeaponEquipment : Equipment
{
    public int attackRange;
    public int minimumDamage;
    public int maximumDamage;
    public int attackSpeed;
    public int critChance;
    public enum weaponType
    {
        twohandsword,
        righthandsword,
        lefthandsword,
        bow,
        staff,
        dagger,
    };

    public weaponType typeOfWeapon;

    public void OnEnable()
    {
        //playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>();
    }


    public override void Use()
    {
        //NOTE: Weapons don't add to PHYATK stat. Instead, PHYATK will come from putting points in it via the Passive Tree and/or by leveling up
        //      Instead, the BasicAttackDamageAbilityEffect will read the weapon's minimum and maximum damage numbers and make calculations with them
        //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.PHYATK] += damage;
        base.Use();
       // Debug.Log("playerStat before change is " + playerStat[StatTypes.PHYATK]);
        //playerStat.SetValue(StatTypes.PHYATK, damage, false);
       // playerStat[StatTypes.PHYATK] += damage;
       // Debug.Log("playerStat after change is " + playerStat[StatTypes.PHYATK]);

        //switch (typeOfWeapon)
        //{
        //    case weaponType.twohandsword:
        //        animcontroller.ChangeToTwoHandedSword();
        //        //playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>();
        //        //UseHealingPotion();
        //        break;
        //    case weaponType.righthandsword:
        //        GameObject nu = (GameObject)Instantiate(prefab);
        //        nu.transform.parent = rightHand.transform;
        //        animcontroller.ChangeToOnlySwordRight();
        //        //playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>();
        //        //UseEnergyPotion();
        //        break;
        //    default:
        //        Debug.Log("Don't know what this weapon does");
        //        break;
        //}
    }
}
