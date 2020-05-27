using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanels : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        panel.SetActive(false);
    }
}
