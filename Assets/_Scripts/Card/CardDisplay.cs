using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Card Card { get; private set; }

    [Header("Instances")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI[] _valueTexts;
    [SerializeField] private TextMeshProUGUI[] _colorTexts;
    [SerializeField] private Transform _cardOnTop;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Vector2 _lastDragPos;

    public void SetCard(Card card)
    {
        Card = card;
        foreach (var valueText in _valueTexts)
            valueText.SetText(Card.value.ToString());

        foreach (var colorText in _colorTexts)
            colorText.SetText(Card.color.ToString());

        transform.name = $"{Card.color} {Card.value}";

        if ((int)card.color < 2)
            _image.color = Color.red;
    }
    
    public void AddCardOnTop(CardDisplay card)
    {
        card.transform.SetParent(_cardOnTop);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastDragPos = transform.position;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = .6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            transform.position = _lastDragPos;
            return;
        }

        CardDisplay draggedCard = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (draggedCard == null || draggedCard == this)
        {
            transform.position = _lastDragPos;
            return;
        }

        AddCardOnTop(draggedCard);
    }
}
