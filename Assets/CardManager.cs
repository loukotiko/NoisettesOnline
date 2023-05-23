using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class CardManager : MonoBehaviour
{
    public GameObject Illustration;
    public GameObject Strength;
    public GameObject Replacable;

    private GameManager GameManager;
    public CardState CardState;
    private SpriteRenderer illustrationSprite;
    private TextMeshPro strengthText;

    public int riverPosition = -1;
    public int gridPositionRow = -1;
    public int gridPositionCol = -1;

    private CardDragScript DragScript;

    public static CardManager CreateCard(CardState cardState)
    {
        CardManager cardManager = null;
        GameObject asset = Resources.Load<GameObject>("Card2d");

        if (asset != null)
        {
            GameObject game = GameObject.Find("Game");

            GameObject Card2d = Instantiate(asset);
            Card2d.SetActive(false);
            cardManager = Card2d.GetComponent<CardManager>();
            cardManager.UpdateCardState(cardState);
        }

        return cardManager;
    }

    void Awake()
    {
        GameManager = GameObject.FindWithTag("Game").GetComponent<GameManager>();

        if (Illustration != null)
            illustrationSprite = Illustration.GetComponent<SpriteRenderer>();

        if (Strength != null)
            strengthText = Strength.GetComponent<TextMeshPro>();
    }

    public void CheckCardState()
    {
        int finalStrength = CardState.GetStrength();
        strengthText.text = "" + finalStrength;

        if (finalStrength > CardState.GetInitialStrength())
            strengthText.color = new Color(0, 255, 0);
        else if (finalStrength < CardState.GetStrength())
            strengthText.color = new Color(255, 0, 0);
        else
            strengthText.color = new Color(255, 255, 255);

        // Modifier l'illustration
        Texture2D texture = Resources.Load<Texture2D>(
            "CardsIllustrations/" + CardState.GetIllustration()
        );
        if (!texture)
            texture = Resources.Load<Texture2D>("CardsIllustrations/_0000");
        illustrationSprite.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        Replacable.SetActive(CardState.IsReplacable());
    }

    public void UpdateCardState(CardState cardState)
    {
        CardState = cardState;
        CheckCardState();
    }

    public void ReplaceCard(CardManager cardToReplace)
    {
        if (!GameManager.SetTurn(this, cardToReplace))
            CancelReplaceCard();
    }

    public void CancelReplaceCard()
    {
        MoveBackToRiver();
    }

    public void AppearInRiver(int position)
    {
        riverPosition = position;
        MoveBackToRiver();
        SetDraggable();
        gameObject.SetActive(true); // Lancer l'animation d'apparition
        CheckCardState();
    }

    public void AppearInGrid()
    {
        AppearInGrid(gridPositionRow, gridPositionRow);
    }

    public void AppearInGrid(int row, int col)
    {
        gridPositionCol = col;
        gridPositionRow = row;
        MoveToGrid(row, col);
        UnsetDraggable();
        gameObject.SetActive(true); // Lancer l'animation d'apparition
        CheckCardState();
    }

    public void ReplaceInGrid(CardManager cardToReplace)
    {
        MoveToGrid(cardToReplace.gridPositionRow, cardToReplace.gridPositionCol);
    }

    public void MoveToGrid(int row, int col)
    {
        // calcul de la position réelle en multipliant les valeurs de ligne et de colonne par la taille d'un sprite
        float xPos = col * Constants.CellWidth;
        float yPos = row * Constants.CellHeight;
        Vector3 position =
            new Vector3(xPos, yPos, 1f) + new Vector3(Constants.GridX, Constants.GridY, 0f);
        transform.position = position;
    }

    public void RemoveFromRiver()
    {
        // Lancer l'animation de disparition ===> ça arrive quand ça ? Probablement jamais.
        Destroy(gameObject);
    }

    public void RemoveFromGrid()
    {
        // Lancer l'animation de disparition ===> ça arrive quand ça ? Probablement jamais.
        Destroy(gameObject);
    }

    public void MoveBackToRiver()
    {
        Vector3 position = GameObject.Find("River" + riverPosition).transform.position;
        position.z = -1;
        transform.position = position;
        CheckCardState();
    }

    public void MoveBackToGrid()
    {
        MoveToGrid(gridPositionRow, gridPositionCol);
    }

    public bool InGrid()
    {
        return gridPositionCol != -1 && gridPositionRow != -1;
    }

    public bool InRiver()
    {
        return riverPosition != -1;
    }

    public void SetDraggable()
    {
        if (!DragScript)
            DragScript = gameObject.AddComponent<CardDragScript>();
    }

    public void UnsetDraggable()
    {
        if (DragScript)
        {
            Destroy(DragScript);
            DragScript = null;
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
