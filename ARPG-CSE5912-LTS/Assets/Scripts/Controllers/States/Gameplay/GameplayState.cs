using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using ARPG.Core;
using TMPro;
public class GameplayState : BaseGameplayState
{
    //[SerializeField] private DialogueUI dialogueUI;
    //public DialogueUI DialogueUI => dialogueUI;
    //public IInteractable Interactable { get; set; }

    int groundLayer, npcLayer, enemyLayer;
    Player player;
    PlayerAbilityController playerAbilityController;
    NavMeshAgent agent;
    Animator animator;
    ContextMenuPanel contextMenuPanel;
    UtilityMenuPanel utilityMenuPanel;

    ActionBar actionBar;
    GameObject passiveTreeUI;
    bool lockedActions = false;
    bool tutorialNotSeen = true;

    // Test inventory system

    public override void Enter()
    {
        base.Enter();

        Debug.Log("entered GameplayState");

        gameplayStateController.gameplayUICanvas.enabled = true;
        gameplayStateController.gameplayUICanvasObj.SetActive(true);
        gameplayStateController.npcNamesObj.SetActive(true);
        gameplayStateController.equipmentObj.SetActive(true);

        groundLayer = LayerMask.NameToLayer("Walkable");
        npcLayer = LayerMask.NameToLayer("NPC");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        player = GetComponentInChildren<Player>();
        agent = player.GetComponent<NavMeshAgent>();
        animator = player.GetComponent<Animator>();

        AddButtonListeners();

        contextMenuPanel = gameplayStateController.GetComponentInChildren<ContextMenuPanel>();
        utilityMenuPanel = gameplayStateController.GetComponentInChildren<UtilityMenuPanel>();
        if (contextMenuPanel != null)
        {
            contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        }
        actionBar = gameplayStateController.GetComponentInChildren<ActionBar>();
        playerAbilityController = player.GetComponent<PlayerAbilityController>();
        passiveTreeUI = gameplayStateController.passiveTreeUI;
        passiveTreeUI.SetActive(false);

        gameplayStateController.npcInterfaceObj.SetActive(true);

        gameplayStateController.customCharacter.UpdatePlayerModel(gameplayStateController.playerModel);

        GetComponentInChildren<InteractionManager>().ReactivateNPCS();

        if (tutorialNotSeen)
        {
            TutorialWindow.Instance.text.text = TutorialWindow.Instance.scrollAndQuestTutorial;
            TutorialWindow.Instance.ShowCanvas();
            tutorialNotSeen = false;
        }
        
        var charaPanel = gameplayStateController.GetComponentInChildren<CharacterPanelController>();
        charaPanel.playerInfo = gameplayStateController.customCharacter;
        charaPanel.showCharacterStates();

        LoadSaveData();
    }

    void LoadSaveData()
    {
        var slotNum = gameplayStateController.customCharacter.slotNum;
        var slot = gameplayStateController.saveSlots[slotNum - 1];

        player.GetComponent<Inventory>().LoadSaveData(slot);
        player.GetComponent<EquipManager>().LoadSavedData(slot);        
    }

    void AddButtonListeners()
    {
        gameplayStateController.gameplayUICanvas.enabled = true;
        skillNotificationButton.onClick.AddListener(() => SkillNotificationButtonPressed());
        pauseMenuButton.onClick.AddListener(() => OnPauseMenuClicked());
        charaPanelButton.onClick.AddListener(() => OnCharaPanelClicked());
    }

    void RemoveButtonListeners()
    {
        skillNotificationButton.onClick.RemoveAllListeners();
        pauseMenuButton.onClick.RemoveAllListeners();
        charaPanelButton.onClick.RemoveAllListeners();
    }

    public override void Exit()
    {
        base.Exit();
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        RemoveButtonListeners();

        // Can remove this line to keep gameplay HUD visible while game is paused.
        gameplayStateController.gameplayUICanvas.enabled = false;
    }

    private void OnEnable()
    {
        CastTimerCastType.AbilityBeganBeingCastEvent += OnAbilityBeingCast;
        CastTimerCastType.AbilityCastWasCancelledEvent += OnAbilityWasCancelled;
        CastTimerCastType.AbilityCastTimeWasCompletedEvent += OnAbilityWasCompleted;
    }

