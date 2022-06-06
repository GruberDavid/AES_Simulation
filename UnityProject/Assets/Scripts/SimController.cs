using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABMU;
using ABMU.Core;

public class SimController : AbstractController
{
    [Header("Sim Parameters")]
    public int population = 20;
    public float distance = 20.0f;
    public float velocity = 10.0f;
    public enum flockingShape{
        Quad, 
        Ring,
        Circle,
        SolidQuad
    };
    public flockingShape shape;
    public float radius;
    [Header("Agent")]
    public GameObject agentPrefab;

    // Simulation Controller Initialization
    public void Init(int p, float d, float v, flockingShape s, bool rndSpeed)
    {
        base.Init();

        population = p;
        distance = d;
        velocity = v;
        shape = s;

        List<GameObject> listA = new List<GameObject>();
        // Creation + Agents Initialization based on chosen shape
        switch(shape)
        {
            case flockingShape.Quad :
                CreateQuad(listA, rndSpeed);
                break;
            case flockingShape.Ring :
                CreateRing(listA, rndSpeed);
                break;
            case flockingShape.Circle :
                CreateCircle(listA, rndSpeed);
                break;
            case flockingShape.SolidQuad :
                CreateQuad(listA, rndSpeed);
                break;
            default :
                break;
        }
        // Detect Agents neighbors
        foreach(GameObject a in listA)
        {
            a.GetComponent<AESAgent>().FindNeighbors();
        }
    }

    // Creation + Agents Initialization in quadrilateral shape
    void CreateQuad(List<GameObject> listA, bool rndSpeed)
    {
        for (int i = 0; i < population; i++)
        {
            Vector2 vPos = new Vector2((i%10)*distance, (i/10)*distance);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            GameObject a = Instantiate(agentPrefab, vPos, rot);
            listA.Add(a);
            a.GetComponent<AESAgent>().Init(distance, velocity, rndSpeed);
        }
    }

    // Creation + Agents Initialization in ring shape
    void CreateRing(List<GameObject> listA, bool rndSpeed)
    {
        radius = (population * distance) / Mathf.PI;
        for (int i = 0; i < population; i++)
        {
            float angle = i * Mathf.PI*2f / population;
            Vector2 vPos = new Vector2(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            GameObject a = Instantiate(agentPrefab, vPos, rot);
            listA.Add(a);
            a.GetComponent<AESAgent>().Init(distance, velocity, rndSpeed);
        }
    }

    // Creation + Agents Initialization in circle shape
    void CreateCircle(List<GameObject> listA, bool rndSpeed)
    {
        radius = (population * distance) / Mathf.PI;
        for (int i = 0; i < population; i++)
        {
            float theta = i * 2 * Mathf.PI / population;
            Vector2 vPos = new Vector2(Mathf.Sin(theta)*radius, Mathf.Cos(theta)*radius);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            GameObject a = Instantiate(agentPrefab, vPos, rot);
            listA.Add(a);
            a.GetComponent<AESAgent>().Init(distance, velocity, rndSpeed);
        }
    }
}
