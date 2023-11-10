using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField]
    public Player player;

    [SerializeField]
    GameManager gameManager;

    int fireLevel;
    int cutLevel;
    int kneadLevel;
    int sortLevel;
    bool hasBoughtTime;
    bool hasBoughtMind;
    bool hasBoughtOven;
    bool hasBoughtSlicer;
    bool hasBoughtPounder;

    float firePrice;
    float cutPrice;
    float kneadPrice;
    float sortPrice;
    float timePrice;
    float mindPrice;
    float ovenPrice;
    float slicerPrice;
    float pounderPrice;

    float playerMoney;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameManager.GameState.Closed)
        {
            playerMoney = gameManager.CashTotal;
        }
    }


    public void BuyFireUpgrade()
    {
        if (fireLevel < 5 && playerMoney >= firePrice)
        {
            playerMoney -= firePrice;
            fireLevel++;
            firePrice *= 1.2f;
            player.fireCoolDown *= 0.9f;
            gameManager.CashTotal = playerMoney;
        }
    }

    public void BuyCutUpgrade()
    {
        if(cutLevel < 5 && playerMoney <= cutPrice)
        {
            playerMoney -= cutPrice;
            cutLevel++;
            cutPrice *= 1.2f;
            player.cutCoolDown *= 0.9f;
            gameManager.CashTotal = playerMoney;
        }
    }

    public void BuyOven()
    {
        if(!hasBoughtOven && playerMoney>= ovenPrice)
        {
            hasBoughtOven = true;
            
        }
    }
}
