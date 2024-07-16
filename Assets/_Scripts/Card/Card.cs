using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool IsShown { get; private set; }

    [Header("Instances")]
    [SerializeField] private Image[] _suitImages;
    [SerializeField] private Image[] _valueImages;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _reverse;

    private CardData _card;
    private CardPile _pile;
    private Transform _lastParentTransform;
    private Transform _beforeDragParent;
    private Transform _parentWhenDragging;
    private Vector2 _startDragPos;

    public delegate void OnMovedDelegate();
    public OnMovedDelegate OnMovedHandler;

    public void SetDragParent(Transform canvas) => _parentWhenDragging = canvas;

    public void SetCard(CardData card, Sprite suitSprite, Sprite valueSprite)
    {
        _card = card;

        Color cardColor = _card.Suit == CardSuit.Diamonds ||
                          _card.Suit == CardSuit.Hearts ?
                          Color.red :
                          Color.black;

        foreach (Image suitImage in _suitImages)
        {
            suitImage.sprite = suitSprite;
            suitImage.color = cardColor;
        }

        foreach (Image valueImage in _valueImages)
        {
            valueImage.sprite = valueSprite;
            valueImage.color = cardColor;
        }

        transform.name = $"{_card.Suit} {_card.Value}";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsShown)
            return;

        _startDragPos = transform.position;
        _beforeDragParent = transform.parent;
        transform.SetParent(_parentWhenDragging);
        _lastParentTransform = transform.parent;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = .6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsShown)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        if (transform.parent == _lastParentTransform)
        {
            transform.SetParent(_beforeDragParent);
            transform.position = _startDragPos;
        }

        OnMovedHandler?.Invoke();
    }

    public void ShowFace()
    {
        IsShown = true;
        _face.SetActive(true);
        _reverse.SetActive(false);
    }

    public void ShowReverse()
    {
        IsShown = false;
        _face.SetActive(false);
        _reverse.SetActive(true);
    }

    public void SetParent(Transform parent, Vector2 offset)
    {
        _beforeDragParent = parent;
        transform.SetParent(parent);
        transform.localPosition = offset;
        transform.localScale = Vector3.one;
    }

    public void SetPile(CardPile pile)
    {
        _pile?.RemoveCard(this);
        _pile = pile;
    }

    public CardPile GetPile()
    {
        return _pile;
    }

    public List<Card> GetAllCardsBelow()
    {
        if (_pile == null)
            return new List<Card>();

        return _pile.GetCardsBelow(this);
    }

    public CardSuit GetSuit() => _card.Suit;

    public CardValue GetValue() => _card.Value;

    public Transform GetParent() => _beforeDragParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsShown)
            return;

        GameSetup.Instance.TryAutoPlaceCard(this);
    }
}
