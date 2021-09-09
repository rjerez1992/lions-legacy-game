using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeController : MonoBehaviour
{
    public string NextLevelName = "Tutorial";
    public GameObject FinishUI;
    public bool AutoChange = true;
    public bool IsChangingScene = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowFinishText() {
        //TODO: Fade in
        //TODO: Add win sound upon finishing
        Time.timeScale = 0f;
        FinishUI.SetActive(true);
        if(AutoChange)
            StartCoroutine(AutoChangeRoutine());
    }

    public void NextLevel() {
        IsChangingScene = true;
        Time.timeScale = 1f;
        GetComponent<GlobalCharacterController>().SaveHabilities();
        GetComponent<GlobalCharacterController>().SaveScore();
        GetComponent<CheckPointManager>().OnLevelChange();
        SceneManager.LoadScene(NextLevelName);
    }

    private IEnumerator AutoChangeRoutine() {
        yield return new WaitForSecondsRealtime(10f);
        NextLevel();
    }

    public void ResetLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
