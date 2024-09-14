using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;
    

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaved();
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("wyszedlem z gry");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFade(float delay)
    {
        fadeScreen.FadeIn();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
