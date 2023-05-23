using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<GameState> GameState = new NetworkVariable<GameState>();

    private GridManager Grid;
    private List<CardManager> Deck;
    private RiverManager River;

    // Turn

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NewGameServerOnly();
        }
        if (IsClient)
        {
            Grid = gameObject.AddComponent<GridManager>();
            Deck = new List<CardManager>(Constants.DeckSize);
            River = gameObject.AddComponent<RiverManager>();

            CheckChanges(null, GameState.Value);
            GameState.OnValueChanged += CheckChanges;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            GameState.OnValueChanged -= CheckChanges;
        }
    }

    void NewGameServerOnly()
    {
        GameState.Value = new GameState().NewGame();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ValidateTurnServerRpc(int riverPosition, int gridPositionRow, int gridPositionCol)
    {
        GameState.Value = new GameState(GameState.Value).ValidateTurn(
            riverPosition,
            gridPositionRow,
            gridPositionCol
        );
    }

    private void CheckChanges(GameState previousGameState, GameState newGameState)
    {
        if (newGameState != null)
        {
            River.CheckChanges(newGameState);
            Grid.CheckChanges(newGameState);
        }
    }

    public bool SetTurn(CardManager card, CardManager cardToReplace)
    {
        if (!cardToReplace.CardState.IsReplacable())
            return false;

        ValidateTurnServerRpc(
            card.riverPosition,
            cardToReplace.gridPositionRow,
            cardToReplace.gridPositionCol
        );

        return true;
    }

    void DisableRiver()
    {
        // Réactiver la rivière
        foreach (var card in River.Cards)
        {
            card.Value.UnsetDraggable();
        }
    }

    void EnableRiver()
    {
        // Réactiver la rivière
        foreach (var card in River.Cards)
        {
            card.Value.SetDraggable();
        }
    }
}
