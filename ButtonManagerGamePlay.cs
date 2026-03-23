using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerGamePlay : MonoBehaviour
{
    [Header("Menues")]
    public GameObject settingsMenu;

    PlayerMovement player;

    [Header("Sliders")]
    public Slider volumeMusicSlider;
    public Slider volumeSoundEffectsSlider;

    [Header("AudioSources")]
    public AudioSource musicAudioSource;

    public Toggle useGraphicToggle;

    public GameObject postProccesingObject;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();

        settingsMenu.SetActive(false);

        volumeMusicSlider.value = PlayerPrefs.GetFloat("VolumeOfMusic");
        volumeSoundEffectsSlider.value = PlayerPrefs.GetFloat("VolumeOfSoundEffects");

        if (PlayerPrefs.GetString("Graphic") == "On")
        {
            useGraphicToggle.isOn = true;
        }
        else if (PlayerPrefs.GetString("Graphic") == "Off")
        {
            useGraphicToggle.isOn = false;
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        SaveGraphicToogle();
        SaveVolume();
    }
    public void Pause()
    {
        player.pauseMenu.SetActive(true);
        player.isPaused = true;
    }
    public void Move(int i)
    {
        print('a');
        if(i == 0)
        {

            player.rb2D.velocity =new Vector2(1 * player.speed,0);
        }else if(i == 1)
        {
            player.rb2D.velocity = new Vector2(-1 * player.speed, 0);

        }
        else if (i == 2)
        {
            player.rb2D.velocity = new Vector2(0, 1 * player.speed);

        }
        else if (i == 3)
        {
            player.rb2D.velocity = new Vector2(0, -1 * player.speed);

        }



    }


    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("VolumeOfMusic", volumeMusicSlider.value);
        PlayerPrefs.SetFloat("VolumeOfSoundEffects", volumeSoundEffectsSlider.value);

        musicAudioSource.volume = PlayerPrefs.GetFloat("VolumeOfMusic");
    }

    public void SaveGraphicToogle()
    {
        if (useGraphicToggle.isOn == true)
        {
            PlayerPrefs.SetString("Graphic", "On");

        }
        else if (useGraphicToggle.isOn == false)
        {
            PlayerPrefs.SetString("Graphic", "Off");

        }

        if (useGraphicToggle.isOn == false)
        {
            postProccesingObject.SetActive(false);
        }
        else
        {
            postProccesingObject.SetActive(true);
        }
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.GetComponent<Animator>().SetBool("ZoomOut", false);
        settingsMenu.SetActive(true);
    }
    public void CloseSettingMenu()
    {
        settingsMenu.GetComponent<Animator>().SetBool("ZoomOut", true);
    }

    public void Resume()
    {
        Time.timeScale = 1;

        player.pauseMenu.GetComponent<Animator>().SetBool("GoOut", true);

        player.isPaused = false;

        StartCoroutine(DisablePauseMenuAfterCertainAmountOfTime(1.02f));
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
        player.isPaused = false;
        Time.timeScale = 1;
    }

    public IEnumerator DisablePauseMenuAfterCertainAmountOfTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.pauseMenu.SetActive(false);
    }

}
