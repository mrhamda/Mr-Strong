using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

[RequireComponent(typeof(SetCurrentToSoundEffectsVolume))]
[RequireComponent(typeof(AudioSource))]

public class ButtonManagerLobby : MonoBehaviour
{
    [Header("Other")]

    public int currentCost;

    public string currentString;
    public string currentTypeOfItem;
    public GameObject currentInfoBox;
    [Header("Menues")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject shopMenu;
    public GameObject charachetersMenu;
    public GameObject powerMenu;
    public GameObject weaponMenu;
    public GameObject itemsMenu;

    public GameObject confirmBought;

    [Header("UI")]
    public TextMeshProUGUI HtsscoreText;
    public TextMeshProUGUI coinsAmountText;

    [Header("Sliders")]
    public Slider volumeMusicSlider;
    public Slider volumeSoundEffectsSlider;

    [Header("AudioSources")]
    public AudioSource musicAudioSource;

    public Toggle useGraphicToggle;

    public GameObject postProccesingObject;

    [Header("TextsCosts")]
    public TextMeshProUGUI reloderTextCost;
    public TextMeshProUGUI tankTextCost;

    public TextMeshProUGUI handsCostText;
    public TextMeshProUGUI fireBallCostText;
    public TextMeshProUGUI rifleCostText;
    public TextMeshProUGUI swordCostText;

    public TextMeshProUGUI teleporationTextCosts;
    public TextMeshProUGUI hardeningTextCosts;
    public TextMeshProUGUI shieldingTextCosts;
    public TextMeshProUGUI healingTextCosts;
    public TextMeshProUGUI shootAroundTextCosts;
    public TextMeshProUGUI blackHoleBulletTextCosts;

    public TextMeshProUGUI makesEnemiesSlowerTextCosts;
    public TextMeshProUGUI makesPlayerStrongerTextCosts;
    public TextMeshProUGUI makesPlayerFasterTextCosts;
    public TextMeshProUGUI extraLifeChanceTextCosts;
    public TextMeshProUGUI extraCoinsTextCosts;
    public TextMeshProUGUI makesYouRelodFasterPowerTextCosts;
    [Header("Costs")]
    public int reloderCost;
    public int tankCost;

    public int handsCost;
    public int swordCost;
    public int fireballCost;
    public int rifleCost;

    public int healingCost;
    public int hardeningCost;
    public int shieldingCost;
    public int teleporationCost;
    public int shootAroundCost;
    public int blackHoleBulletCost;

    public int makesEnemiesSlowerCost;
    public int makesPlayerStrongerCost;
    public int makesPlayerFasterCost;
    public int extraLifeChanceCosts;
    public int extraCoinsCosts;
    public int makesYouRelodFasterPowerCost;

    [Header("ButtonsText")]
    public TextMeshProUGUI tankTextBuy;
    public TextMeshProUGUI healerTextBuy;
    public TextMeshProUGUI reloderTextBuy;

    public TextMeshProUGUI handsTextBuy;
    public TextMeshProUGUI swordTextBuy;
    public TextMeshProUGUI fireBallTextBuy;
    public TextMeshProUGUI rifleTextBuy;

    public TextMeshProUGUI teleporationTextBuy;
    public TextMeshProUGUI hardeningTextBuy;
    public TextMeshProUGUI shieldingTextBuy;
    public TextMeshProUGUI healingTextBuy;
    public TextMeshProUGUI shootAroundTextBuy;
    public TextMeshProUGUI blackHoleBulletTextBuy;

    public TextMeshProUGUI makesEnemiesSlowerTextBuy;
    public TextMeshProUGUI makesPlayerStrongerTextBuy;
    public TextMeshProUGUI makesPlayerFasterTextBuy;
    public TextMeshProUGUI extraLifeChanceTextBuy;
    public TextMeshProUGUI extraCoinsTextBuy;
    public TextMeshProUGUI makesYouRelodFasterPowerTextBuy;

    [Header("AudioClips")]
    private AudioSource audioSource;
    public AudioClip dontHaveEnoughtMoney;
    public AudioClip bought;
    public AudioClip use;
    public AudioClip sellAudio;

    public string keysPressed;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        IsFirstTime();

        reloderTextCost.text = reloderCost.ToString();
        tankTextCost.text = tankCost.ToString();

        handsCostText.text = handsCost.ToString();
        swordCostText.text = swordCost.ToString();

        fireBallCostText.text = fireballCost.ToString();
        rifleCostText.text = rifleCost.ToString();


        teleporationTextCosts.text = teleporationCost.ToString();
        shieldingTextCosts.text = shieldingCost.ToString();
        hardeningTextCosts.text = hardeningCost.ToString();
        healingTextCosts.text = healingCost.ToString();
        shootAroundTextCosts.text = shootAroundCost.ToString();
        blackHoleBulletTextCosts.text = blackHoleBulletCost.ToString();

        makesEnemiesSlowerTextCosts.text = makesEnemiesSlowerCost.ToString();
        makesPlayerFasterTextCosts.text = makesPlayerFasterCost.ToString();
        extraCoinsTextCosts.text = extraCoinsCosts.ToString();
        extraLifeChanceTextCosts.text = extraLifeChanceCosts.ToString();
        makesPlayerStrongerTextCosts.text = makesPlayerStrongerCost.ToString();
        makesYouRelodFasterPowerTextCosts.text = makesYouRelodFasterPowerCost.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        print(Input.inputString);
        keysPressed += Input.inputString;
        SaveGraphicToogle();
        SaveVolume();

        Text();

        FixButtonsTextsToUseOrBuy();

        AdminCode();

        

    }

