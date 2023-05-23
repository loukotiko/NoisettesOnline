using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Netcode;

public class CardDragScript : MonoBehaviour
{
    private GameManager GameManager;
    private CardManager cardManager;
    private SortingGroup sortingGroup;
    private Vector3 originalScale;
    private float hoverScale = 1.1f;
    private bool isDragging = false;

    private Vector3 mousePositionOffset;

    void Awake()
    {
        GameManager = GameObject.FindWithTag("Game").GetComponent<GameManager>();

        cardManager = GetComponent<CardManager>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        transform.localScale = originalScale * hoverScale;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        sortingGroup.sortingOrder = 1;
        mousePositionOffset = transform.position - GetMousePosition();
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = GetMousePosition();

        CardManager cardUnder = GetCardUnder(mousePosition);

        // Afficher un retour de carte
        /*
                if (locationUnder != null)
                {
                    bool enabledLocation = locationUnder.GetComponent<LocationScript>().Enabled();
                    if (!enabledLocation)
                    {
                        // Show disabled location // can't drop here
                    }
                }
        */
        transform.position = mousePosition + mousePositionOffset;
    }

    private void OnMouseExit()
    {
        if (isDragging)
            return;

        transform.localScale = originalScale;
    }

    private void OnMouseUp()
    {
        sortingGroup.sortingOrder = 0;
        isDragging = false;

        Vector3 mousePosition = GetMousePosition();

        // Check si une carte éthérée est en dessous.
        CardManager cardUnder = GetCardUnder(mousePosition);

        if (cardUnder)
            cardManager.ReplaceCard(cardUnder);
        else
            cardManager.CancelReplaceCard();
    }

    private CardManager GetCardUnder(Vector3 mousePosition)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);

        CardManager cardUnder = null;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                CardManager cardManager = collider.gameObject.GetComponent<CardManager>();
                if (cardManager.CardState.IsReplacable() && cardManager.InGrid())
                {
                    cardUnder = cardManager;
                }
            }
        }
        return cardUnder;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -1;
        return mousePosition;
    }
}
