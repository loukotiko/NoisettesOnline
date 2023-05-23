using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class CardState : INetworkSerializable
{
    protected string Uid;

    protected string CardId;

    protected string Name;

    protected string Illustration = "_0000";

    protected string Definition = "Aucun effet";

    protected int InitialStrength = 0;

    protected int Strength = 0;

    protected bool Replacable = false;

    protected CardState()
    {
        Uid = Guid.NewGuid().ToString();
        Strength = InitialStrength;
    }

    public string GetUid()
    {
        return Uid;
    }

    virtual public string GetCardId()
    {
        return CardId;
    }

    virtual public string GetIllustration()
    {
        return Illustration;
    }

    virtual public int GetInitialStrength()
    {
        return InitialStrength;
    }

    virtual public int GetStrength()
    {
        return Strength;
    }

    virtual public bool IsReplacable()
    {
        return Replacable;
    }

    virtual public void AddStrength(int addedStrength)
    {
        Strength += addedStrength;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        serializer.SerializeValue(ref Uid);
        serializer.SerializeValue(ref CardId);
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref Illustration);
        serializer.SerializeValue(ref Definition);
        serializer.SerializeValue(ref InitialStrength);
        serializer.SerializeValue(ref Strength);
        serializer.SerializeValue(ref Replacable);
    }

    override public string ToString()
    {
        return $"{Name} [{CardId}-{Uid}]";
    }
}
