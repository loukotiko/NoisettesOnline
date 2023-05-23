using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class RiverState : INetworkSerializable
{
    public CardsMap Cards = new CardsMap();

    public void FillCards(List<CardState> newCards)
    {
        for (int i = 0; i < Constants.RiverSize; i++)
        {
            if (newCards.Count == 0)
                break;

            if (!Cards.ContainsKey(i) || Cards[i] == null)
            {
                // On dépile newCards et on ajoute le premier élément du tableau à chaque fois.
                Cards.Add(i, newCards.ElementAt(0));
                newCards.RemoveAt(0);
            }
        }
    }

    public CardState ExtractCard(int riverPosition)
    {
        CardState extractedCard = Cards[riverPosition];
        Cards.Remove(riverPosition);
        return extractedCard;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        for (int i = 0; i < Constants.RiverSize; i++)
        {
            var card = (!Cards.ContainsKey(i) || Cards[i] == null) ? null : Cards[i];

            var id = card == null ? "Null" : card.GetCardId();
            serializer.SerializeValue(ref id);

            if (card == null && serializer.IsReader)
            {
                Type type = this.GetType().Assembly.GetType($"{id}CardState");
                card = (CardState)Activator.CreateInstance(type);
            }

            card.NetworkSerialize(serializer);
            Cards[i] = card.GetCardId() == null ? null : card;
        }
    }

    override public string ToString()
    {
        return $"[{Cards.Count}]\nCards: {string.Join(",", Cards)}";
    }
}
