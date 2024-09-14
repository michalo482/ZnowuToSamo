using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End screen")] 
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    public UI_ItemTooltip itemTooltip;

    public UIStatTooltip statTooltip;

    public UI_CraftWindow craftWindow;
    public UI_SkillTooltip skillTooltip;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //itemTooltip = GetComponentInChildren<UI_ItemTooltip>();
        //SwitchTo(null);
        SwitchTo(inGameUI);
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    public void SwitchTo(GameObject menu)
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!isFadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                return;
            }
        }
        
        SwitchTo(inGameUI);
    }

    public void SwitchToEndScreen()
    {
        //SwitchTo(null);
        
        fadeScreen.FadeIn();

        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        endText.SetActive(true);
        
        yield return new WaitForSeconds(1);
        
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void SaveAndQuit()
    {
        SaveManager.instance.SaveGame();
        Application.Quit();
    }

}
