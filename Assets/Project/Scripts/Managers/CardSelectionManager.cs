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
        if (card == null || card.TrapData == null) return;

        cardsByData[card.TrapData] = card;
        UpdateCardCountFromLevel(card);
    }

    public void UpdateAllCardsForCurrentLevel()
    {
        ClearSelection();

        LevelController activeLevel = FindActiveLevelController();
        if (activeLevel == null) return;

        foreach (var kvp in cardsByData)
        {
            int count = activeLevel.GetTrapCount(kvp.Key);
            kvp.Value.SetQuantity(count);
        }
    }

    private void UpdateCardCountFromLevel(TrapCardUI card)
    {
        LevelController activeLevel = FindActiveLevelController();
        if (activeLevel == null || card == null || card.TrapData == null) return;

        int count = activeLevel.GetTrapCount(card.TrapData);
        card.SetQuantity(count);
    }

    private LevelController FindActiveLevelController()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentLevelRoot != null)
        {
            LevelController level = GameManager.Instance.CurrentLevelRoot.GetComponentInChildren<LevelController>();
            if (level != null) return level;
        }

        LevelController[] allLevels = FindObjectsOfType<LevelController>();
        foreach (LevelController level in allLevels)
        {
            if (level.gameObject.activeInHierarchy)
            {
                return level;
            }
        }

        return null;
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

        SoundManager.Instance.PlayCardClicked();
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