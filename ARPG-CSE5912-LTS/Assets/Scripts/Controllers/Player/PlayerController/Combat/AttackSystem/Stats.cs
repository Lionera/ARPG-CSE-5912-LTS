using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script should be attached to all entities that require stats
public class Stats : MonoBehaviour
{
    ////Temp variables for testing
    //public float health;
    //public float maxHealth;
    //public float attackDmg;
    //public float attackSpd;
    //public float attackDuration;
    public int this[StatTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    public int[] _data = new int[(int)StatTypes.Count];

    //Could add events here for checking whether a stat will change, and whether a stat did change, in case something else should modify it first


    public float GetValue(StatTypes type)
    {
        return this[type];
    }

    //Assign an updated value to a stat
    public void SetValue(StatTypes type, int value, bool allowExceptions)
    {
        int oldValue = this[type];
        if (oldValue == value)
            return;

        if (allowExceptions)
        {
            // Get the exception to the rule here and maybe post event that it is going to change, then get the modified value
        }

        _data[(int)type] = value;

        // maybe broadcast an event that the value was changed if necessary
    }

    //Assign the initial value for a stat, use this to ensure you don't accidentally allow exceptions when setting up characters
    public void InitializeValue(StatTypes type, int value)
    {
        float oldValue = this[type];
        if (oldValue == value)
            return;

        _data[(int)type] = value;
    }
}