    void OnPauseMenuClicked()
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        PauseGame();
        if(FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    void OnCharaPanelClicked()
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        OpenCharacterPanel();
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    protected override void OnClick(object sender, InfoEventArgs<RaycastHit> e)
    {
        //updated to include enemylayer as well
        if (agent.enabled && !playerAbilityController.playerInAOEAbilityTargetSelectionMode)
        {
            player.GetComponent<PlayerController>().PlayerOnClickEventResponse(e.info.collider.gameObject.layer, sender, e);
        }
    }

    protected override void OnClickCanceled(object sender, InfoEventArgs<RaycastHit> e)
    {
        if (agent.enabled)
        {
            player.GetComponent<PlayerController>().PlayerCancelClickEventResponse(sender, e);
        }
        if (playerAbilityController.playerInAOEAbilityTargetSelectionMode)
        {
            playerAbilityController.playerInAOEAbilityTargetSelectionMode = false;
        }
        if (playerAbilityController.playerNeedsToReleaseMouseButton)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
        }
    }

    protected override void OnCancelPressed(object sender, InfoEventArgs<int> e)
    {
        if (contextMenuPanel.contextMenuPanelCanvas.activeSelf || utilityMenuPanel.utilityMenuPanelCanvas.activeSelf)
        {
            utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
            contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        }
        else if (!playerAbilityController.playerInAOEAbilityTargetSelectionMode && !playerAbilityController.playerInSingleTargetAbilitySelectionMode) { 
            PauseGame();
        }
    }

