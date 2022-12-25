using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public string itemName;
    public int numOfRequirements;
    public string Req1;
    public int Req1amount;
    public string Req2;
    public int Req2amount;

    public Blueprint(
        string itemName,
        int numOfRequirements,
        string Req1,
        int Req1amount,
        string Req2,
        int Req2amount
    )
    {
        this.itemName = itemName;
        this.numOfRequirements = numOfRequirements;
        this.Req1 = Req1;
        this.Req1amount = Req1amount;
        this.Req2 = Req2;
        this.Req2amount = Req2amount;
    }
}
