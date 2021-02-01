using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    //responsible for the health bar
    //spagoot by ReRo
    public HealthScript healthScript;
    public Image fillImage;
    private Slider slider;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float fillValue = (float)healthScript.curHealth / healthScript.maxHealth;
        slider.value = fillValue;
    }
}
