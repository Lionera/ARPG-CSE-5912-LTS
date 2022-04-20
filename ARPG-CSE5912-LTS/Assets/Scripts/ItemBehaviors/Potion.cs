using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : Ite
{
    public enum potionType
    {
        health,
        mana,
        teleport,
        defense,
        speed,
    }
    ;

    public int mana;
    public int health;
    public int defense;
    public int speed;

    public potionType typeOfPotion;
    public override void Use()
    {
        //Debug.Log("Use potion!");
        base.Use();
        switch ((int)typeOfPotion)
        {
            case (int)potionType.health:
                UseHealingPotion();
                break;
            case (int)potionType.mana:
                UseEnergyPotion();
                break;
            case (int)potionType.defense:
                PotionBehavior.instance.defenseDuration = Time.time + 60;
                PotionBehavior.instance.isDefenseActive = true;

                //PotionBehavior.instance.defenseParticle.Stop();
                var main = PotionBehavior.instance.defenseParticle.main;
                main.duration = PotionBehavior.instance.defenseDuration;
                PotionBehavior.instance.defenseParticle.Play();

                int defensePoints = defense - PotionBehavior.instance.defense;
                PotionBehavior.instance.defense = defense;
                UseDefensePotion(defensePoints);
               // IEnumerator defenseFunc = ApplyDefense(120);
               //PotionBehavior.instance.isDefenseActive = true;
                break;
            case (int)potionType.speed:
                PotionBehavior.instance.speedDuration = Time.time + 60;
                PotionBehavior.instance.isSpeedActive = true;
                int speedPoints = speed - PotionBehavior.instance.speed;
                PotionBehavior.instance.speed = speed;
                UseDefensePotion(speedPoints);
                // IEnumerator defenseFunc = ApplyDefense(120);
                //PotionBehavior.instance.isDefenseActive = true;
                break;
            default:
                Debug.Log("Don't know what this potion does");
                break;
        }
    }

    //IEnumerator ApplyDefense( float seconds)
    //{
    //    Debug.Log("Say something.");
    //    int playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor];
    //    GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor] = playerStat + defense;
    //    yield return new WaitForSeconds(seconds);
    //    GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor] = playerStat - defense;
    //    PotionBehavior.instance.isDefenseActive = false;


    //    Debug.Log("Say something again 2.5 seconds later.");
    //}
    public void UseDefensePotion(int defensePoints)
    {
        int playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor];
        Debug.Log("armor is " + playerStat);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor] = playerStat + defensePoints;
        Debug.Log("new armor after using potion is " + GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Armor]);
    }

    public void UseSpeedPotion(int speedPoints)
    {
        int playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.RunSpeed];
        Debug.Log("speed is " + playerStat);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.RunSpeed] = playerStat + speedPoints;
        Debug.Log("new speed after using potion is " + GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.RunSpeed]);
    }
    public void UseHealingPotion()
    {
       // Debug.Log("ADD HEALTH!!");
        int playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.HP];
        playerStat += health;
        playerStat = Mathf.Clamp(playerStat, 0, GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.MaxHP]);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.HP] = playerStat;
    }
    public void UseEnergyPotion()
    {
       // Debug.Log("ADD Energy!!");

        int playerStat = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Mana];

        playerStat += mana;
        playerStat = Mathf.Clamp(playerStat, 0, GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.MaxMana]);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Stats>()[StatTypes.Mana] = playerStat;
    }
}
