using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticCalculator : MonoBehaviour
{
    public List<GameObject> AppearList;

    public Text PointsText;
    public Text TimeText;
    public Text DiffText;
    public Text Powers;
    public Text Deaths;
    public Text RankLetter;

    private AudioSource asrc;

    void Awake()
    {
        StartCoroutine(DoDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CountHabilities() {
        int count = 0;

        string powersEncoded = PlayerPrefs.GetString("Powers", string.Empty);
        if (powersEncoded != string.Empty) {
            string[] powersList = powersEncoded.Split('|');
            for (int i = 0; i < powersList.Length; i += 2) {
                int powerCount = int.Parse(powersList[i]);
                count += powerCount;
            }
        }

        return count;
    }

    public IEnumerator DoDelay() {
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log("Playing end effect");
        asrc = GetComponent<AudioSource>();
        int score = PlayerPrefs.GetInt("Score", 0);
        int habilities = CountHabilities();
        int deaths = PlayerPrefs.GetInt("Deaths", 0);
        bool easyMode = PlayerPrefs.GetInt("ModoFacil", 0) == 0;
        float StartedAt = PlayerPrefs.GetFloat("StartAt", Time.realtimeSinceStartup);

        float total = score;
        total += (total * habilities * 0.08f);
        total = total / (deaths * 0.35f);
        float elapsedMins = (Time.realtimeSinceStartup - StartedAt) / 60;
        //NOTE: When less than 30 mins, adds points
        float timeAdded = 30 - elapsedMins;
        if (timeAdded < 0)
            timeAdded = 0;
        total += timeAdded;

        if (!easyMode)
            total = total * 1.5f;

        TimeText.text = "Tiempo total: " + ((int)elapsedMins) + " minutos";
        PointsText.text = "Puntaje total: " + ((int)total) + " puntos";
        if (easyMode)
            DiffText.text = "Dificultad: Chikorita (sin bonus)";
        else
            DiffText.text = "Dificultad: Normal (x 1.5 bonus)";
        Powers.text = "Total habilidades: " + habilities;
        Deaths.text = "Total de muertes: " + deaths;

        if (total >= 10000)
            RankLetter.text = "S";
        else if (total >= 8500)
            RankLetter.text = "A";
        else if (total >= 6000)
            RankLetter.text = "B";
        else if (total >= 4000)
            RankLetter.text = "C";
        else if (total >= 2000)
            RankLetter.text = "D";
        else if (total >= 1000)
            RankLetter.text = "E";
        else
            RankLetter.text = "F";

        StartCoroutine(DoRun());
    }

    public IEnumerator DoRun() {
        Debug.Log("Doing end animation");
        foreach (GameObject go in AppearList) {
            Debug.Log("Enabling " + go.name);
            asrc.Play();
            go.SetActive(true);
            yield return new WaitForSecondsRealtime(0.7f);
        }
    }
}
