using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class GridState : INetworkSerializable
{
    public CardState[,] Cards = new CardState[Constants.GridHeight, Constants.GridWidth];

    public GridState()
    {
        Cards = new CardState[Constants.GridHeight, Constants.GridWidth];
    }

    public void FillEmpty()
    {
        Cards = new CardState[Constants.GridHeight, Constants.GridWidth];
        for (int i = 0; i < Constants.GridWidth * Constants.GridHeight; i++)
        {
            int row = i / Constants.GridHeight; // calcul de la ligne
            int col = i % Constants.GridWidth; // calcul de la colonne

            Cards[row, col] = new _0000CardState();
        }
    }

    public void SetCard(CardState card, int row, int col)
    {
        Cards[row, col] = card;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        // Par rapport au reader
        // Vérifie toute la grille (elle est censée être toujours pleine de cartes)

        for (int i = 0; i < Constants.GridWidth * Constants.GridHeight; i++)
        {
            int row = i / Constants.GridHeight; // calcul de la ligne
            int col = i % Constants.GridWidth; // calcul de la colonne

            var card = Cards[row, col];

            var id = card == null ? "_0000" : card.GetCardId();
            serializer.SerializeValue(ref id);

            serializer.SerializeValue(ref id);
            if (card == null && serializer.IsReader)
            {
                Type type = this.GetType().Assembly.GetType($"{id}CardState");
                card = (CardState)Activator.CreateInstance(type);
            }

            card.NetworkSerialize(serializer);
            Cards[row, col] = card;
        }
    }

    override public string ToString()
    {
        string cardsOutput = string.Join(",", Cards.Cast<CardState>());
        return $"[{Cards.Length}]\nCards: {cardsOutput}";
    }
}