    protected override void OnSecondaryClickPressed(object sender, InfoEventArgs<int> e)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
            utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);

            Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButtonRight);
            if (abilityInSlot != null && !actionBar.actionButtonRight.abilityInSlotOnCooldown && !lockedActions)
            {
                playerAbilityController.playerNeedsToReleaseMouseButton = false;
                player.QueueAbilityCast(abilityInSlot);
            }
        }
    }

    protected override void OnCharacterMenuPressed(object sender, InfoEventArgs<int> e)
    {
        gameplayStateController.ChangeState<CharacterPanelState>();
    }

    protected override void OnStationaryButtonPressed(object sender, InfoEventArgs<int> e)
    {

    }

    protected override void OnStationaryButtonCanceled(object sender, InfoEventArgs<bool> e)
    {

    }

    protected override void OnPotion1Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ite item = actionBar.GetItemOnPotionButton(actionBar.potionButton1);
        if (item != null)
        {
            actionBar.potionButton1.UseItem();
        }
    }

    protected override void OnPotion2Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ite item = actionBar.GetItemOnPotionButton(actionBar.potionButton2);
        if (item != null)
        {
            actionBar.potionButton2.UseItem();
        }
    }

    protected override void OnPotion3Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ite item = actionBar.GetItemOnPotionButton(actionBar.potionButton3);
        if (item != null)
        {
            actionBar.potionButton3.UseItem();
        }
    }

    protected override void OnPotion4Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ite item = actionBar.GetItemOnPotionButton(actionBar.potionButton4);
        if (item != null)
        {
            actionBar.potionButton4.UseItem();
        }
    }

    protected override void OnActionBar1Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton1);
        if (abilityInSlot != null && !actionBar.actionButton1.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar2Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton2);
        if (abilityInSlot != null && !actionBar.actionButton2.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar3Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton3);
        if (abilityInSlot != null && !actionBar.actionButton3.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar4Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton4);
        if (abilityInSlot != null && !actionBar.actionButton4.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar5Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton5);
        if (abilityInSlot != null && !actionBar.actionButton5.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar6Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton6);
        if (abilityInSlot != null && !actionBar.actionButton6.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar7Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton7);
        if (abilityInSlot != null && !actionBar.actionButton7.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar8Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton8);
        if (abilityInSlot != null && !actionBar.actionButton8.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar9Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton9);
        if (abilityInSlot != null && !actionBar.actionButton9.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar10Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton10);
        if (abilityInSlot != null && !actionBar.actionButton10.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar11Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton11);
        if (abilityInSlot != null && !actionBar.actionButton11.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnActionBar12Pressed(object sender, InfoEventArgs<int> e)
    {
        contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
        utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
        Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionBar.actionButton12);
        if (abilityInSlot != null && !actionBar.actionButton12.abilityInSlotOnCooldown && !lockedActions)
        {
            playerAbilityController.playerNeedsToReleaseMouseButton = false;
            player.QueueAbilityCast(abilityInSlot);
        }
    }

    protected override void OnUIElementLeftClicked(object sender, InfoEventArgs<List<RaycastResult>> e)
    {
        //figure out if the results contain an action button
        foreach (RaycastResult result in e.info)
        {
            GameObject go = result.gameObject;
            ActionButton actionButton = go.GetComponent<ActionButton>();
            if (actionButton != null)
            {
                Ability abilityInSlot = actionBar.GetAbilityOnActionButton(actionButton);
                if (abilityInSlot != null)
                {
                    playerAbilityController.playerNeedsToReleaseMouseButton = true;
                    player.QueueAbilityCast(abilityInSlot);
                }
            }
        }
    }

    protected override void OnUIElementRightClicked(object sender, InfoEventArgs<List<RaycastResult>> e)
    {
        Debug.Log("UI element right clicked.");
        //figure out if the results contain an action button
        foreach (RaycastResult result in e.info)
        {
            GameObject go = result.gameObject;
            ActionButton actionButton = go.GetComponent<ActionButton>();
            PotionButton potionButton = go.GetComponentInChildren<PotionButton>();
            //Debug.Log("Gameobject name: "+go.name);
            if (actionButton != null && actionButton != actionBar.actionButtonLeft)
            {
                Debug.Log("Action Button clicked on: " + actionButton.name);
                utilityMenuPanel.utilityMenuPanelCanvas.SetActive(false);
                contextMenuPanel.transform.position = Mouse.current.position.ReadValue();
                contextMenuPanel.transform.position = new Vector3(contextMenuPanel.transform.position.x, 400, contextMenuPanel.transform.position.z);
                contextMenuPanel.contextMenuPanelCanvas.SetActive(true);
                contextMenuPanel.PopulateContextMenu(actionButton);
            }
            if (potionButton != null)
            {
                contextMenuPanel.contextMenuPanelCanvas.SetActive(false);
                utilityMenuPanel.transform.position = Mouse.current.position.ReadValue();
                utilityMenuPanel.transform.position = new Vector3(utilityMenuPanel.transform.position.x, 400, utilityMenuPanel.transform.position.z);
                utilityMenuPanel.utilityMenuPanelCanvas.SetActive(true);
                utilityMenuPanel.PopulateUtilityMenu(potionButton);
            }
        }
    }

    protected override void OnUIElementHovered(object sender, InfoEventArgs<List<RaycastResult>> e)
    {
        //figure out if the raycast results contain an ability
        foreach (RaycastResult result in e.info)
        {
            GameObject go = result.gameObject;
            //Debug.Log("GameObject: " + go.name);
            ActionButton actionButton = go.GetComponent<ActionButton>();
            if (actionButton != null)
            {
                Ability ability = actionButton.abilityAssigned;
                if (ability != null)
                {
                    TipManager.instance.ShowAbilityTooltip(ability);
                }
                else
                {
                    TipManager.instance.HideWindow();
                }
            }
            PotionButton potionButton = go.GetComponent<PotionButton>();
            if (potionButton != null)
            {
                Ite item = potionButton.item;
                if (item != null)
                {
                    TipManager.instance.ShowInventoryTooltip(item);
                }
                else
                {
                    TipManager.instance.HideWindow();
                }
            }
        }
    }

    private void OnAbilityBeingCast(object sender, InfoEventArgs<Ability> e)
    {
        //Debug.Log("Actions locked");
        lockedActions = true;
    }

    private void OnAbilityWasCancelled(object sender, InfoEventArgs<int> e)
    {
        //Debug.Log("Actions unlocked");
        lockedActions = false;
    }

    private void OnAbilityWasCompleted(object sender, InfoEventArgs<AbilityCast> e)
    {
        //Debug.Log("Actions unlocked");
        lockedActions = false;
    }

    void PauseGame()
    {
        gameplayStateController.ChangeState<PauseGameState>();
    }

    void OpenCharacterPanel()
    {
        gameplayStateController.ChangeState<CharacterPanelState>();
    }

    void GameOver()
    {
        gameplayStateController.ChangeState<GameoverState>();
    }
    protected override void OnOpenPassiveTreePressed(object sender, InfoEventArgs<int> e)
    {
        gameplayStateController.ChangeState<PassiveTreeState>();
    }
    void SkillNotificationButtonPressed()
    {
        gameplayStateController.ChangeState<PassiveTreeState>();
    }

    protected override void OnMouseScrollMoved(object sender, InfoEventArgs<float> e)
    {
        if (gameplayStateController.CurrentState.ToString() == "GameplayController (GameplayState)")
        {
            if (e.info > 0) { gameplayStateController.CameraZoom.ZoomIn(); }
            else if (e.info < 0) { gameplayStateController.CameraZoom.ZoomOut(); }
        }
    }

    void Update()
    {
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (animator.GetBool("Dead") == true && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2f && animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            GameOver();
        }
    }
}

