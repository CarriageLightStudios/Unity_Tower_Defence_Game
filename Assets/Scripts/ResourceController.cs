using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceController
{
    /*
     * The Resource Controller is responsible for handling all economy in the game.
     * Any time a resource is spend or added it is done through the Resource Controller
     */

    // A collection of the players money.
    private static int _bank;
    public static int bank
    {
        get { return _bank; }
    }
    private static int _resources;
    public static int resources
    {
        get { return _resources; }
    }

    //Add money to the global value
    public static void AddMoney(int value)
    {
        _bank += value;
    }

    public static bool SpendMoney(int value)
    {
        if (value < _bank)
        {
            Debug.LogError("Error: Cannot afford to buy.");
            return false;
        } else
        {
            _bank -= value;
            return true;
        }
    }

    public static void AddResources(int value)
    {
        _resources += value;
    }

    public static bool SpendResources(int value)
    {
        if (value < _resources)
        {
            Debug.LogError("Error: Not enough resources.");
            return false;
        }
        else
        {
            _resources -= value;
            return true;
        }
    }
}
