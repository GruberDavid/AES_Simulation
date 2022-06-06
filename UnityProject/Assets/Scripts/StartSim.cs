using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class StartSim : MonoBehaviour
{
    [Header("StartMenu")]
    public GameObject startMenu;
    public Text pText;
    public Text dText;
    public Text spText;
    public Text shText;
    public Text errText;
    public Toggle toggleSpeed;
    public Toggle toggleObs;

    [Header("SimUI")]
    public GameObject simUI;
    public Text pText2;
    public InputField dIF;
    public InputField spIF;

    public SimController simController;

    public GameObject[] obstacles;
    
    public int population = -1;
    public float distance = -1f;
    public float speed = -1f;
    public SimController.flockingShape shape;
    
    // Starts simulation
    public void StartSimulation()
    {
        bool valid = CheckValues();
        if (valid)
        {
            if(toggleObs.isOn)
            {
                foreach(GameObject obs in obstacles)
                    obs.SetActive(true);
            }
            else
            {
                foreach(GameObject obs in obstacles)
                    obs.SetActive(false);
            }
            startMenu.SetActive(false);
            simController.Init(population, distance, speed, shape, toggleSpeed.isOn);
            pText2.text = pText.text;
            dIF.text = dText.text;
            spIF.text = spText.text;
            simUI.SetActive(true);
        }
        else
            errText.text = "Error : Invalid values";
    }

    // Checks values validity
    bool CheckValues()
    {
        bool pBool = int.TryParse(pText.text, out population);
        if (!pBool) return false;
        bool dBool = float.TryParse(dText.text, NumberStyles.Any, CultureInfo.InvariantCulture, out distance);
        if(!dBool) return false;
        bool spBool = float.TryParse(spText.text, NumberStyles.Any, CultureInfo.InvariantCulture, out speed);
        if(!spBool) return false;


        if ((population<=0) || (distance<=0f) || (speed<=0f))
            return false;
        
        string s = shText.text;
        switch(s)
        {
            case "Quad" :
                shape = SimController.flockingShape.Quad;
                break;
            case "Ring" :
                shape = SimController.flockingShape.Ring;
                break;
            case "Circle" :
                shape = SimController.flockingShape.Circle;
                break;
            case "SolidQuad" :
                shape = SimController.flockingShape.SolidQuad;
                break;
        }
        return true;
    }
}
