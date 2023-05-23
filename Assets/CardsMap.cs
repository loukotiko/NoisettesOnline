using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsMap : Dictionary<int, CardState>
{
    override public string ToString()
    {
        return $"[${Count}]\nCards: ${string.Join(",", this.Select(x => x.Value).ToList())}";
    }
}
