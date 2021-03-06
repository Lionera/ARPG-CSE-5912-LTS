using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class BaseMenuState : State
{
    protected MainMenuController mainMenuController;

    // main menu buttons
    public Button startGameButton;
    public Button createCharButton;
    public Button deleteCharButton;
    public Button optionsButton;
    public Button exitGameButton;
    public Button slot1Button, slot2Button, slot3Button, slot4Button, slot5Button, slot6Button;
    public Button yesDeleteButton, noDeleteButton;
    public RectTransform charaScroll;
    public SaveSlot selectedSlot;
    public Button creditsButton, backFromCreditsButton;

    // options menu buttons
    public AudioMixer audioMixer;
    public Button backFromOptionsToMainButton, confirmOptionsButton, resetOptionsButton;
    public Button fullScreenButton, noFullScreenButton;
    public TMP_Dropdown resolutionDropDown;
    public Slider musicVolumeSlider, soundEffectsVolumeSlider;

    // character creation buttons
    public Button backFromCharCreateToMainButton, resetCharaButton, confirmButton;
    public Button maleButton, femaleButton;
    public Button hairForwardButton, hairBackwardButton, hairColorButton;
    public Button eyebrowForwardButton, eyebrowBackwardButton, eyebrowColorButton;
    public Button faceMarkForwardButton, faceMarkBackwardButton, faceMarkColorButton;
    public Button facialHairForwardButton, facialHairBackwardButton, facialHairColorButton;
    public Button eyeColorButton, skinColorButton;
    public InputField nameField;
    public GameObject nameError;


    protected virtual void Awake()
    {
        //main menu
        mainMenuController = GetComponent<MainMenuController>();
        startGameButton = mainMenuController.startGameButtonObj.GetComponent<Button>();
        createCharButton = mainMenuController.createCharButtonObj.GetComponent<Button>();
        deleteCharButton = mainMenuController.deleteCharButtonObj.GetComponent<Button>();
        optionsButton = mainMenuController.optionsButtonObj.GetComponent<Button>();
        exitGameButton = mainMenuController.exitGameButtonObj.GetComponent<Button>();
        noDeleteButton = mainMenuController.noDeleteObj.GetComponent<Button>();
        yesDeleteButton = mainMenuController.yesDeleteObj.GetComponent<Button>();
        charaScroll = mainMenuController.slotContainerObj.GetComponent<RectTransform>();
        creditsButton = mainMenuController.creditsButtonObj.GetComponent<Button>();
        backFromCreditsButton = mainMenuController.backFromCreditsButtonObj.GetComponent<Button>();

        //save slots
        slot1Button = mainMenuController.saveSlotButtonObjs[0].GetComponent<Button>();
        slot2Button = mainMenuController.saveSlotButtonObjs[1].GetComponent<Button>();
        slot3Button = mainMenuController.saveSlotButtonObjs[2].GetComponent<Button>();
        slot4Button = mainMenuController.saveSlotButtonObjs[3].GetComponent<Button>();
        slot5Button = mainMenuController.saveSlotButtonObjs[4].GetComponent<Button>();
        slot6Button = mainMenuController.saveSlotButtonObjs[5].GetComponent<Button>();

        //options
        backFromOptionsToMainButton = mainMenuController.backFromOptionsToMainButtonObj.GetComponent<Button>();
        confirmOptionsButton = mainMenuController.confirmOptionsButtonObj.GetComponent<Button>();
        resetOptionsButton = mainMenuController.resetOptionsButtonObj.GetComponent<Button>();
        resolutionDropDown = mainMenuController.resolutionDropDownObj.GetComponent<TMP_Dropdown>();
        fullScreenButton = mainMenuController.fullScreenButtonObj.GetComponent<Button>();
        noFullScreenButton = mainMenuController.noFullScreenButtonObj.GetComponent<Button>();
        musicVolumeSlider = mainMenuController.musicVolumeSliderObj.GetComponent<Slider>();
        soundEffectsVolumeSlider = mainMenuController.soundEffectsVolumeSliderObj.GetComponent<Slider>();

        //character creation
        backFromCharCreateToMainButton = mainMenuController.backFromCharCreateToMainButtonObj.GetComponent<Button>();
        resetCharaButton = mainMenuController.resetCharaButtonObj.GetComponent<Button>();
        confirmButton = mainMenuController.confirmButtonObj.GetComponent<Button>();
        maleButton = mainMenuController.maleButtonObj.GetComponent<Button>();
        femaleButton = mainMenuController.femaleButtonObj.GetComponent<Button>();
        hairForwardButton = mainMenuController.hairForwardButtonObj.GetComponent<Button>();
        hairBackwardButton = mainMenuController.hairBackwardButtonObj.GetComponent<Button>();
        hairColorButton = mainMenuController.hairColorButtonObj.GetComponent<Button>();
        eyebrowForwardButton = mainMenuController.eyebrowsForwardButtonObj.GetComponent<Button>();
        eyebrowBackwardButton = mainMenuController.eyebrowsBackwardButtonObj.GetComponent<Button>();
        eyebrowColorButton = mainMenuController.eyebrowsColorButtonObj.GetComponent<Button>();
        faceMarkForwardButton = mainMenuController.faceForwardButtonObj.GetComponent<Button>();
        faceMarkBackwardButton = mainMenuController.faceBackwardButtonObj.GetComponent<Button>();
        faceMarkColorButton = mainMenuController.faceColorButtonObj.GetComponent<Button>();
        facialHairForwardButton = mainMenuController.facialHairForwardButtonObj.GetComponent<Button>();
        facialHairBackwardButton = mainMenuController.facialHairBackwardButtonObj.GetComponent<Button>();
        facialHairColorButton = mainMenuController.facialHairColorButtonObj.GetComponent<Button>();
        eyeColorButton = mainMenuController.eyesColorButtonObj.GetComponent<Button>();
        skinColorButton = mainMenuController.skinColorButtonObj.GetComponent<Button>();
        nameField = mainMenuController.nameFieldObj.GetComponent<InputField>();
        nameError = mainMenuController.nameErrorObj;

    }
}