    public void FixButtonsTextsToUseOrBuy()
    {
        if (PlayerPrefs.GetString("Tank") == "Owned")
        {
            tankTextBuy.text = "Use";
        }
        else
        {
            tankTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Reloder") == "Owned")
        {
            reloderTextBuy.text = "Use";
        }
        else
        {
            reloderTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Healer") == "Owned")
        {
            healerTextBuy.text = "Use";
        }
        else
        {
            healerTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Hands") == "Owned")
        {
            handsTextBuy.text = "Use";
        }
        else
        {
            handsTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Sword") == "Owned")
        {
            swordTextBuy.text = "Use";
        }
        else
        {
            swordTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Rifle") == "Owned")
        {
            rifleTextBuy.text = "Use";
        }
        else
        {
            rifleTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("FireBall") == "Owned")
        {
            fireBallTextBuy.text = "Use";
        }
        else
        {
            fireBallTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Teleportation") == "Owned")
        {
            teleporationTextBuy.text = "Use";
        }
        else
        {
            teleporationTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Hardening") == "Owned")
        {
            hardeningTextBuy.text = "Use";
        }
        else
        {
            hardeningTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Shielding") == "Owned")
        {
            shieldingTextBuy.text = "Use";
        }
        else
        {
            shieldingTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Healing") == "Owned")
        {
            healingTextBuy.text = "Use";
        }
        else
        {
            healingTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Stronger") == "Owned")
        {
            makesPlayerStrongerTextBuy.text = "Use";
        }
        else
        {
            makesPlayerStrongerTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Faster") == "Owned")
        {
            makesPlayerFasterTextBuy.text = "Use";
        }
        else
        {
            makesPlayerFasterTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("ExtraCoins") == "Owned")
        {
            extraCoinsTextBuy.text = "Use";
        }
        else
        {
            extraCoinsTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("Slower") == "Owned")
        {
            makesEnemiesSlowerTextBuy.text = "Use";
        }
        else
        {
            makesEnemiesSlowerTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("RelodesFaster") == "Owned")
        {
            makesYouRelodFasterPowerTextBuy.text = "Use";
        }
        else
        {
            makesYouRelodFasterPowerTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("ExtraLife") == "Owned")
        {
            extraLifeChanceTextBuy.text = "Use";
        }
        else
        {
            extraLifeChanceTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("BlackHoleShoot") == "Owned")
        {
            blackHoleBulletTextBuy.text = "Use";
        }
        else
        {
            blackHoleBulletTextBuy.text = "Buy";
        }
        if (PlayerPrefs.GetString("ShootAround") == "Owned")
        {
            shootAroundTextBuy.text = "Use";
        }
        else
        {
            shootAroundTextBuy.text = "Buy";
        }
    }
    public void Text()
    {
        HtsscoreText.text = Mathf.RoundToInt(PlayerPrefs.GetFloat("Highscore")).ToString();
        coinsAmountText.text = PlayerPrefs.GetInt("Coins").ToString();

    }
    public void IsFirstTime()
    {
        if (PlayerPrefs.GetString("IsFirstTime") == "")
        {
            volumeMusicSlider.value = 0.5f;
            volumeSoundEffectsSlider.value = 0.5f;

            PlayerPrefs.SetString("Graphic", "On");

            PlayerPrefs.SetFloat("VolumeOfSoundEffects", 0.5f);

            PlayerPrefs.SetString("IsFirstTime", "True");
            PlayerPrefs.SetString("Healer", "Owned");
            PlayerPrefs.SetString("Hands", "Owned");

            PlayerPrefs.SetString("Char", "Healer");
            PlayerPrefs.SetString("Weapon", "Hands");


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


    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);

        ChooseCharachterMenu();

    }

    public void GoBack()
    {
        shopMenu.SetActive(false);
        charachetersMenu.SetActive(false);
        powerMenu.SetActive(false);
        weaponMenu.SetActive(false);
        if (currentInfoBox != null)
        {
            currentInfoBox.SetActive(false);
        }
        itemsMenu.SetActive(false);

    }
    public void ChooseWeaponMenu()
    {
        weaponMenu.SetActive(true);
        powerMenu.SetActive(false);
        charachetersMenu.SetActive(false);
        itemsMenu.SetActive(false);

    }

    public void ChooseItemsMenu()
    {
        weaponMenu.SetActive(false);
        powerMenu.SetActive(false);
        charachetersMenu.SetActive(false);
        itemsMenu.SetActive(true);
    }

    public void ChooseCharachterMenu()
    {
        weaponMenu.SetActive(false);
        powerMenu.SetActive(false);
        itemsMenu.SetActive(false);
        charachetersMenu.SetActive(true);
    }

    public void ChoosePowerMenu()
    {
        weaponMenu.SetActive(false);
        powerMenu.SetActive(true);
        charachetersMenu.SetActive(false);
        itemsMenu.SetActive(false);

    }

    public void InfoButton(GameObject infoBox)
    {
        if (currentInfoBox != null && currentInfoBox != infoBox)
        {
            currentInfoBox.SetActive(false);

        }
        if (PlayerPrefs.GetString("Device") == "Computer")
        {
            infoBox.SetActive(!infoBox.activeSelf);
        }

        currentInfoBox = infoBox;
    }

    public void InfoButtonMobile(GameObject infoBox)
    {
        if (currentInfoBox != null && currentInfoBox != infoBox)
        {
            currentInfoBox.SetActive(false);

        }
        if (PlayerPrefs.GetString("Device") == "Phone")
        {
            infoBox.SetActive(!infoBox.activeSelf);
        }

        currentInfoBox = infoBox;
    }

    public void FindingCost(TextMeshProUGUI costText)
    {
        int cost;
        int.TryParse(costText.text, out cost);

        currentCost = cost;
    }

    public void FindingCurrentItemName(string nameOfItem)
    {
        currentString = nameOfItem;

    }
    public void FindCurrentTypeItem(string typeOfItem)
    {
        currentTypeOfItem = typeOfItem;


    }

    public void Buying()
    {
        if (PlayerPrefs.GetString(currentString) == "Owned")
        {
            PlayerPrefs.SetString(currentTypeOfItem, currentString);
            audioSource.PlayOneShot(use);
            OpenConfirmBuyMenu();

        }
        if (PlayerPrefs.GetInt("Coins") >= currentCost && PlayerPrefs.GetString(currentString) != "Owned")
        {

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - currentCost);
            PlayerPrefs.SetString(currentString, "Owned");
            audioSource.PlayOneShot(bought);
            OpenConfirmBuyMenu();
        }

        if (PlayerPrefs.GetInt("Coins") < currentCost && PlayerPrefs.GetString(currentString) != "Owned")
        {
            audioSource.PlayOneShot(dontHaveEnoughtMoney);
        }
    }
    public void Sell()
    {
        if (PlayerPrefs.GetString(currentString) == "Owned")
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + currentCost / 3);
            PlayerPrefs.SetString(currentString, "");
            audioSource.PlayOneShot(sellAudio);
            if (currentTypeOfItem == "Char")
            {
                PlayerPrefs.SetString(currentTypeOfItem, "Healer");
            }
            else if (currentTypeOfItem == "Weapon")
            {
                PlayerPrefs.SetString(currentTypeOfItem, "Hands");

            }
            else if (currentTypeOfItem == "Power")
            {
                PlayerPrefs.SetString(currentTypeOfItem, "None");

            }

        }
    }
    public void OpenConfirmBuyMenu()
    {
        GameObject noBtn = confirmBought.transform.GetChild(1).gameObject;
        GameObject yesBtn = confirmBought.transform.GetChild(0).gameObject;
        GameObject textTitel = confirmBought.transform.GetChild(2).gameObject;

        confirmBought.SetActive(!confirmBought.activeSelf);

        if (PlayerPrefs.GetString(currentString) != "Owned")
        {
            noBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "No";
            yesBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Yes";
            textTitel.transform.GetComponent<TextMeshProUGUI>().text = "Are you sure you want to buy?";
        }
        else if (PlayerPrefs.GetString(currentString) == "Owned")
        {
            noBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sell";
            yesBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use";
            textTitel.GetComponent<TextMeshProUGUI>().text = "Do you want to use or sell? Selling it give you back only a third of the orginal price";

        }

    }
    public void CloseConfirmBought()
    {
        confirmBought.SetActive(false);
    }

    public void AdminCode()
    {
        if (keysPressed.Contains("ABDULLAH_MOHAMMED_HAMDAN_YEAR_7A"))
        {
            PlayerPrefs.SetInt("Coins", 10000000);
        }

    }
    public void Play()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
