using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private GameObject bowlingball;
    [SerializeField] private List<Pin> pins = new List<Pin>();
    
    public static event Action ResetRound;
    public static event Action OnStrike;
    public static event Action PinSignal;

    private int throwCount; // so I can reset the pins every two throws instead of just one
    private int pinsFallen;
    private bool ballAlive;
    private void Start()
    {
        ballAlive = true;
        throwCount = 0;
        pinsFallen = 0;
        BowlingBall.OnBallDie += BowlingBallOnOnBallDie;
        // foreach (Pin pin in pins)
        // {
        //     Pin.PinStillStanding += PinOnPinStillStanding;
        // }
        Pin.PinStillStanding += PinOnPinStillStanding;
    }

    private void PinOnPinStillStanding()
    {
        ++pinsFallen;
    }

    private void Update()
    {
        if (throwCount == 1 && pinsFallen == pins.Count && !ballAlive)
        {
            Debug.Log("Pins Fallen: " + pinsFallen);
            Debug.Log("Strike");
            ResetRound?.Invoke();
            resetBall();
            throwCount = 0;
            pinsFallen = 0;
        }
        else if (throwCount == 1 && pinsFallen < pins.Count && !ballAlive)
        {
            Debug.Log("Pins Fallen: " + pinsFallen);
            Debug.Log("Not Strike");
            resetBall();
        }
        else if (throwCount == 2 && pinsFallen == pins.Count && !ballAlive)
        {
            Debug.Log("Pins Fallen: " + pinsFallen);
            Debug.Log("SPARE");
            resetBall();
            throwCount = 0;
            pinsFallen = 0;
        }
        else if (throwCount == 2 && !ballAlive)
        {
            Debug.Log("Pins Fallen: " + pinsFallen);
            Debug.Log("End round");
            ResetRound?.Invoke();
            resetBall();
            throwCount = 0;
            pinsFallen = 0;
        }
    }

    private void OnDestroy()
    {
        BowlingBall.OnBallDie -= BowlingBallOnOnBallDie;
        // foreach (Pin pin in pins)
        // {
        //     pin.PinStillStanding -= PinOnPinStillStanding;
        // }
        Pin.PinStillStanding -= PinOnPinStillStanding;
    }

    private void BowlingBallOnOnBallDie()
    {
        StartCoroutine(DelayThenPinEvent());
        ballAlive = false;
    }

    IEnumerator DelayThenPinEvent()
    {
        yield return new WaitForSeconds(2f);
        ++throwCount;
        PinSignal?.Invoke();
    }

    IEnumerator DelayThenResetBall()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(bowlingball);
        Debug.Log("Throws: " + throwCount);
    }

    void resetBall()
    {
        if (!ballAlive)
        {
            StartCoroutine(DelayThenResetBall());
            ballAlive = true;
        }

    }
}
