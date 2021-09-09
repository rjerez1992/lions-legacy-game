using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasePowerCard : MonoBehaviour
{
    public Image Icon;
    public Text Title;
    public Text Description;
    private SpecialPowerUp PUReference;
    public bool IsSelected = false;
    public GameObject SelecteEffect;
    public Sprite RareBackground;
    public Image Background;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetPowerUp(SpecialPowerUp pu) {
        this.PUReference = pu;
        this.Title.text = pu.Name;
        this.Description.text = pu.Description;
        //NOTE: File must be directly on Resources folder. Extension not included
        //this.Icon.sprite = Resources.Load<Sprite>(pu.Icon_Name);
        this.Icon.sprite = LoadFromMultiple(pu.Icon_Name);
        if (pu.Rarity == SpecialPowerUp.PowerUpRarity.RARE) {
            Background.sprite = RareBackground;
        }
    }

    public SpecialPowerUp GetPowerUp() {
        return this.PUReference;
    }

    public void SetSelected(bool selected) {
        this.IsSelected = selected;
        this.SelecteEffect.SetActive(this.IsSelected);
    }

    public Sprite LoadFromMultiple(string name) {
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Powers");
        return abilityIconsAtlas.Single(s => s.name == name);
    }
}
