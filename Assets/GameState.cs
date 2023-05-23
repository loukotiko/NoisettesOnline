using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class GameState : INetworkSerializable
{
    public int Turn;
    public GridState Grid;
    public RiverState River;
    public DeckState Deck;

    public GameState()
    {
        Turn = 0;
        Grid = new GridState();
        River = new RiverState();
        Deck = new DeckState();
    }

    public GameState(GameState gameState)
    {
        Turn = gameState.Turn;
        Grid = gameState.Grid;
        River = gameState.River;
        Deck = gameState.Deck;
    }

    public GameState NewGame()
    {
        Turn = 0;

        // Initialiser les zones
        Grid = new GridState();
        River = new RiverState();
        Deck = new DeckState();

        // Mets la grille vide
        Grid.FillEmpty();
        // Sélectionner des cartes au hasard à placer dans le deck
        Deck.SetCards(GetRandomCards());
        // Initialiser la rivière
        River.FillCards(Deck.ExtractFirstCards(Constants.RiverSize));

        return this;
    }

    public GameState ValidateTurn(int riverPosition, int gridPositionRow, int gridPositionCol)
    {
        Turn += 1;
        CardState card = River.ExtractCard(riverPosition);

        CardState replacedCard = Grid.Cards[gridPositionRow, gridPositionCol];

        if (replacedCard != null)
        {
            card.AddStrength(replacedCard.GetStrength());
        }

        Grid.SetCard(card, gridPositionRow, gridPositionCol);

        if (Deck.Cards.Count >= 1)
            River.FillCards(Deck.ExtractFirstCards(1));

        return this;
    }

    private List<CardState> GetRandomCards()
    {
        List<CardState> cards = new List<CardState>(Constants.DeckSize);
        for (int i = 0; i < Constants.DeckSize; i++)
        {
            cards.Insert(
                i,
                (Random.Range(0, 2) == 0) ? new _0001CardState() : new _0002CardState()
            );
        }
        return cards;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        Debug.Log($"Starting {(serializer.IsReader ? "un" : "")}serialize");
        Debug.Log(this);
        Debug.Log($"{(serializer.IsReader ? "Uns" : "S")}erialize Turn : {Turn}.");
        serializer.SerializeValue(ref Turn);

        if (serializer.IsReader)
        {
            if (Grid == null)
                Grid = new GridState();
            if (River == null)
                River = new RiverState();
            if (Deck == null)
                Deck = new DeckState();
        }

        Grid.NetworkSerialize(serializer);
        River.NetworkSerialize(serializer);
        Deck.NetworkSerialize(serializer);
    }

    override public string ToString()
    {
        return $"Turn: {Turn}\n" + $"River: {River}\n" + $"Deck: {Deck}\n" + $"Grid: {Grid}\n";
    }
}
