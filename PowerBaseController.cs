using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerBaseController : MonoBehaviour
{
    public Image SourceImage;
    public Text Stacks;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetPower(SpecialPowerUp spu, int stacks) {
        this.SourceImage.sprite = LoadFromMultiple(spu.Icon_Name); //Resources.Load<Sprite>(spu.Icon_Name);

        if (stacks <= 1) {
            Stacks.gameObject.SetActive(false);
        }
        else {
            Stacks.gameObject.SetActive(true);
            Stacks.text = $"x{stacks}";
        }
    }

    public Sprite LoadFromMultiple(string name) {
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Powers");
        return abilityIconsAtlas.Single(s => s.name == name);
    }
}
