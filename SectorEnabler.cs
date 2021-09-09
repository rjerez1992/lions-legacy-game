using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorEnabler : MonoBehaviour
{
    public Transform Target;
    public float EnablingDistance;
    public float EnablingInterval = 0.75f;
    private GlobalMusicController gmc;
    public float PresentationDistance = 15f;
    public float StartTime = 0f;

    public Color[] backColors;
    public Color[] titleColors;
    public Color[] flavourColors;

    void Start()
    {
        if (Target == null)
            Target = FindObjectOfType<CharacterController2D>().transform;
        InvokeRepeating("RunCheck", StartTime, EnablingInterval);
        gmc = FindObjectOfType<GlobalMusicController>();
    }

    void Update()
    {
        
    }

    public void RunCheck() {
        foreach (Transform child in transform) {
            if (child != null) {
                if (DistanceFromTarget(child) <= EnablingDistance) {
                    child.gameObject.SetActive(true);
                    if (DistanceFromTarget(child) <= PresentationDistance) {
                        CheckForPresentation(child.gameObject);
                    }
                }
                else {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckForPresentation(GameObject go) {
        string name = go.name;
        if (name.StartsWith("AlexSniper") && RequiresPresentation("AlexSniper")) {
            StartCoroutine(gmc.RunPresentation(go, "ALEXSNIPER", "APUNTA.", "DISPARA.", "GG EZ.", titleColors[0], flavourColors[0], backColors[0]));
        }
        else if (name.StartsWith("DiegoPlane") && RequiresPresentation("DiegoPlane")) {
            StartCoroutine(gmc.RunPresentation(go, "DIEGOVION", "TENGO", "UNAS", "BOMBAS!", titleColors[1], flavourColors[1], backColors[1]));
        }
        else if (name.StartsWith("FerCompliance") && RequiresPresentation("FerCompliance")) {
            StartCoroutine(gmc.RunPresentation(go, "FERTIFICATION", "ACASO", "ESTÁ", "CERTIFICADO?", titleColors[2], flavourColors[2], backColors[2]));
        }
        else if (name.StartsWith("RexLurker") && RequiresPresentation("RexLurker")) {
            StartCoroutine(gmc.RunPresentation(go, "NOT REX", "DEFINITIVAMENTE", "NO ES", "REX", titleColors[3], flavourColors[3], backColors[3]));
        }
        else if (name.StartsWith("RobertCar") && RequiresPresentation("RobertCar")) {
            StartCoroutine(gmc.RunPresentation(go, "ROBERRRRRT", "LA MAQUINA MAS", "VELOZ DE", "TOTA ITALIA!", titleColors[4], flavourColors[4], backColors[4]));
        }
        else if (name.StartsWith("RocioBug") && RequiresPresentation("RocioBug")) {
            StartCoroutine(gmc.RunPresentation(go, "BUG-CHIO", "BUGS,", "BUGS,", "Y... BUGS.", titleColors[5], flavourColors[5], backColors[5]));
        }
        else if (name.StartsWith("VictorWarden") && RequiresPresentation("VictorWarden")) {
            StartCoroutine(gmc.RunPresentation(go, "EL GUARDIAN", "TIENE LA", "LLAVE A", "TU CORAZON <3", titleColors[6], flavourColors[6], backColors[6]));
        }
        else if (name.StartsWith("VictorZen") && RequiresPresentation("VictorZen")) {
            StartCoroutine(gmc.RunPresentation(go, "TRUE-PANA", "¡¿QUE", "DICE", "LIONS?!", titleColors[7], flavourColors[7], backColors[7]));
        }
        else if (name.StartsWith("Tronsbonk") && RequiresPresentation("Tronsbonk")) {
            StartCoroutine(gmc.RunPresentation(go, "TR0NZB0NK", "DE AQUI", "NO", "PASAS", titleColors[5], flavourColors[6], backColors[5], true));
        }
    }

    private bool RequiresPresentation(string partialKey) {
        string fullKey = "Meet" + partialKey;
        int meet = PlayerPrefs.GetInt(fullKey, 0);
        if (meet == 0) {
            PlayerPrefs.SetInt(fullKey, 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }


    private float DistanceFromTarget(Transform t) {
        return Vector2.Distance(Target.position, t.position);
    }
}
