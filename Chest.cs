using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        LevelChangeController lcc = FindObjectOfType<LevelChangeController>();
        if (lcc != null) {
            bool isChangingScene = lcc.IsChangingScene;
            if (!isChangingScene)
                FindObjectOfType<CheckPointManager>().RegisterConsumable(gameObject.name);
        }
    }
}
