using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text coinText;
    private int currentCoin = 0;

    const string COIN_AMOUNT_TEXT = "Coin Amount Text";

    public void UpdateCurrentCoin()
    {
        currentCoin += 1;
        if(coinText == null)
        {
            coinText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        coinText.text = currentCoin.ToString("D3");
    }
}
