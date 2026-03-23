using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseDevice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Device(string deviceName)
    {
        PlayerPrefs.SetString("Device", deviceName);

        SceneManager.LoadScene("Lobby");
    }
}
