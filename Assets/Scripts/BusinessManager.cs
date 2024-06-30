using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BusinessManager : MonoBehaviour
{
    [SerializeField] GameObject businessManagerUI;
    [SerializeField] GameObject InputManagerHead;
    [SerializeField] Button upgradeToolsButton;
    [SerializeField] TMP_InputField workerInputField;
    [SerializeField] TMP_InputField rawMaterialInputField;
    [SerializeField] Button confirmWorkerButton;
    [SerializeField] Button confirmRawMaterialButton;
    [SerializeField] TMP_InputField productPriceInputField;
    [SerializeField] Button confirmPriceButton;

    [SerializeField] GameObject[] panels;
    [SerializeField] Button[] panelButtons;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI workerText;
    public TextMeshProUGUI rawMaterialText;
    public TextMeshProUGUI productionSpeedText;
    public TextMeshProUGUI inefficiencyScoreText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI debtText;
    public TextMeshProUGUI qualityText;
    public TextMeshProUGUI demandText;
    public TextMeshProUGUI customerSatisfactionText;
    public TextMeshProUGUI grossExpendituresText;
    public TextMeshProUGUI grossProfitText;
    public TextMeshProUGUI supplyText;
    public TextMeshProUGUI toolsText;
    public TextMeshProUGUI eventBannerText;
    public TextMeshProUGUI productPriceText;
    public GameObject eventBannerUI;

    public int workerCostPerSecond = 5;
    public float rawMaterialCostPerSecond = 2;
    public float productPrice = 50;
    public int workerProduction = 10;
    public float workerHappiness = 0.7f;
    public float defectiveProductRate = 0.01f;
    public float advertisingEffect = 1.0f;
    public float interestRate = 0.05f;
    public float loanAmount = 0f;

    public int tools = 0;
    public int maxTools = 5;
    public int toolUpgradeCost = 100;
    public float toolEfficiencyIncrease = 0.1f;
    public float toolPriceIncrease = 5.0f;

    private int workers = 0;
    private int rawMaterials = 0;
    private float productionSpeed = 0f;
    private float productsProduced = 0f;
    private int inefficiencyScore = 0;
    private float profitRate = 0f;
    private float debt = 0f;
    private float qualityMultiplier = 1.0f;
    private float demand = 1.0f;
    private float customerSatisfaction = 1.0f;
    public bool businessStart = false;
    public bool randomEventsEnabled = false;

    private InputManager inputManager;
    public bool UIOpen = false;

    private float grossExpendituresRate = 0f;
    private float grossProfitRate = 0f;
    private float supply = 0f;

    private float randomEventMinInterval = 30f;
    private float randomEventMaxInterval = 45f;

    private void Awake()
    {
        inputManager = InputManagerHead.GetComponent<InputManager>();
    }

    void Start()
    {
        UpdateUI();
        InvokeRepeating("ApplyInterest", 10.0f, 10.0f); // Apply interest every 10 seconds
        upgradeToolsButton.onClick.AddListener(UpgradeTools);
        confirmWorkerButton.onClick.AddListener(UpdateWorkerCount);
        confirmRawMaterialButton.onClick.AddListener(UpdateRawMaterialCount);
        confirmPriceButton.onClick.AddListener(UpdateProductPrice);

        CheckToolUpgradeButton();

        if (randomEventsEnabled)
        {
            StartCoroutine(RandomEventsCoroutine());
        }
    }


    public void setInterest(float inputInterest) {
        interestRate = inputInterest;
    }
    void Update()
    {
        if (businessStart)
        {
            if (inputManager.GetBPressed())
            {
                ToggleBusinessUI();
                inputManager.SetBPressed(false);
            }
            ProduceProducts();
            UpdateUI();
        }
    }

    public void toggleBusinessStart(bool input) {
        businessStart = input;
    }
    public void setBusiness(bool checkBusiness)
    {
        businessStart = checkBusiness;
    }

    public void setRandomEvents(bool enableRandomEvents)
    {
        randomEventsEnabled = enableRandomEvents;
        if (randomEventsEnabled)
        {
            StartCoroutine(RandomEventsCoroutine());
        }
        else
        {
            StopCoroutine(RandomEventsCoroutine());
        }
    }

    public void ToggleBusinessUI()
    {
        if (UIOpen)
        {
            Debug.Log("Closed UI");
            businessManagerUI.SetActive(false);
            UIOpen = false;
        }
        else
        {
            Debug.Log("Opened UI");
            businessManagerUI.SetActive(true);
            UIOpen = true;
        }
    }

    public void openBusinessUI()
    {
        // Implement as needed
    }

    public void closeBusinessUI()
    {
        // Implement as needed
    }

    public void HireWorker()
    {
        if (GameManager.Instance.playerMoney >= workerCostPerSecond)
        {
            GameManager.Instance.AdjustMoney(-workerCostPerSecond);
            workers++;
            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    public void changeMoney(int moneyamount)
    {
        if ((GameManager.Instance.playerMoney + moneyamount) <= 0)
        {
            GameManager.Instance.playerMoney = 0;
        }
        else
        {
            GameManager.Instance.playerMoney += moneyamount;
        }
    }

    public void FireWorker(int amt)
    {
        if (workers > 0)
        {
            if (workers - amt <= 0)
            {
                workers = 0;
            }
            else
            {
                workers -= amt;
            }

            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    public void OrderRawMaterials()
    {
        if (GameManager.Instance.playerMoney >= rawMaterialCostPerSecond)
        {
            GameManager.Instance.AdjustMoney(-rawMaterialCostPerSecond);
            rawMaterials++;
            UpdateProductionSpeed();
            CheckForInefficiency();
        }
    }

    public void TakeLoan(float amount)
    {
        GameManager.Instance.AdjustMoney(amount);
        debt += amount;
        loanAmount += amount;
    }

    public void PayOffLoan()
    {
        float amount = GameManager.Instance.playerMoney;
        if (debt >= amount)
        {
            GameManager.Instance.AdjustMoney(-amount);
            debt -= amount;

        }

        else {
            GameManager.Instance.AdjustMoney(-debt);
            if (debt < 0) debt = 0;
        }
    }

    private void ApplyInterest()
    {
        if (debt > 0)
        {
            float interest = debt * interestRate;
            debt += interest;
        }
    }

    private void UpdateProductionSpeed()
    {
        productionSpeed = Mathf.Min(workers, rawMaterials);
    }

    private void ProduceProducts()
    {
        float efficiency = workerHappiness + 0.3f + (tools * toolEfficiencyIncrease); // Worker efficiency calculation
        productsProduced += productionSpeed * efficiency * Time.deltaTime;
        supply = productionSpeed * efficiency;
        SellProducts();
    }

    private void SellProducts()
    {
        float productsToSell = Mathf.FloorToInt(productsProduced);
        float manufacturingCost = productsToSell * (workerCostPerSecond + rawMaterialCostPerSecond);
        productsToSell = Mathf.Min(productsToSell, demand);
        float salesRevenue = productsToSell * productPrice * qualityMultiplier;

        // Calculate defective products
        int defectiveProductsProduced = Mathf.FloorToInt(productsToSell * defectiveProductRate);
        productsToSell -= defectiveProductsProduced;

        // Adjust for defective products
        float defectiveProductLoss = defectiveProductsProduced * productPrice;
        salesRevenue -= defectiveProductLoss;
        customerSatisfaction -= defectiveProductRate * 0.1f; // Decrease customer satisfaction

        // Sell products based on demand
        float productsSold = Mathf.Min(productsToSell, Mathf.FloorToInt(demand));
        productsToSell -= productsSold;

        // Calculate profit rate and adjust money
        grossExpendituresRate = manufacturingCost;
        grossProfitRate = productsSold * productPrice * qualityMultiplier;
        float netProfit = grossProfitRate - grossExpendituresRate;
        profitRate = netProfit;
        GameManager.Instance.AdjustMoney(netProfit);

        productsProduced -= productsSold;
    }

    private void CheckForInefficiency()
    {
        if (workers > rawMaterials)
        {
            inefficiencyScore += (workers - rawMaterials);
        }
        else if (rawMaterials > workers)
        {
            inefficiencyScore += (rawMaterials - workers);
        }

        // Adjust customer satisfaction based on quality and inefficiency
        customerSatisfaction = qualityMultiplier - (inefficiencyScore * 0.01f);
        if (customerSatisfaction < 0) customerSatisfaction = 0;
    }

    public void Advertise(float cost, float effect)
    {
        if (GameManager.Instance.playerMoney >= cost)
        {
            GameManager.Instance.AdjustMoney(-cost);
            advertisingEffect = effect;
        }
    }

    private void UpgradeTools()
    {
        if (tools < maxTools && GameManager.Instance.playerMoney >= toolUpgradeCost)
        {
            GameManager.Instance.AdjustMoney(-toolUpgradeCost);
            tools++;
            CheckToolUpgradeButton();
        }
    }

    private void CheckToolUpgradeButton()
    {
        if (tools >= maxTools)
        {
            upgradeToolsButton.gameObject.SetActive(false);
        }
        else
        {
            upgradeToolsButton.gameObject.SetActive(true);
        }
    }

    private void UpdateWorkerCount()
    {
        if (int.TryParse(workerInputField.text, out int workerCount))
        {
            int cost = workerCount * workerCostPerSecond;
            if (GameManager.Instance.playerMoney >= cost)
            {
                GameManager.Instance.AdjustMoney(-cost);
                workers += workerCount;
                UpdateProductionSpeed();
                CheckForInefficiency();
                workerInputField.text = ""; // Clear the input field after updating
            }
        }
    }

    private void UpdateRawMaterialCount()
    {
        if (int.TryParse(rawMaterialInputField.text, out int rawMaterialCount))
        {
            float cost = rawMaterialCount * rawMaterialCostPerSecond;
            if (GameManager.Instance.playerMoney >= cost)
            {
                GameManager.Instance.AdjustMoney(-cost);
                rawMaterials += rawMaterialCount;
                UpdateProductionSpeed();
                CheckForInefficiency();
                rawMaterialInputField.text = ""; // Clear the input field after updating
            }
        }
    }

    private void UpdateProductPrice()
    {
        if (float.TryParse(productPriceInputField.text, out float newPrice))
        {
            productPrice = newPrice;
            demand = 100.0f / productPrice; // Demand is inversely proportional to price
            productPriceInputField.text = ""; // Clear the input field after updating
        }
    }

    private void UpdateUI()
    {
        moneyText.text = "Money: $" + GameManager.Instance.playerMoney.ToString("F2");
        workerText.text = "Workers: " + workers;
        rawMaterialText.text = "Raw Materials: " + rawMaterials;
        productionSpeedText.text = "Production Speed: " + productionSpeed;
        inefficiencyScoreText.text = "Inefficiency Score: " + inefficiencyScore;
        profitText.text = "Profit Rate: $" + profitRate.ToString("F2");
        debtText.text = "Debt: $" + debt.ToString("F2");
        qualityText.text = "Quality Multiplier: " + qualityMultiplier.ToString("F2");
        demandText.text = "Demand: " + demand.ToString("F2");
        customerSatisfactionText.text = "Customer Satisfaction: " + customerSatisfaction.ToString("F2");
        grossExpendituresText.text = "Gross Expenditures Rate: $" + grossExpendituresRate.ToString("F2");
        grossProfitText.text = "Gross Profit Rate: $" + grossProfitRate.ToString("F2");
        supplyText.text = "Supply: " + supply.ToString("F2") + " products/s";
        toolsText.text = "Tools Level: " + tools + "/" + maxTools;
        productPriceText.text = "Product Price: $" + productPrice.ToString("F2");
    }

    private IEnumerator RandomEventsCoroutine()
    {
        while (randomEventsEnabled)
        {
            yield return new WaitForSeconds(Random.Range(randomEventMinInterval, randomEventMaxInterval)); // Wait for a random interval
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        int eventType = Random.Range(0, 5);
        string eventMessage = "";

        switch (eventType)
        {
            case 0:
                Debug.Log("Random Event: Increased Taxes!");
                demand *= 0.9f; // Decrease demand by 10%
                eventMessage = "Increased Taxes are reducing customer's demand for your product!";
                break;
            case 1:
                Debug.Log("Random Event: Tech Improvements!");
                rawMaterialCostPerSecond *= 0.8f; // Decrease raw material cost by 20%
                eventMessage = "Tech improvements have occurred, which drives down your production costs!";
                break;
            case 2:
                Debug.Log("Random Event: Supplier Closed Down!");
                rawMaterialCostPerSecond *= 1.2f; // Increase raw material cost by 20%
                eventMessage = "One of your suppliers has unexpectedly closed down, which increases the price per unit for the same quantity of materials!";
                break;
            case 3:
                Debug.Log("Random Event: Celebrity Endorsement!");
                demand *= 1.3f; // Increase demand by 30%
                eventMessage = "A Celebrity is seen with your product, which increases the demand for your product!";
                break;
            case 4:
                Debug.Log("Random Event: Minimum Wage Increase!");
                workerCostPerSecond += 5; // Increase worker cost by 5
                eventMessage = "Minimum wage has increased, causing your workers to cost $5/hour extra!";
                break;
        }

        ShowEventBanner(eventMessage);
    }

    private void ShowEventBanner(string message)
    {
        eventBannerText.text = message;
        eventBannerUI.SetActive(true);
        StartCoroutine(HideEventBanner());
    }

    private IEnumerator HideEventBanner()
    {
        yield return new WaitForSeconds(5.0f); // Display the banner for 5 seconds
        eventBannerUI.SetActive(false);
    }
}



