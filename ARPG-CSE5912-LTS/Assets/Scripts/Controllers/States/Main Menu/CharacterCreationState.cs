using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattleDrakeStudios.ModularCharacters;

public class CharacterCreationState : BaseMenuState
{
    private CharacterCustomizer characterManager;
    public CustomCharacter customCharacter;
    private bool initialized = false;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered character creation menu state");
        mainMenuController.createCharMenuCanvas.enabled = true;
        mainMenuController.characterCreationCamera.enabled = true;
        if (characterManager == null)
        {
            characterManager = new CharacterCustomizer(mainMenuController.customCharacterObj.GetComponent<ModularCharacterManager>());
        }
        customCharacter = mainMenuController.charaScriptableObj;
        nameError.SetActive(false);
        SetUpButtons();
    }

    void SetUpButtons()
    {
        backFromCharCreateToMainButton.onClick.AddListener(() => OnBackButtonClicked());
        resetCharaButton.onClick.AddListener(() => ResetCharacter());
        confirmButton.onClick.AddListener(() => OnConfirmButtonClicked());
        maleButton.onClick.AddListener(() => SetGenderMale());
        femaleButton.onClick.AddListener(() => SetGenderFemale());
        hairForwardButton.onClick.AddListener(() => SetHairStyle(SelectionDirection.Forward));
        hairBackwardButton.onClick.AddListener(() => SetHairStyle(SelectionDirection.Backward));
        hairColorButton.onClick.AddListener(() => SetHairColor());
        eyebrowForwardButton.onClick.AddListener(() => SetEyebrowsStyle(SelectionDirection.Forward));
        eyebrowBackwardButton.onClick.AddListener(() => SetEyebrowsStyle(SelectionDirection.Backward));
        eyebrowColorButton.onClick.AddListener(() => SetEyebrowsColor());
        faceMarkForwardButton.onClick.AddListener(() => SetFacialMarkStyle(SelectionDirection.Forward));
        faceMarkBackwardButton.onClick.AddListener(() => SetFacialMarkStyle(SelectionDirection.Backward));
        faceMarkColorButton.onClick.AddListener(() => SetFacialMarkColor());
        facialHairForwardButton.onClick.AddListener(() => SetFacialHairStyle(SelectionDirection.Forward));
        facialHairBackwardButton.onClick.AddListener(() => SetFacialHairStyle(SelectionDirection.Backward));
        facialHairColorButton.onClick.AddListener(() => SetFacialHairColor());
        eyeColorButton.onClick.AddListener(() => SetEyeColor());
        skinColorButton.onClick.AddListener(() => SetSkinColor());
        nameField.onEndEdit.AddListener(s => SetName(s));

        ChangeEyebrowButton();
        ChangeEyeButton();
        ChangeFacialHairButton();
        ChangeFacialMarkButton();
        ChangeHairButton();
        ChangeSkinButton();
    }

    void RemoveButtonListeners()
    {
        backFromCharCreateToMainButton.onClick.RemoveAllListeners();
        resetCharaButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
        maleButton.onClick.RemoveAllListeners();
        femaleButton.onClick.RemoveAllListeners();
        hairForwardButton.onClick.RemoveAllListeners();
        hairBackwardButton.onClick.RemoveAllListeners();
        hairColorButton.onClick.RemoveAllListeners();
        eyebrowForwardButton.onClick.RemoveAllListeners();
        eyebrowBackwardButton.onClick.RemoveAllListeners();
        eyebrowColorButton.onClick.RemoveAllListeners();
        faceMarkForwardButton.onClick.RemoveAllListeners();
        faceMarkBackwardButton.onClick.RemoveAllListeners();
        faceMarkColorButton.onClick.RemoveAllListeners();
        facialHairForwardButton.onClick.RemoveAllListeners();
        facialHairBackwardButton.onClick.RemoveAllListeners();
        facialHairColorButton.onClick.RemoveAllListeners();
        eyeColorButton.onClick.RemoveAllListeners();
        skinColorButton.onClick.RemoveAllListeners();
        nameField.onEndEdit.RemoveAllListeners();
    }

    public override void Exit()
    {
        base.Exit();
        RemoveButtonListeners();
        mainMenuController.createCharMenuCanvas.enabled = false;
        mainMenuController.characterCreationCamera.enabled = false;
    }

    void OnBackButtonClicked()
    {
        ResetCharacter();
        mainMenuController.ChangeState<MainMenuRootState>();
    }

    void OnConfirmButtonClicked()
    {
        Debug.Log("Confirm Button Clicked!");

        if (characterManager.NameIsValid())
        {
            Debug.Log("Now entering game");
            GetCharacterDetails();
            SetDefaultOutfit();
            nameError.SetActive(false);
            int slotNum = FindEmptySlot();
            customCharacter.slotNum = slotNum;
            mainMenuController.saveSlotDataObjs[slotNum].containsData = true;
            mainMenuController.saveSlotDataObjs[slotNum].characterData.CopyCharacterData(customCharacter);
            selectedSlot = mainMenuController.saveSlotDataObjs[slotNum];
            Debug.Log("Slot " + selectedSlot.slotNumber + " should have data: " + selectedSlot.containsData);
            GenerateNewDungeons();
            LoadingStateController.Instance.InitalizeGameScene();
        }
        else
        {
            Debug.Log("Error: need a name!");
            nameError.SetActive(true);
        }
        PlayAudio();
    }


    void GenerateNewDungeons()
    {
        mainMenuController.dungeon1.generated = false;
        mainMenuController.dungeon2.generated = false;
        mainMenuController.dungeon3.generated = false;
    }

    int FindEmptySlot()
    {
        for (int i = 0; i < mainMenuController.saveSlotDataObjs.Count; i++)
        {
            if (!mainMenuController.saveSlotDataObjs[i].containsData)
            {
                return i;
            }
        }
        return -1; //should never be here
    }

    void SetGenderMale()
    {
        characterManager.SetGender(Gender.Male);
        Debug.Log("Player gender set to Male");
        PlayAudio();
    }

    void SetGenderFemale()
    {
        characterManager.SetGender(Gender.Female);
        Debug.Log("Player gender set to Female");
        PlayAudio();
    }

    void SetHairStyle(SelectionDirection d)
    {
        characterManager.SetHairStyle(d);
        Debug.Log("Player hair style changed");
        PlayAudio();
    }

    void SetHairColor()
    {
        Debug.Log("Player hair color changed");
        characterManager.SetHairColor();
        ChangeHairButton();
        PlayAudio();
    }

    void ChangeHairButton()
    {
        var colors = hairColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.Hair);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.Hair);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.Hair);
        hairColorButton.colors = colors;
    }

    void SetEyebrowsStyle(SelectionDirection d)
    {
        Debug.Log("Player eyebrow style changed");
        characterManager.SetEyebrowStyle(d);
        PlayAudio();
    }

    void SetEyebrowsColor()
    {
        Debug.Log("Player eyebrow color changed");
        characterManager.SetEyebrowColor();
        ChangeEyebrowButton();
        PlayAudio();
    }

    void ChangeEyebrowButton()
    {
        var colors = eyebrowColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.Eyebrows);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.Eyebrows);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.Eyebrows);
        eyebrowColorButton.colors = colors;
    }

    void SetFacialMarkStyle(SelectionDirection d)
    {
        Debug.Log("Player face mark color changed");
        characterManager.SetFaceMarkStyle(d);
        PlayAudio();
    }

    void SetFacialMarkColor()
    {
        Debug.Log("Player face mark color changed");
        characterManager.SetFaceMarkColor();
        PlayAudio();
        ChangeFacialMarkButton();
    }

    void ChangeFacialMarkButton()
    {
        var colors = faceMarkColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.FaceMark);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.FaceMark);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.FaceMark);
        faceMarkColorButton.colors = colors;
    }

    void SetFacialHairStyle(SelectionDirection d)
    {
        Debug.Log("Player facial hair style changed");
        characterManager.SetFacialHairStyle(d);
        PlayAudio();
    }

    void SetFacialHairColor()
    {
        Debug.Log("Player facial hair color changed");
        characterManager.SetFacialHairColor();
        ChangeFacialHairButton();
        PlayAudio();
    }

    void ChangeFacialHairButton()
    {
        var colors = facialHairColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.FacialHair);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.FacialHair);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.FacialHair);
        facialHairColorButton.colors = colors;
    }

    void SetEyeColor()
    {
        Debug.Log("Player eye color changed");
        characterManager.SetEyeColor();
        ChangeEyeButton();
        PlayAudio();
    }

    void ChangeEyeButton()
    {
        var colors = eyeColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.Eyes);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.Eyes);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.Eyes);
        eyeColorButton.colors = colors;
    }

    void SetSkinColor()
    {
        Debug.Log("Player skin color changed");
        characterManager.SetSkinColor();
        ChangeSkinButton();
        PlayAudio();
    }

    void ChangeSkinButton()
    {
        var colors = skinColorButton.colors;
        colors.normalColor = characterManager.GetPartColor(BodyPartNames.Skin);
        colors.selectedColor = characterManager.GetPartColor(BodyPartNames.Skin);
        colors.highlightedColor = characterManager.GetPartColor(BodyPartNames.Skin);
        skinColorButton.colors = colors;
    }

    void SetName(string n)
    {
        Debug.Log("Player name changed to " + n);
        characterManager.SetCharacterName(n);
    }


    void SetDefaultOutfit()
    {
        customCharacter.SetDefaultOutfit(characterManager.GetDefaultOutfitIds());
    }

    void ResetCharacter()
    {
        Debug.Log("Player character reset");
        characterManager.ResetParts();

        ChangeEyebrowButton();
        ChangeEyeButton();
        ChangeFacialHairButton();
        ChangeFacialMarkButton();
        ChangeHairButton();
        ChangeSkinButton();
    }

    void PlayAudio()
    {
        FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    void GetCharacterDetails()
    {
        var hairId = characterManager.GetPartID(BodyPartNames.Hair);
        var hairColor = characterManager.GetPartColor(BodyPartNames.Hair);

        var faceMarkID = characterManager.GetPartID(BodyPartNames.FaceMark);
        var facemarkColor = characterManager.GetPartColor(BodyPartNames.FaceMark);

        var facialHairID = characterManager.GetPartID(BodyPartNames.FacialHair);
        var facialHairColor = characterManager.GetPartColor(BodyPartNames.FacialHair);

        var eyebrowID = characterManager.GetPartID(BodyPartNames.Eyebrows);
        var eyebrowColor = characterManager.GetPartColor(BodyPartNames.Eyebrows);

        var eyeColor = characterManager.GetPartColor(BodyPartNames.Eyes);
        var skinColor = characterManager.GetPartColor(BodyPartNames.Skin);

        var gender = characterManager.CharacterGender;
        var charName = characterManager.CharacterName;

        customCharacter.UpdateIds(hairId, eyebrowID, faceMarkID, facialHairID);
        customCharacter.UpdateColors(hairColor, eyebrowColor, facemarkColor, facialHairColor, eyeColor, skinColor);
        customCharacter.UpdateGender(gender);
        customCharacter.UpdateName(charName);
    }

}