using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStore : NPC
{

    [Header("Ink JSON")]
    [SerializeField] public Shop shop;
    [SerializeField] public UI_shop shopUI;
    [SerializeField] public UI_Sale saleUI;

    public List<TextAsset> DialogueJSON;
    private int currentStory;
    GameObject child;

    private void Start()
    {
        Player.InteractNPC += Interact;
        child = transform.GetChild(0).gameObject;
        currentStory = 0;
    }

    public override TextAsset GetCurrentDialogue()
    {
        return DialogueJSON[currentStory];
    }

    public override IEnumerator LookAtPlayer()
    {
        float time = 0.0f;
        float speed = 1.0f;
        Quaternion rotate = Quaternion.LookRotation(player.transform.position - child.transform.position);

        while (time < 1.0f)
        {
            child.transform.rotation = Quaternion.RotateTowards(child.transform.rotation, rotate, 50f * Time.deltaTime);
            child.transform.eulerAngles = new Vector3(0, child.transform.eulerAngles.y, 0);
            time += Time.deltaTime * speed;
            yield return null;
        }
    }

    public override void NextStory()
    {
        if (currentStory < DialogueJSON.Count - 1)
        {
            currentStory++;
        }
    }

    protected override void Interact(object sender, EventArgs e)
    {
        if (Interactable())
        {
            if (!hasNewInfo) { InteractionManager.GetInstance().EnterOptionsMenu(); }
            else { InteractionManager.GetInstance().EnterDialogueMode(GetCurrentDialogue()); }
            //else { SetDialogue(); }
            shopUI.initializeShop(shop);

            //SetMenu();
            StartCoroutine(LookAtPlayer());
            
            //InteractionManager.GetInstance().StopInteraction();
            //InteractionManager.GetInstance().DisableInteractionView();
        }
        //shopUI.resetShop();
    }
}
