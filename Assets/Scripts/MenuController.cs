using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private GameObject _aboutWindow;

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void AboutButtonClicked()
    {
        _menuWindow.SetActive(false);
        _aboutWindow.SetActive(true);
    }

    public void BackButtonClicked()
    {
        _menuWindow.SetActive(true);
        _aboutWindow.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

}
