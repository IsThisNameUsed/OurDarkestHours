using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogResources : MonoBehaviour {

    string[] dial1 = new string[] { "Saloperie de tunnel", "tirons nous d'içi" };
    int[] dial1Order = new int[] { 1, 2 };

    string[] dial2 = new string[] { "", "" };
    int[] dial2Order = new int[] { 2, 1 };

    string aurevoir = "Au revoir";

    public string[] getDialogStrings(String name)
    {
       switch(name)
       {
            case "dial1":
                return dial1;
            case "dial2":
                return dial2;
            default:
                return null;
       }
    }

    public int[] getDialogOrder(String name)
    {
        switch (name)
        {
            case "dial1":
                return dial1Order;
            case "dial2":
                return dial2Order;
            default:
                return null;
        }
    }
}
