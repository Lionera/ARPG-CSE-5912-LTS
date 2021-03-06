using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DisplayNodeInfo : MonoBehaviour
{
    private PassiveSkillInfo passiveSkillInfo;
    private PassiveNode[] passiveTree;
    private GameObject text;
    void Start()
    {
        passiveSkillInfo = new PassiveSkillInfo();
        this.passiveTree = passiveSkillInfo.passiveTree;

    }
    public void DisplayInfo()
    {
        PassiveNode passiveNode = Array.Find(passiveTree, node => node.Name == name);
        text = Instantiate(Resources.Load("SkillInfo") as GameObject, transform);
        text.transform.position += new Vector3(0,50,0);
        string info = "Adds " + passiveNode.StatValues[0] + " to " + passiveNode.Stats[0];
        text.GetComponent<Text>().text = info;
    }
    public void RemoveInfo()
    {
        Destroy(text);
    }
}
