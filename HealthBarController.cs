using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
    public AudioSource AudioSource;
    public GameObject BackBar;
    public Transform PointsBar;
    public GameObject LifePointBase;
    public float BarIncreaseLenght = 10f;
    public float BarBaseLenght = 100f;
    private int currentPoints;
    private int maxPoints;
    //private List<GameObject> LifePoints;

    void Start() {
        //LifePoints = new List<GameObject>();
    }


    void Update() {

    }

    public void SetPoints(int value) {
        maxPoints = value;
        currentPoints = value;
        SetupBar();
    }

    public void AddPoints(int value) {
        maxPoints += value;
        currentPoints += value;
        SetupBar();
        CheckAudio();
    }

    public void ReduceLife(int value) {
        currentPoints -= value;
        if (currentPoints < 0)
            currentPoints = 0;
        SetupBar();
        CheckAudio();
    }

    public void HealPoints(int value) {
        currentPoints += value;
        if (currentPoints > maxPoints)
            currentPoints = maxPoints;
        SetupBar();
        CheckAudio();
    }

    void SetupBar() {
        foreach (Transform child in PointsBar) {
            GameObject.Destroy(child.gameObject);
        }
        SetRight(BackBar.GetComponent<RectTransform>(), BarBaseLenght + (maxPoints * BarIncreaseLenght * -1));
        for (int i = 0; i < currentPoints; i++) {
            GameObject lifePoint = Instantiate(LifePointBase, PointsBar.transform);
            lifePoint.SetActive(true);
            //LifePoints.Add(lifePoint);
        }
        for (int i = 0; i < maxPoints-currentPoints; i++) {
            GameObject lifePoint = Instantiate(LifePointBase, PointsBar.transform);
            lifePoint.SetActive(true);
            lifePoint.GetComponent<Image>().color = Color.gray;
            //LifePoints.Add(lifePoint);
        }
    }

    void CheckAudio() {
        if (currentPoints > 0 && (maxPoints > 1 && currentPoints < 2))
            AudioSource.Play();
        else
            AudioSource.Stop();
    }

    void SetRight(RectTransform rt, float right) {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }
}
