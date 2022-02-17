using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public bool isActive;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private KillingGoal[] killingGoals;
    public QuestScript QuestScriptReference { get; set; }//scipt that controls this quest
   
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }
    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }
    public KillingGoal[] KillingGoals
    {
        get
        {
            return killingGoals;
        }
    }
    /*
     * TODO: we will imply the complete in other places 
     * 
     */
    public void Complete()
    {
        isActive = false;
        Debug.Log(title + " is complete!");
    }

    //public string description;
    //public int expReward;
    //public int goldReward;

    //public QuestGoal goal;
}
