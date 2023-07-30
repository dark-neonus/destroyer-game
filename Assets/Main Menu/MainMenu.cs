using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject personageSettingsPanel;

    void Start()
    {
        settingsPanel.SetActive(false);
        personageSettingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void StopGame() {
        SceneManager.LoadScene(0);
        Destroy(GameManager.gameManager);
        Destroy(GameManager.gameManager.gameObject);
    }

    public void OpenSettings() {
        settingsPanel.SetActive(true);
        personageSettingsPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void OpenPersonageSettings() {
        settingsPanel.SetActive(false);
        personageSettingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
