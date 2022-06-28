using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class City : MonoBehaviour, IBuilding
{
    SimpleTimer timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = new SimpleTimer(2000, false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
    }

    private void CheckTimer()
    {
        if (timer.Finished)
        {
            ResourceController.AddMoney(10);
            timer = new SimpleTimer(2000, false);
        }
    }
}
