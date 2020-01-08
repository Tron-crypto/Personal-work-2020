using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NNet))]

public class carControls : MonoBehaviour
{

    private Vector3 startPosition, startRotation; // if car crashes its reset
    private NNet network;
    [Range(-1f, 1f)]
    public float a, t;

    public float timeSinceStart = 0f;

    [Header("fitness")]
    public float overallfitness;
    public float distanceMultiplier = 1.4f;
    public float aveSpeedMultiplier = 0.2f;
    public float sensorMultiplier = 0.1f;
    [Header("Network Options")]
    public int LAYERS = 1;
    public int NEURONS = 10;
    private Vector3 lastPosition;
    private float totalDistanceTravelled;
    private float avgSpeed;

    private float aSensor, bSensor, cSensor; //inputs to neural net

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NNet>();

        //test
        network.Initialise(LAYERS, NEURONS);
    }
    public void ResetWithNetwork (NNet net)
    {
        network = net;
        Reset();
    }
    public void Reset()
    {
        //test
        network.Initialise(LAYERS, NEURONS);


        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallfitness = 0f;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Death();
    }

    private void FixedUpdate()
    {
        InputSensors();
        lastPosition = transform.position;

        (a, t) = network.RunNetwork(aSensor, bSensor, cSensor);

        moveCar(a, t);

        timeSinceStart += Time.deltaTime;
        CalculateFitness();

    }

    private void Death ()
    {
        GameObject.FindObjectOfType<Genetic>().Death(overallfitness, network);
    }

    private void CalculateFitness()
    {
        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistanceTravelled / timeSinceStart;

        overallfitness = (totalDistanceTravelled * distanceMultiplier) + (aveSpeedMultiplier) + (((aSensor + bSensor + cSensor) / 3) * sensorMultiplier);

        if(timeSinceStart > 20 && overallfitness < 40)
        {
            Death();
        }
        if (overallfitness >=10000)
        {
            Death();
        }
    }   


    private void InputSensors()
    {
        Vector3 a = (transform.forward + transform.right);
        Vector3 b = (transform.forward);
        Vector3 c = (transform.forward - transform.right);

        Ray r = new Ray(transform.position, a);
        RaycastHit hit;

        if(Physics.Raycast(r,out hit))
        {
            aSensor = hit.distance / 70;
            Debug.DrawLine(r.origin, hit.point, Color.red);
            print("A : " + aSensor);
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit))
        {
            bSensor = hit.distance / 90;
            Debug.DrawLine(r.origin, hit.point, Color.red);
            print("b : " + aSensor);
        }


        r.direction = c;

        if (Physics.Raycast(r, out hit))
        {
            cSensor = hit.distance / 100;
            Debug.DrawLine(r.origin, hit.point, Color.red);
            print("c : " + aSensor);
        }

    }


    private Vector3 inp;
    public void moveCar (float v, float h)
    {
        inp = Vector3.Lerp(Vector3.zero,new Vector3(0, 0, v * 11.4f), 0.02f);
        inp = transform.TransformDirection(inp);
        transform.position += inp;

        transform.eulerAngles += new Vector3(0, (h * 90) * 0.02f, 0);
    }



}
