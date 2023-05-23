using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class DeckState : INetworkSerializable
{
    public List<CardState> Cards = new List<CardState>(Constants.DeckSize);

    public void SetCards(List<CardState> NewCards)
    {
        Cards = NewCards;
    }

    public List<CardState> ExtractFirstCards(int count = 1)
    {
        List<CardState> firstCards = Cards.Take(count).ToList();
        Cards.RemoveRange(0, count);
        return firstCards;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        int count = Cards.Count;
        serializer.SerializeValue(ref count);

        if (serializer.IsReader)
            Cards = new List<CardState>(count);

        for (int i = 0; i < count; i++)
        {
            var card = Cards.ElementAtOrDefault(i);

            var id = card == null ? "Null" : card.GetCardId();
            serializer.SerializeValue(ref id);

            if (card == null && serializer.IsReader)
            {
                Type type = this.GetType().Assembly.GetType($"{id}CardState");
                card = (CardState)Activator.CreateInstance(type);
                Cards.Add(card);
            }

            card.NetworkSerialize(serializer);
        }
    }

    override public string ToString()
    {
        return $"[{Cards.Count}]\nCards: {string.Join(",", Cards)}";
    }
}
