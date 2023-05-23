using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class RiverManager : MonoBehaviour
{
    public Dictionary<int, CardManager> Cards;

    void Awake()
    {
        Cards = new Dictionary<int, CardManager>();
    }

    public void CheckChanges(GameState gameState)
    {
        Dictionary<int, CardState> cards = gameState.River.Cards;
        for (var i = 0; i < Constants.RiverSize; i++)
        {
            CardState cardState = cards.ContainsKey(i) ? cards[i] : null;
            CardManager existingCardManager = Cards.ContainsKey(i) ? Cards[i] : null;

            if (cardState == null)
            {
                if (existingCardManager)
                {
                    existingCardManager.RemoveFromRiver();
                }
                continue;
            }

            CardManager cardManager = null;
            if (existingCardManager) // On remplace la carte déjà dans la rivière si c'est la même, sinon on la vire et on rajoute une nouvelle
            {
                if (existingCardManager.CardState.GetUid() == cardState.GetUid())
                {
                    cardManager = existingCardManager;
                    cardManager.UpdateCardState(cardState);
                    //            cardManager.TransformInRiver(i);
                }
                else
                {
                    existingCardManager.RemoveFromRiver();
                }
            }

            if (cardManager == null)
            {
                cardManager = CardManager.CreateCard(cardState);
                cardManager.AppearInRiver(i);
            }

            Cards[i] = cardManager;
        }
    }

    // Update is called once per frame
    void Update() { }
}
