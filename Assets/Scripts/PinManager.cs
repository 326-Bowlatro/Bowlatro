using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private List<Pin> pins = new List<Pin>(); //holds all the pins

    private void Update()
    {
        foreach (Pin pin in pins)
        {
            if (pin.knockedOver)
            {
                // pins.Remove(pin);
            }
        }
    }
    //make god object, when ball dies, send signal to god, then god waits a little bit for pins to settle,
    //then god sends signal to pins to see which are knocked down, pins will send back signal if not knocked down
    //will keep track of which pins are knocked down or not
}
