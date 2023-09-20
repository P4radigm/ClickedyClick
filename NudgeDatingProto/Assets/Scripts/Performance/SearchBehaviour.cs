using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchBehaviour : MonoBehaviour
{
    [SerializeField] private SearchCardBehaviour[] searchCards;
    [SerializeField] private float oneCardOdds;
    [SerializeField] private float twoCardsOdds;
    [SerializeField] private float threeCardsOdds;
    [SerializeField] private float fourCardsOdds;
    [SerializeField] private float isSponsoredOdds;
    [SerializeField] private TMP_InputField inputField;
    private string lastSearchInput;

    private void Start()
    {
        Initialise();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleNewSearchInput();
        }
    }

    private void Initialise()
    {
        lastSearchInput = inputField.text;

        for (int i = 0; i < searchCards.Length; i++)
        {
            searchCards[i].visualParent.SetActive(true);
            searchCards[i].sponsoredObject.SetActive(false);
            searchCards[i].numberText.text = $"# {(i+1):D2}";
        }

        searchCards[2].sponsoredObject.SetActive(true);
    }

    public void HandleNewSearchInput()
    {
        if (lastSearchInput == inputField.text)
        {
            return;
        }

        for (int i = 0; i < searchCards.Length; i++)
        {
            searchCards[i].visualParent.SetActive(false);
            searchCards[i].sponsoredObject.SetActive(false);
        }

        int cardsToSpawn = 0;
        float r = Random.Range(0f, 100f);
        //Debug.Log(r);
        if(r < oneCardOdds) { cardsToSpawn = 1; }
        else if(r < oneCardOdds + twoCardsOdds) { cardsToSpawn = 2; }
        else if(r < oneCardOdds + twoCardsOdds + threeCardsOdds) { cardsToSpawn = 3; }
        else { cardsToSpawn = 4; }

        for (int i = 0; i < cardsToSpawn; i++)
        {
            searchCards[i].visualParent.SetActive(true);
            float rs = Random.value * 100f;
            bool isSponsored = rs <= isSponsoredOdds ? true : false;
            searchCards[i].sponsoredObject.SetActive(isSponsored);

            searchCards[i].numberText.text = $"# {Random.Range(1,100):D2}";
        }

        lastSearchInput = inputField.text;
    }
}
