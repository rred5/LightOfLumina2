using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BusinessManager : MonoBehaviour
{
    public Text moneyText;
    public Text workerText;
    public Text rawMaterialText;
    public Text productionSpeedText;
    public Text inefficiencyScoreText;
    public Text profitText;
    public Text faultyProductText;

    public int workerCost = 100;
    public int rawMaterialCost = 10;
    public int productPrice = 50;
    public int workerPayPerUnit = 5;
    public int inefficiencyPenalty = 5;
    public int faultyProductPenalty = 10;
    public float faultyProductRate = 0.1f; // 10% of products are faulty

    private int workers = 0;
    private int rawMaterials = 0;
    private float productionSpeed = 0f;
    private float productsProduced = 0f;
    private int inefficiencyScore = 0;
    private float profit = 0f;
    private int faultyProducts = 0;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        ProduceProducts();
        UpdateUI();
    }

    public void HireWorker()
    {
        if (GameManager.Instance.playerMoney >= workerCost)
        {
            GameManager.Instance.AdjustMoney(-workerCost);
            workers++;
            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    public void FireWorker()
    {
        if (workers > 0)
        {
            workers--;
            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    public void OrderRawMaterials()
    {
        if (GameManager.Instance.playerMoney >= rawMaterialCost)
        {
            GameManager.Instance.AdjustMoney(-rawMaterialCost);
            rawMaterials++;
            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    private void UpdateProductionSpeed()
    {
        productionSpeed = Mathf.Min(workers, rawMaterials);
    }

    private void ProduceProducts()
    {
        productsProduced += productionSpeed * Time.deltaTime;
        SellProducts();
    }

    private void SellProducts()
    {
        int productsToSell = Mathf.FloorToInt(productsProduced);
        float manufacturingCost = productsToSell * workerPayPerUnit;
        float salesRevenue = productsToSell * productPrice;

        // Calculate faulty products
        int faultyProductsProduced = Mathf.FloorToInt(productsToSell * faultyProductRate);
        faultyProducts += faultyProductsProduced;

        // Adjust for faulty products
        float returnCost = faultyProductsProduced * faultyProductPenalty;
        salesRevenue -= returnCost;

        GameManager.Instance.AdjustMoney(salesRevenue - manufacturingCost);
        profit += (salesRevenue - manufacturingCost);

        productsProduced -= productsToSell;
    }

    private void CheckForInefficiency()
    {
        if (workers > rawMaterials)
        {
            inefficiencyScore += (workers - rawMaterials) * inefficiencyPenalty;
        }
        else if (rawMaterials > workers)
        {
            inefficiencyScore += (rawMaterials - workers) * inefficiencyPenalty;
        }
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: $" + GameManager.Instance.playerMoney.ToString("F2");
        workerText.text = "Workers: " + workers;
        rawMaterialText.text = "Raw Materials: " + rawMaterials;
        productionSpeedText.text = "Production Speed: " + productionSpeed;
        inefficiencyScoreText.text = "Inefficiency Score: " + inefficiencyScore;
        profitText.text = "Profit: $" + profit.ToString("F2");
        faultyProductText.text = "Faulty Products: " + faultyProducts;
    }
}

