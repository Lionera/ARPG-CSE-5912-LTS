using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PauseGameState : BaseGameplayState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Game paused");
        Time.timeScale = 0;
        gameplayStateController.pauseMenuCanvas.enabled = true;
        gameplayStateController.npcInterfaceObj.SetActive(false);
        gameplayStateController.equipmentObj.SetActive(false);
        if (FindObjectOfType<AudioManager>() != null)
        {
            foreach (Sound s in FindObjectOfType<AudioManager>().sounds)
            {
                if (!s.name.Contains("BGM")) s.source.Stop();
            }
        }

        AddButtonListeners();
    }

    void AddButtonListeners()
    {
        resumeGameButton.onClick.AddListener(() => OnResumeGameClicked());
        inGameOptionsButton.onClick.AddListener(() => OnOptionsClicked());
        exitToMainMenuButton.onClick.AddListener(() => OnExitToMenuClicked());
        exitGameButton.onClick.AddListener(() => OnExitGameClicked());
    }

    public override void Exit()
    {
        base.Exit();
        RemoveButtonListeners();
        gameplayStateController.pauseMenuCanvas.enabled = false;
        gameplayStateController.npcInterfaceObj.SetActive(true);
        gameplayStateController.equipmentObj.SetActive(true);
        Time.timeScale = 1;
    }

    void RemoveButtonListeners()
    {
        resumeGameButton.onClick.RemoveAllListeners();
        inGameOptionsButton.onClick.RemoveAllListeners();
        exitToMainMenuButton.onClick.RemoveAllListeners();
        exitGameButton.onClick.RemoveAllListeners();
    }

    void OnResumeGameClicked()
    {
        ResumeGame();
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    void OnOptionsClicked()
    {
        gameplayStateController.ChangeState<OptionsGameplayState>();
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    void OnExitToMenuClicked()
    {
        SetPlayerSpawn();
        gameplayStateController.ChangeState<GameplayState>();
        gameplayStateController.gameplayUICanvas.enabled = false;

        gameplayStateController.gameplayUICanvasObj.SetActive(false);
        gameplayStateController.npcNamesObj.SetActive(false);
        gameplayStateController.equipmentObj.SetActive(false);

        Time.timeScale = 1;
        LoadingStateController.Instance.LoadScene("MainMenu");
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");
    }

    void SetPlayerSpawn()
    {
        var player = gameplayStateController.GetComponentInChildren<PlayerController>();
        if (player != null)
        {
            Debug.Log("Reset Player Location in Game");
            player.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            player.gameObject.transform.position = new Vector3(198.5f, 9.6f, 206.32f);
            player.gameObject.GetComponent<NavMeshAgent>().enabled = true;

            player.DungeonNum = 0;
        }
    }

    void OnExitGameClicked()
    {
        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("MenuClick");

        Application.Quit();
    }

    protected override void OnClick(object sender, InfoEventArgs<RaycastHit> e)
    {

    }

    protected override void OnClickCanceled(object sender, InfoEventArgs<RaycastHit> e)
    {

    }

    protected override void OnCancelPressed(object sender, InfoEventArgs<int> e)
    {
        ResumeGame();
    }

    void ResumeGame()
    {
        gameplayStateController.ChangeState<GameplayState>();
    }
}
