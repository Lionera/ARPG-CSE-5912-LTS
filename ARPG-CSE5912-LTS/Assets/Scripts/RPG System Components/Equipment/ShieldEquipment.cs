using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment/Shield")]
public class ShieldEquipment : Equipment
{
    public int armor;
    public int blockChance;
    public override void Use()
    {
        base.Use();
        //GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>().SetValue(StatTypes.PHYDEF, armor, false);
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
