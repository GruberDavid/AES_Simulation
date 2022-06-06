using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ModifySim : MonoBehaviour
{
    [Header("SimUI parameters")]
    public InputField dIF;
    public InputField spIF;
    public Toggle toggleSpeed;
    public Toggle toggleObs;

    public GameObject[] obstacles;
    
    // Applies new simulation values
    public void ApplyModif()
    {
        // Obstacles On / Off
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

        // Agents update
        foreach(AESAgent a in Resources.FindObjectsOfTypeAll(typeof(AESAgent)) as AESAgent[])
        {
            if(!UpdateValues(a)) return;
        }
    }

    // Updates agent values and returns true if updated
    bool UpdateValues(AESAgent a)
    {
        float dist;
        float speed;

        if(!float.TryParse(dIF.text, NumberStyles.Any, CultureInfo.InvariantCulture, out dist)) 
            return false;
        if(!float.TryParse(spIF.text, NumberStyles.Any, CultureInfo.InvariantCulture, out speed)) 
            return false;

        if(dist != a.distance)
        {
            float prevDist = a.distance;
            a.distance = dist;
            
            SpringJoint2D[] listSJ;
            listSJ = a.gameObject.GetComponents<SpringJoint2D>();

            foreach(SpringJoint2D sj in listSJ)
            {
                float prevSJDist = sj.distance;
                float q = prevDist / dist;
                float sjDist = prevSJDist / q;
                sj.distance = (1.0f + a.noiseOffset) * sjDist;
                a.circleCollider.radius = a.distance/2;
            }
        }

        if(speed != a.velocity)
        {
            if(toggleSpeed.isOn) a.velocity = Random.Range(0.0f, speed);
            else a.velocity = speed;
        }

        return true;
    }
}
