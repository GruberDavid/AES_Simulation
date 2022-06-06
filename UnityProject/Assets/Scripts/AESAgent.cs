using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ABMU;
using ABMU.Core;

public class AESAgent : AbstractAgent
{
    [Header("Sim Controller")]
    [SerializeField]
    private SimController simController;
    [Header("Agent Parameters")]
    public float distance;
    public float noiseOffset;
    public float velocity;
    [Header("Colliders")]
    public CircleCollider2D circleCollider;
    public List<Collider2D> listNeighbors = new List<Collider2D>();

    private Vector3 prevPos;

    // Agent Initialization
    public void Init(float d, float v, bool rndSpeed)
    {
        base.Init();
        
        simController = GameObject.FindWithTag("SimController").GetComponent<SimController>();
        distance = d;
        noiseOffset = Random.Range(-0.01f, 0.01f);
        if(!rndSpeed) velocity = v;
        else velocity = Random.Range(0.0f, v);
        prevPos = gameObject.transform.position;
        
        // Add Move function to scheduler
        CreateStepper(Move, 1, Stepper.StepperQueueOrder.NORMAL);
    }

    // Detect his neighbors
    public void FindNeighbors()
    {
        ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();
        switch(simController.shape)
        {
            case SimController.flockingShape.Quad :
                circleCollider.radius = distance;
                break;
            case SimController.flockingShape.Ring :
                circleCollider.radius = simController.radius;
                break;
            case SimController.flockingShape.Circle :
                circleCollider.radius = simController.radius;
                break;
            case SimController.flockingShape.SolidQuad :
                circleCollider.radius = simController.population * distance;
                break;
        }
        int nbColl = Physics2D.OverlapCollider(circleCollider, contactFilter, listNeighbors);
        
        // Filters colliders list to keep only Agents
        List<Collider2D> filteredList = new List<Collider2D>();
        foreach(Collider2D c in listNeighbors)
        {
            if(c.gameObject.tag == "Agent")
                filteredList.Add(c);
        }
        listNeighbors = filteredList;

        circleCollider.radius = distance/2;

        // Spring Joint 2D creation for each neighbor
        List<SpringJoint2D> listSJ = new List<SpringJoint2D>();
        for (int i = 0; i < listNeighbors.Count; i++)
        {
            SpringJoint2D sj = gameObject.AddComponent<SpringJoint2D>();
            listSJ.Add(sj);
        }
        for (int i = 0; i < listSJ.Count; i++)
        {
            Rigidbody2D rb = listNeighbors[i].gameObject.GetComponent<Rigidbody2D>();
            listSJ[i].connectedBody = rb;
            float dist;
            switch(simController.shape)
            {
                case SimController.flockingShape.Quad :
                    listSJ[i].distance = (1.0f + noiseOffset) * distance;
                    break;
                case SimController.flockingShape.Ring :
                    dist = Vector3.Distance(gameObject.transform.position, rb.gameObject.transform.position);
                    listSJ[i].distance = (1.0f + noiseOffset) * dist;
                    break;
                case SimController.flockingShape.Circle :
                    listSJ[i].distance = (1.0f + noiseOffset) * distance;
                    break;
                case SimController.flockingShape.SolidQuad :
                    dist = Vector3.Distance(gameObject.transform.position, rb.gameObject.transform.position);
                    listSJ[i].distance = (1.0f + noiseOffset) * dist;
                    break;
            }
            listSJ[i].dampingRatio = 0.8f;
            listSJ[i].autoConfigureDistance = false;
            listSJ[i].enableCollision = true;
        }
    }

    // Moves Agent
    void Move()
    {
        Vector3 direction = GetRelativeDirection();
        Quaternion rot = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);
        gameObject.transform.Translate(velocity * Vector3.up * Time.deltaTime);
        prevPos = gameObject.transform.position;
    }
    
    // Returns relative neighbors direction
    Vector3 GetRelativeDirection()
    {
        Vector3 res = Vector3.zero;
        foreach(Collider2D c in listNeighbors)
        {
            res += c.gameObject.transform.position - gameObject.transform.position;
        }
        res += prevPos - gameObject.transform.position;
        res = res / (listNeighbors.Count+1);
        return res;
    }

    
}
