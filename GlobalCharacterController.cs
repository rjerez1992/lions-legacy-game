using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalCharacterController : MonoBehaviour {
    public GameObject CardsUI;
    public GameObject CardGrid;
    public GameObject BaseCard;

    public GameObject PowersUI;
    public GameObject PowersBase;

    public Text ScoreText;

    public AudioSource AudioSource;
    public AudioClip ChangeAudio;
    public AudioClip SelectAudio;

    private List<SpecialPowerUp> PowerUps;
    private List<GameObject> CreatedCards;
    private bool IsSelecting = false;
    private int CurrentCard = 0;
    public bool HasShadowStep = false;
    public GameObject ShadowPrefab;
    public LayerMask GroundLayer;
    public bool ClearHabilities = false;
    public bool ClearAllPrefs = false;

    private int Score = 0;
    private Tweener scoreTween;


    [System.NonSerialized]
    public List<(int stacks, SpecialPowerUp power)> ObtainedPowers;

    void Start() {
        PowerUps = SpecialPowerUp.AvailablePowerUps();
        ObtainedPowers = new List<(int, SpecialPowerUp)>();
        CardsUI.SetActive(false);
        if (ClearHabilities)
            PlayerPrefs.SetString("Powers", string.Empty);
        if (ClearAllPrefs)
            PlayerPrefs.DeleteAll();
        LoadProperties();
    }

    private void Awake() {
        if (PowerUps != null) {
            //Refreshsed power ups on death
            PowerUps = SpecialPowerUp.AvailablePowerUps();
        }
    }

    private void LoadProperties() {
        LoadHabilities();
        LoadScore();
        //UpdatePowersListUI();
        AddScore(0);
    }

    void Update() {
        if (IsSelecting) {
            if (Input.GetKeyDown(KeyCode.A)) {
                ChangeSelectedCard(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                ChangeSelectedCard(1);
            }
            else if (Input.GetKeyDown(KeyCode.Return)) {
                OnCardSelected();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.R)) {
            GetComponent<LevelChangeController>().ResetLevel();
        }
    }

    public void OnOpenChest() {
        Time.timeScale = 0f;
        CardsUI.SetActive(true);
        IsSelecting = true;
        List<SpecialPowerUp> SelectedPowerUps = RandomPowerUps();
        CreatedCards = new List<GameObject>();

        foreach (Transform child in CardGrid.transform) {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < 3; i++) {
            GameObject go = Instantiate(BaseCard, CardGrid.transform);
            go.SetActive(true);
            go.GetComponent<BasePowerCard>().SetPowerUp(SelectedPowerUps[i]);
            CreatedCards.Add(go);
        }

        CurrentCard = 0;
        CreatedCards[CurrentCard].GetComponent<BasePowerCard>().SetSelected(true);
    }

    private List<SpecialPowerUp> RandomPowerUps() {
        List<SpecialPowerUp> shuffled = PowerUps.OrderBy(x => Random.value).ToList();
        List<SpecialPowerUp> pool = new List<SpecialPowerUp>();
        for (int i = 0; i < 3; i++) {
            SpecialPowerUp.PowerUpRarity rarityTarget;
            if (Random.value >= 0.65f)
                rarityTarget = SpecialPowerUp.PowerUpRarity.RARE;
            else {
                rarityTarget = SpecialPowerUp.PowerUpRarity.NORMAL;
            }
            SpecialPowerUp pu = shuffled.First(x => x.Rarity == rarityTarget);
            pool.Add(pu);
            shuffled.Remove(pu);
        }
        return pool;
    }

    public void ChangeSelectedCard(int dir) {
        CreatedCards[CurrentCard].GetComponent<BasePowerCard>().SetSelected(false);
        CurrentCard += dir;
        AudioSource.PlayOneShot(ChangeAudio);
        if (CurrentCard < 0)
            CurrentCard = 0;
        if (CurrentCard > 2)
            CurrentCard = 2;
        CreatedCards[CurrentCard].GetComponent<BasePowerCard>().SetSelected(true);
    }

    public void OnCardSelected() {
        AudioSource.PlayOneShot(SelectAudio);
        SpecialPowerUp selected = CreatedCards[CurrentCard].GetComponent<BasePowerCard>().GetPowerUp();
        RemoveSelectedSingleUse(selected);
        IsSelecting = false;
        CardsUI.SetActive(false);
        Time.timeScale = 1f;
        UpdatePowersList(selected);
        FindObjectOfType<CharacterController2D>().OnPowerUpSelected(selected);
    }

    private void RemoveSelectedSingleUse(SpecialPowerUp selected) {
        if (selected.IsOneTime) {
            PowerUps.Remove(selected);
        }
    }

    private void UpdatePowersList(SpecialPowerUp selected) {
        if (selected.IsConsumable)
            return;
        bool addedToList = false;
        for (int i = 0; i < ObtainedPowers.Count; i++) {
            if (ObtainedPowers[i].power == selected) {
                ObtainedPowers[i] = (ObtainedPowers[i].stacks + 1, ObtainedPowers[i].power);
                addedToList = true;
                break;
            }
        }
        if (!addedToList) {
            ObtainedPowers.Add((1, selected));
        }
        UpdatePowersListUI();
    }

    private void UpdatePowersListUI() {
        foreach (Transform child in PowersUI.transform) {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < ObtainedPowers.Count; i++) {
            GameObject go = Instantiate(PowersBase, PowersUI.transform);
            go.SetActive(true);
            go.GetComponent<PowerBaseController>().SetPower(ObtainedPowers[i].power, ObtainedPowers[i].stacks);
        }
    }

    public void AddScore(int quantity) {
        Score += quantity;
        int totalDigits = 6;
        //int padding = totalDigits - ((Score + "").Length);
        string targetText = (Score + "").PadLeft(totalDigits, '0');
        if (scoreTween != null)
            scoreTween.Complete();
        scoreTween = ScoreText.DOText(targetText, 1f, true, ScrambleMode.Numerals);
    }

    public void SetShadowStep() {
        HasShadowStep = true;
    }

    public void DoShadowStep() {
        HasShadowStep = false;
        GameObject cc = FindObjectOfType<CharacterController2D>().gameObject;
        cc.SetActive(false);

        Vector2 verticalOffset = new Vector2(0, 10f);
        float[] angles = new float[] { -60f, -50f, 50f, 60f };
        angles = angles.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < angles.Length; i++) {
            Vector2 castDirection = Quaternion.Euler(0, 0, angles[i]) * Vector2.down;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)cc.transform.position + verticalOffset, castDirection, 300f, GroundLayer);

            if (hit && hit.normal.normalized != Vector2.right && hit.normal.normalized != Vector2.left) {
                GameObject go = Instantiate(ShadowPrefab, cc.transform.position, Quaternion.identity);
                Tweener t = go.transform.DOMove(hit.point, 1f);

                t.OnComplete(() => {
                    cc.transform.position = go.transform.position;
                    cc.SetActive(true);
                    Destroy(go);
                });
                break;
            }
        }        
    }

    public void LoadHabilities() {
        string powersEncoded = PlayerPrefs.GetString("Powers", string.Empty);
        //Debug.Log("Loaded from player prefs - " + powersEncoded);

        if (powersEncoded != string.Empty) {
            string[] powersList = powersEncoded.Split('|');
            for (int i = 0; i < powersList.Length; i+= 2) {
                int powerCount = int.Parse(powersList[i]);
                SpecialPowerUp power = PowerUps.First(x => x.Name == powersList[i + 1]);
                for (int j = 0; j < powerCount; j++) {
                    RemoveSelectedSingleUse(power);
                    UpdatePowersList(power);
                    FindObjectOfType<CharacterController2D>().OnPowerUpSelected(power, false);
                }
            }
        }

        //Debug.Log(ObtainedPowers.Count + " powers loaded");
    }

    public void LoadScore() {
        int score = PlayerPrefs.GetInt("Score", -1);
        if (score != -1) {
            this.AddScore(score);
        }
    }

    public void SaveScore() {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.Save();
    }

    public void SaveHabilities() {
        if (ObtainedPowers.Count <= 0)
            return;
        string powersEncoded = string.Empty;
        for (int i = 0; i < ObtainedPowers.Count; i++) {
            powersEncoded += ObtainedPowers[i].stacks + "|" + ObtainedPowers[i].power.Name + "|";
        }
        powersEncoded = powersEncoded.Remove(powersEncoded.Length - 1);
        PlayerPrefs.SetString("Powers", powersEncoded);
        PlayerPrefs.Save();       
    }
}
