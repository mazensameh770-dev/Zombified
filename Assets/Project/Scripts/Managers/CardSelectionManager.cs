using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : Singleton<CardSelectionManager>
{
    private TrapCardUI selectedCard;
    private readonly Dictionary<TrapCardData, TrapCardUI> cardsByData = new Dictionary<TrapCardData, TrapCardUI>();

    public bool HasCardSelected => selectedCard != null;

    public event Action<TrapCardData> OnCardSelected;

    public void RegisterCard(TrapCardUI card)
    {
        cardsByData[card.TrapData] = card;
    }

    public void RestoreOne(TrapCardData trapData)
    {
        if (trapData == null) return;
        if (cardsByData.TryGetValue(trapData, out TrapCardUI card))
        {
            card.AddOne();
        }
    }

    public void SelectCard(TrapCardUI card)
    {
        if (selectedCard == card)
        {
            ClearSelection();
            return;
        }

        if (selectedCard != null) selectedCard.SetSelected(false);

        selectedCard = card;
        selectedCard.SetSelected(true);
        OnCardSelected?.Invoke(card.TrapData);
    }

    public void ClearSelection()
    {
        if (selectedCard != null) selectedCard.SetSelected(false);
        selectedCard = null;
        OnCardSelected?.Invoke(null);
    }

    public void NotifyTrapPlaced()
    {
        if (selectedCard == null) return;

        selectedCard.ConsumeOne();
        if (!selectedCard.HasTrapsRemaining)
        {
            ClearSelection();
        }
        else
        {
            OnCardSelected?.Invoke(selectedCard.TrapData);
        }
    }
}