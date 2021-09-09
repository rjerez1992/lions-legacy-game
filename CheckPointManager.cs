using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private List<CheckPoint> CheckPoints;
    private CheckPoint EnabledCP;
    private List<string> ConsumableNames;

    void Start()
    {
        CheckPoints = new List<CheckPoint>(FindObjectsOfType<CheckPoint>());
        LoadConsumablesList();
        

        string cp = PlayerPrefs.GetString("CheckPoint", string.Empty);
        Debug.Log("Loaded checkpoint " + cp);
        if (cp != string.Empty) {
            CheckPoint target = CheckPoints.Find(x => x.name == cp);
            if (target != null) {
                target.ActivateEffect.SetActive(true);
                target.IsActive = true;
                EnabledCP = target;
                FindObjectOfType<CharacterController2D>().transform.position = target.transform.position;
                RemoveConsumables();
            }
            else {
                //NOTE: Clears pref because it might be deleted
                PlayerPrefs.SetString("CheckPoint", string.Empty);
            }
        }
    }

    void Update()
    {
        
    }

    internal void SetEnabled(CheckPoint checkPoint) {
        Debug.Log("Enabling checkpoint " + checkPoint.name);
        if (EnabledCP != null) {
            EnabledCP.OnDisable();
        }
        EnabledCP = checkPoint;
        PlayerPrefs.SetString("CheckPoint", checkPoint.name);
        SaveSkillsAndConsumables();
    }

    public void RegisterConsumable(string name) {
        if(!ConsumableNames.Contains(name))
            ConsumableNames.Add(name);
    }

    public void SaveRegisteredConsumables() {
        string consums = String.Join("|", ConsumableNames.ToArray());
        PlayerPrefs.SetString("Consumed", consums);
        PlayerPrefs.Save();
    }

    public void LoadConsumablesList() {
        string consums = PlayerPrefs.GetString("Consumed", string.Empty);
        ConsumableNames = new List<string>();
        string[] consumables = consums.Split('|');
        foreach (string s in consumables) {
            ConsumableNames.Add(s);
        }
    }

    public void RemoveConsumables() {
        //NOTE: Slow as fuck
        foreach (string s in ConsumableNames) {
            GameObject go = GameObject.Find(s);
            if (go != null)
                Destroy(go);
        }
    }

    public void SaveSkillsAndConsumables() {
        GetComponent<GlobalCharacterController>().SaveHabilities();
        GetComponent<GlobalCharacterController>().SaveScore();
        SaveRegisteredConsumables();
    }

    public void OnLevelChange() {
        PlayerPrefs.SetString("Consumed", string.Empty);
        PlayerPrefs.SetString("CheckPoint", string.Empty);
    }

}
