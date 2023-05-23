using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _0001CardState : CardState
{
    public _0001CardState()
    {
        CardId = "_0001";
        Illustration = "_0000";
        Name = "Bourrin";
        Replacable = Random.Range(0, 100) < 50;
        InitialStrength = 9;
        Strength = 9;
    }
}
