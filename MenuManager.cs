using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //NOTE: Auto-selects the buttons
    public List<Button> Buttons;
    public GameObject DificultadSelector;
    public GameObject MainMenuSelector;
    public List<Button> ButtonsDificulty;
    private AudioSource asrc;

    void Start()
    {
        asrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool hasSelected = false;
        foreach (Button b in Buttons) {
            if (EventSystem.current.currentSelectedGameObject == b.gameObject) {
                hasSelected = true;
                break;
            }
        }
        if (!hasSelected) {
            EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject);
        }
    }

    public void SelectDificulty() {
        asrc.Play();
        MainMenuSelector.SetActive(false);
        DificultadSelector.SetActive(true);
        Buttons = ButtonsDificulty;
    }

    public void StartGame(bool modoFacil) {
        asrc.Play();
        if (modoFacil) {
            PlayerPrefs.SetInt("ModoFacil", 1);
            PlayerPrefs.Save();
        }
        float t = Time.realtimeSinceStartup;
        PlayerPrefs.SetFloat("StartAt", t);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
