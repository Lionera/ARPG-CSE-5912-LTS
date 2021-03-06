using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveTreeUI : MonoBehaviour
{
    [SerializeField] Player player;
    public PassiveSkills passiveSkills;
    public GameObject skillNodes;
    public Connections[] connections;
    public GameObject skillNotification;
    public Text remainingPoints;
    private void Awake()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        passiveSkills = new PassiveSkills(player, connections, skillNodes, skillNotification, remainingPoints, audioManager);
        Debug.Log(passiveSkills == null);
        // assign each child an event listener that listens for button click
        foreach(Transform child in skillNodes.transform)
        {
            Button btn = child.GetComponent<Button>();
            var something = btn.gameObject.transform.Find("Background");
            btn.onClick.AddListener(delegate { TaskOnClick(child.name, btn.gameObject.transform.Find("Background")); });
        }
    }
	void TaskOnClick(string name, Transform background){
        Debug.Log(name);
        passiveSkills.PassivesReadyForUnlock(name, background);
	}
}
