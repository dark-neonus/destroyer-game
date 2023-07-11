using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject personageSettingsPanel;
    
    public void CloseSettings() {
        settingsPanel.SetActive(false);
        personageSettingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
