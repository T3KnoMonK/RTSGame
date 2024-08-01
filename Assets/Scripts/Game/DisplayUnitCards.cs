using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUnitCards : MonoBehaviour
{
    [SerializeField]
    private Player _Player;
    private RectTransform _ParentPanel;

    private float _UnitCardWidth;
    private float _UnitCardHeight;
    private float _RightPadding = 10.0f;
    private float _BottomPadding = 10.0f;

    [SerializeField] private int _UnitCardRows = 2;
    [SerializeField] private int _UnitCardColumns = 10;

    private List<GameObject> _UnitCards = new List<GameObject>();

    private void Start()
    {
        Unit.DestroyUnitCardEvent += DestroySingleCard;

        _ParentPanel = GetComponent<RectTransform>();
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _UnitCardWidth = _ParentPanel.GetComponent<RectTransform>().rect.width / 10;
        _UnitCardHeight = _ParentPanel.GetComponent<RectTransform>().rect.height / 3;
    }

    public void AddUnitCardsToUI()
    {
        List<Selectable> unitsList = _Player.Army.GetPlayerSelectedObjects();

        for (int i = 0; i < unitsList.Count; i++)
        {
            Selectable currentUnit = unitsList[i];
            int id = currentUnit.GetID();
            GameObject card = new GameObject();
            SO_Unit unitSO = currentUnit.GetComponent<Unit>().GetSO() as SO_Unit;

            card.transform.SetParent(gameObject.transform);

            card.AddComponent<Button>();
            card.AddComponent<CanvasRenderer>();
            card.AddComponent<Image>();
            card.AddComponent<BoxCollider2D>();

            //For some reason AddComponent won't pass component in an assignment.
            Button cardButton = card.GetComponent<Button>();
            Image cardImage = card.GetComponent<Image>();
            RectTransform cardRect = card.GetComponent<RectTransform>();
            BoxCollider2D cardCollider = card.GetComponent<BoxCollider2D>();

            cardImage.sprite = unitSO.Card;
            cardImage.raycastTarget = true;

            cardRect.anchorMin = new Vector2(0.0f, 1.0f);
            cardRect.anchorMax = new Vector2(0.0f, 1.0f);
            cardRect.pivot = new Vector2(0.0f, 1.0f);
            cardRect.sizeDelta = new Vector2(_UnitCardWidth, _UnitCardHeight);

            cardRect.anchoredPosition = new Vector2(
                (i % _UnitCardColumns * _UnitCardWidth) + (_RightPadding*i)
                ,0); //TODO: Fix row/column spacing

            cardCollider.size.Set(_UnitCardWidth, _UnitCardHeight);

            cardButton.onClick.AddListener(
                delegate { SelectUnitFromCard(currentUnit);}
                );

            card.gameObject.tag = "UnitCard";

            _UnitCards.Add(card);
            currentUnit.GetComponent<Unit>().SetUnitCardRef(card);
        }
    }

    private void ResizeCardArray()
    {

    }

    //public void RemovedDestroyedUnitCard(Selectable go)
    //{
    //    Debug.Log("Destroying unit card");
    //    GameObject tmp;
    //    for(int i = 0; i < _UnitCardAssoc.Count; i++)
    //    {
    //        if(_UnitCardAssoc.TryGetValue(go.GetID(), out tmp))
    //        {
    //            if (gameObject.transform.GetChild(i).gameObject != null)
    //                Destroy(gameObject.transform.GetChild(i).gameObject);
    //        }
    //        else
    //        {
    //            Destroy(tmp);
    //            break;
    //        }
    //    }
    //}
    private void DestroySingleCard(GameObject card, GameObject unit)
    {
        _UnitCards.Remove(card);
        Destroy(card);
        //card.GetComponent<Image>().color = Color.red;
        Destroy(unit);
        ReorderCards();
    }

    private void ReorderCards()
    {
        for(int i = 0; i < _UnitCards.Count; i++)
        {
            _UnitCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                (i % _UnitCardColumns * _UnitCardWidth) + (_RightPadding * i)
                , 0);
        }
    }

    public void SelectUnitFromCard(Selectable go)
    {
        _Player.Army.AddSingleObjectToSelected(go);
    }

    public void RemoveUnitCardsFromUI()
    {
        int count = gameObject.transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject card = gameObject.transform.GetChild(i).gameObject;
            //This doesn't work with more than 1 for some reason
            if (card != null)
            {
                _UnitCards.Remove(card);
                Destroy(card);
            }
        }
    }

}

