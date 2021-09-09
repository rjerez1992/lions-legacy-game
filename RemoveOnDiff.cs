using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnDiff : MonoBehaviour
{
    public enum RemoveDiff { EASY = 1, NORMAL = 0 }

    public RemoveDiff removeOn = RemoveDiff.NORMAL;

    void Start()
    {
        //NOTE: For noobs
        if (PlayerPrefs.GetInt("ModoFacil", 0) == (int) removeOn) {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
