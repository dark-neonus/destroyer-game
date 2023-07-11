using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersongeSettings : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject personageSettingsPanel;
    
    public void ClosePersonageSettings() {
        settingsPanel.SetActive(false);
        personageSettingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
