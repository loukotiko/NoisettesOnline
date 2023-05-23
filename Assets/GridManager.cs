using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    private CardManager[,] Cards;

    void Awake()
    {
        Cards = new CardManager[Constants.GridWidth, Constants.GridHeight];
    }

    public void CheckChanges(GameState gameState)
    {
        for (int i = 0; i < Constants.GridWidth * Constants.GridHeight; i++)
        {
            int row = i / Constants.GridHeight; // calcul de la ligne
            int col = i % Constants.GridWidth; // calcul de la colonne

            if (Cards[row, col])
            {
                // Remove (delete?) previous card
                Destroy(Cards[row, col].gameObject);
            }

            // TODO: Si une carte est déjà présente, la réutiliser (comme sur RiverManager)

            CardManager cardManager = CardManager.CreateCard(gameState.Grid.Cards[row, col]);
            Cards[row, col] = cardManager;
            cardManager.AppearInGrid(row, col);
        }
    }

    // Update is called once per frame
    void Update() { }
}
