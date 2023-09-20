using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CardContent
{
    public string cardID;
    public bool isSponsored;
    public Color highlight;
    public Color fillColored;
    public Color fillBW;
    public Color textBW;
}

[System.Serializable]
public class CardColor
{
    public Color highlight;
    public Color fillColored;
    public Color fillBW;
    public Color textBW;
}

public class PerformanceSearch : MonoBehaviour
{
    [SerializeField] private CardColor[] cardColorsLight;
    [SerializeField] private CardColor[] cardColorsDark;
    private List<CardContent> totalCardList = new List<CardContent>();
    private List<CardContent> searchResultCards = new List<CardContent>();
    [SerializeField] private int totalCards;
    [SerializeField] private float sponsoredOdds;
    private List<GameObject> initialisedCards = new List<GameObject>();
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int minResults;
    [SerializeField] private int maxResults;
    [SerializeField] private float cardsXPos;
    [SerializeField] private float cardsYStartPos;
    [SerializeField] private float cardsYDistance;
    [SerializeField] private float heightNoSearchResults;
    [SerializeField] private float minContentHeight;
    [SerializeField] private float noCardsContentHeight;
    [SerializeField] private RectTransform cardParentTransform;
    [SerializeField] private RectTransform mainContentTransform;
    public int cardsPerPage;
    public int totalPages;
    public int pageDisplayed;
    [SerializeField] private Button firstPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button lastPageButton;
    [SerializeField] private TextMeshProUGUI previousPageText;
    [SerializeField] private TextMeshProUGUI nextPageText;
    [SerializeField] private TextMeshProUGUI lastPageText;
    [SerializeField] private TextMeshProUGUI firstPageText;
    [SerializeField] private TextMeshProUGUI currentPageText;
    [SerializeField] private TMP_InputField inputField;
    private string lastSearch = "asjdlafweueqgbfkdadiuwdfadfbadkdad";

    [SerializeField] private float disappearTime;
    [SerializeField] private AdBehvaiour ad;

    public void DisappearObject(GameObject go)
    {
        StartCoroutine(CountDownToAppear(go));
    }

    private IEnumerator CountDownToAppear(GameObject go)
    {
        go.SetActive(false);
        yield return new WaitForSeconds(disappearTime);
        go.SetActive(true);
    }

    public void HandleNewSearchInput()
    {
        if(inputField.text == lastSearch) { return; }

        GenerateSearchResult();

        lastSearch = inputField.text;
    }

    public void HandleNextPage()
    {
        DisplayPage(pageDisplayed + 1);
    }

    public void HandlePrevPage()
    {
        DisplayPage(pageDisplayed - 1);
    }

    public void HandleFirstPage()
    {
        DisplayPage(0);
    }

    public void HandleLastPage()
    {
        DisplayPage(totalPages-1);
    }

    public void GenerateCardList()
    {
        totalCardList.Clear();

        for (int i = 0; i < totalCards; i++)
        {
            CardContent randomCard = new();
            randomCard.cardID = $"#{i + 1}";
            randomCard.isSponsored = Random.value < sponsoredOdds ? true : false;
            int colorIndex = Random.Range(0, cardColorsLight.Length);
            randomCard.highlight = cardColorsLight[colorIndex].highlight;
            randomCard.fillColored = cardColorsLight[colorIndex].fillColored;
            randomCard.fillBW = cardColorsLight[colorIndex].fillBW;
            randomCard.textBW = cardColorsLight[colorIndex].textBW;

            totalCardList.Add(randomCard);
        }
    }

    private void GenerateSearchResult()
    {
        searchResultCards.Clear();
        List<CardContent> dummyList = totalCardList;
        int totalResults = Random.Range(minResults, maxResults);

        for (int i = 0; i < totalResults; i++)
        {
            int randomIndex = Random.Range(0, totalCardList.Count);
            searchResultCards.Add(dummyList[randomIndex]);
            dummyList.RemoveAt(randomIndex);
        }

        //Shuffle list
        Shuffle(searchResultCards);

        totalPages = (int)Mathf.Ceil((float)searchResultCards.Count / (float)cardsPerPage);

        firstPageText.text = $"{1}";
        lastPageText.text = $"{totalPages}";

        RescaleRecttransforms();

        //Initialise gameobjects

        ad.ResetAd();

        DisplayPage(0);
    }

    public void UpdateCardPerPageAmount(int newAmount)
    {
        int selectedCardIndex = cardsPerPage * pageDisplayed;

        cardsPerPage = newAmount;

        totalPages = (int)Mathf.Ceil((float)searchResultCards.Count / (float)cardsPerPage);

        RescaleRecttransforms();

        DisplayPage(Mathf.FloorToInt((float)selectedCardIndex / (float)cardsPerPage));
    }

    private void RescaleRecttransforms()
    {
        float projectedYSize = cardsYDistance * (cardsPerPage+1) + noCardsContentHeight;
        Mathf.Clamp(projectedYSize, minContentHeight, Mathf.Infinity);
        cardParentTransform.sizeDelta = new Vector2(cardParentTransform.sizeDelta.x, projectedYSize);

        mainContentTransform.sizeDelta = new Vector2(mainContentTransform.sizeDelta.x, heightNoSearchResults + projectedYSize);
    }

    public void DisplayPage(int pageIndex)
    {
        foreach(GameObject card in initialisedCards)
        {
            Destroy(card);
        }

        initialisedCards.Clear();

        int amountOfCardsOnThisPage = cardsPerPage;

        if(searchResultCards.Count - (pageIndex+1)*cardsPerPage < 0)
        {
            amountOfCardsOnThisPage = searchResultCards.Count % cardsPerPage;
        }

        int startIndex = pageIndex * cardsPerPage;

        for (int i = 0; i < amountOfCardsOnThisPage; i++)
        {
            GameObject cards = Instantiate(cardPrefab, cardParentTransform);
            RectTransform rt = cards.GetComponent<RectTransform>();
            //Set correct position
            rt.anchoredPosition = new Vector2(cardsXPos, -cardsYStartPos - i * cardsYDistance);
            DummyCardHandler handler = cards.GetComponent<DummyCardHandler>();

            handler.Recolor(searchResultCards[startIndex + i].highlight, searchResultCards[startIndex + i].fillColored, searchResultCards[startIndex + i].textBW, searchResultCards[startIndex + i].fillBW);
            handler.UpdateID(searchResultCards[startIndex + i].cardID);
            handler.DisplaySponsored(searchResultCards[startIndex + i].isSponsored);
            handler.performanceSearch = this;
            initialisedCards.Add(cards);
        }

        //Update buttons
        if(pageIndex > 1) { firstPageButton.gameObject.SetActive(true); }
        else { firstPageButton.gameObject.SetActive(false); }

        if(pageIndex < totalPages) { lastPageButton.gameObject.SetActive(true); }
        else { lastPageButton.gameObject.SetActive(false); }

        if (pageIndex > 0) { previousPageButton.gameObject.SetActive(true); }
        else { previousPageButton.gameObject.SetActive(false); }

        if (pageIndex != totalPages) { nextPageButton.gameObject.SetActive(true); }
        else { nextPageButton.gameObject.SetActive(false); }

        previousPageText.text = $"{pageIndex}";
        currentPageText.text = $"{pageIndex+1}";
        nextPageText.text = $"{pageIndex+2}";

        pageDisplayed = pageIndex;
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleNewSearchInput();
        }
    }

    public void ToggleLightDark(bool isLight)
    {

    }
}
