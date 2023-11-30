using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Card Card { get; private set; }
    public bool IsShown { get; private set; }

    [Header("Instances")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI[] _valueTexts;
    [SerializeField] private TextMeshProUGUI[] _suitTexts;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _reverse;

    private CardPile _pile;
    private Transform _lastParentTransform;
    private Transform _beforeDragParent;
    private Transform _parentWhenDragging;
    private Vector2 _lastDragPos;

    public delegate void OnMovedDelegate();
    public OnMovedDelegate OnMovedHandler;

    public void SetDragParent(Transform canvas) => _parentWhenDragging = canvas;

    public void SetCard(Card card)
    {
        Card = card;
        foreach (var valueText in _valueTexts)
        {
            if (Card.Value == CardValue.Ace || (int)Card.Value >= 10)
                valueText.SetText(Card.Value.ToString());
            else
                valueText.SetText(((int)Card.Value + 1).ToString());
        }

        foreach (var colorText in _suitTexts)
            colorText.SetText(Card.Suit.ToString());

        transform.name = $"{Card.Suit} {Card.Value}";

        if (card.Suit == CardSuit.Diamonds || card.Suit == CardSuit.Hearts)
            _image.color = Color.red;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsShown)
            return;

        _lastDragPos = transform.position;
        _beforeDragParent = transform.parent;
        transform.parent = _parentWhenDragging;
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
            transform.parent = _beforeDragParent;
            transform.position = _lastDragPos;
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
}
