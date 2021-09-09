using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetting : MonoBehaviour
{
    
    void Start()
    {
        //NOTE: Two extra lifes for noobs
        if (PlayerPrefs.GetInt("ModoFacil", 0) == 1) {
            CharacterController2D charc = GetComponent<CharacterController2D>();
            charc.AddHealthPoint();
            charc.AddHealthPoint();
        }
    }

    
    void Update()
    {
        
    }
}
