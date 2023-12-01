using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsShown { get; private set; }

    [Header("Instances")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI[] _valueTexts;
    [SerializeField] private TextMeshProUGUI[] _suitTexts;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _reverse;

    private Card _card;
    private CardPile _pile;
    private Transform _lastParentTransform;
    private Transform _beforeDragParent;
    private Transform _parentWhenDragging;
    private Vector2 _startDragPos;

    public delegate void OnMovedDelegate();
    public OnMovedDelegate OnMovedHandler;

    public void SetDragParent(Transform canvas) => _parentWhenDragging = canvas;

    public void SetCard(Card card)
    {
        _card = card;
        foreach (var valueText in _valueTexts)
        {
            if (_card.Value == CardValue.Ace || (int)_card.Value >= 10)
                valueText.SetText(_card.Value.ToString());
            else
                valueText.SetText(((int)_card.Value + 1).ToString());
        }

        foreach (var colorText in _suitTexts)
            colorText.SetText(_card.Suit.ToString());

        transform.name = $"{_card.Suit} {_card.Value}";

        if (_card.Suit == CardSuit.Diamonds || _card.Suit == CardSuit.Hearts)
            _image.color = Color.red;
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
        transform.SetParent(parent);
        transform.localPosition = offset;
        transform.localScale = Vector3.one;
    }

    public void SetPile(CardPile pile)
    {
        _pile?.RemoveCard(this);
        _pile = pile;
    }

    public List<CardDisplay> GetAllCardsBelow()
    {
        if (_pile == null)
            return new List<CardDisplay>();

        return _pile.GetCardsBelow(this);
    }

    public CardSuit GetSuit() => _card.Suit;

    public CardValue GetValue() => _card.Value;
}
