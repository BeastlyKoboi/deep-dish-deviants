using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField]
    public Player player;

    [SerializeField]
    GameManager gameManager;

   [SerializeField] int fireLevel;
   [SerializeField] int cutLevel;
   [SerializeField] int kneadLevel;
   [SerializeField] int sortLevel;
   [SerializeField] int timeLevel;
   [SerializeField] int mindLevel;
   [SerializeField] bool hasBoughtOven;
   [SerializeField] bool hasBoughtSlicer;
   [SerializeField] bool hasBoughtPounder;
   [SerializeField] Oven gameOven;
   [SerializeField] Slicer gameCutter;
   [SerializeField] Pounder gameKneader;

    [SerializeField] float firePrice;
    [SerializeField] float cutPrice;
    [SerializeField] float kneadPrice;
    [SerializeField] float sortPrice;
    [SerializeField] float timePrice;
    [SerializeField] float mindPrice;
    [SerializeField] float ovenPrice;
    [SerializeField] float slicerPrice;
    [SerializeField] float pounderPrice;

    [SerializeField] TextMeshProUGUI fireLevelText;
    [SerializeField] TextMeshProUGUI firePriceText;
    [SerializeField] TextMeshProUGUI cutLevelText;
    [SerializeField] TextMeshProUGUI cutPriceText;
    [SerializeField] TextMeshProUGUI kneadLevelText;
    [SerializeField] TextMeshProUGUI kneadPriceText;
    [SerializeField] TextMeshProUGUI sortLevelText;
    [SerializeField] TextMeshProUGUI sortPriceText;
    [SerializeField] TextMeshProUGUI mindLevelText;
    [SerializeField] TextMeshProUGUI mindPriceText;
    [SerializeField] TextMeshProUGUI timeLevelText;
    [SerializeField] TextMeshProUGUI timePriceText;

    float playerMoney;

    // Start is called before the first frame update
    void Start()
    {
        gameOven.gameObject.SetActive(false);
        gameCutter.gameObject.SetActive(false);
        gameKneader.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameManager.GameState.Closed)
        {
            playerMoney = gameManager.CashTotal;
        }
        if (fireLevel == 5)
        {
            firePriceText.text = "Spell Maxed Out";

        }
        if (cutLevel == 5)
        {
            cutPriceText.text = "Spell Maxed Out";

        }
        if (kneadLevel == 5)
        {
            kneadPriceText.text = "Spell Maxed Out";

        }
        if (sortLevel == 5)
        {
            sortPriceText.text = "Spell Maxed Out";

        }
        if (mindLevel == 3)
        {
            mindPriceText.text = "Spell Maxed Out";

        }
        if (timeLevel == 3)
        {
            timePriceText.text = "Spell Maxed Out";

        }
    }


    public void BuyFireUpgrade()
    {
        if (fireLevel < 5 && playerMoney >= firePrice)
        {
            playerMoney -= firePrice;
            fireLevel++;
            firePrice *= 1.2f;
            player.maxCooldownFire *= 0.9f;
            gameManager.CashTotal = playerMoney;
            fireLevelText.text = "Current Level: " + fireLevel;
            firePriceText.text = "Price: $" + firePrice;
        }
  
    }

    public void BuyCutUpgrade()
    {
        if(cutLevel < 5 && playerMoney >= cutPrice)
        {
            playerMoney -= cutPrice;
            cutLevel++;
            cutPrice *= 1.2f;
            player.maxCooldownCut *= 0.9f;
            gameManager.CashTotal = playerMoney;
            cutLevelText.text = "Current Level: " + cutLevel;
            cutPriceText.text = "Price: $" + cutPrice;
        }
    }

    public void BuyKneadUpgrade()
    {
        if (kneadLevel < 5 && playerMoney >= kneadPrice)
        {
            playerMoney -= kneadPrice;
            kneadLevel++;
            kneadPrice *= 1.2f;
            player.maxCooldownKnead *= 0.9f;
            gameManager.CashTotal = playerMoney;
            kneadLevelText.text = "Current Level: " + kneadLevel;
            kneadPriceText.text = "Price: $" + kneadPrice;
        }
    }

    public void BuySortUpgrade()
    {
        if (sortLevel < 5 && playerMoney >= sortPrice)
        {
            playerMoney -= sortPrice;
            sortLevel++;
            sortPrice *= 1.2f;
            player.maxCooldownSort *= 0.85f;
            gameManager.CashTotal = playerMoney;
            sortLevelText.text = "Current Level: " + sortLevel;
            sortPriceText.text = "Price: $" + sortPrice;
        }
    }

    public void BuyTimeUpgrade()
    {
        if (timeLevel < 3 && playerMoney >= timePrice)
        {
            playerMoney -= timePrice;
            timeLevel++;
            timePrice *= 1.2f;
            player.maxCooldownTime *= 0.8f;
            gameManager.CashTotal = playerMoney;
            timeLevelText.text = "Current Level: " + timeLevel;
            timePriceText.text = "Price: $" + timePrice;
        }
    }
    public void BuyMindUpgrade()
    {
        if (mindLevel < 3 && playerMoney >= mindPrice)
        {
            playerMoney -= mindPrice;
            mindLevel++;
            mindPrice *= 1.2f;
            player.maxCooldownMind *= 0.8f;
            gameManager.CashTotal = playerMoney;
            mindLevelText.text = "Current Level: " + mindLevel;
            mindPriceText.text = "Price: $" + mindPrice;
        }
    }

    public void BuyOven()
    {
        if(!hasBoughtOven && playerMoney>= ovenPrice)
        {
            hasBoughtOven = true;
            gameOven.gameObject.SetActive(true);
            
        }
    }
    public void BuySlicer()
    {
        if (!hasBoughtSlicer && playerMoney >= slicerPrice)
        {
            hasBoughtSlicer = true;
            gameCutter.gameObject.SetActive(true);

        }
    }

    public void BuyKneader()
    {
        if (!hasBoughtPounder && playerMoney >= pounderPrice)
        {
            hasBoughtPounder = true;
            gameKneader.gameObject.SetActive(true);

        }
    }
}
