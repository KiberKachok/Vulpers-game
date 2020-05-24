using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UB.Simple2dWeatherEffects.Standard;

public class TradeTab : MonoBehaviour
{
    public float totalAmount;

    public TextMeshProUGUI moneyCaravanText;
    public TextMeshProUGUI woodCaravanText;
    public TextMeshProUGUI ironCaravanText;
    public TextMeshProUGUI foodCaravanText;

    public TextMeshProUGUI moneyVillageText;
    public TextMeshProUGUI woodVillageText;
    public TextMeshProUGUI ironVillageText;
    public TextMeshProUGUI foodVillageText;

    public TextMeshProUGUI totalAmountText;

    [SerializeField]
    D2FogsPE fog;

    [SerializeField]
    GameObject Wrap;

    Caravan caravan;
    Village village;

    public bool isTrading = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTrading)
        {
            moneyCaravanText.text = caravan.Gold.ToString();
            woodCaravanText.text = caravan.Wood.ToString();
            ironCaravanText.text = caravan.Iron.ToString();
            foodCaravanText.text = caravan.Food.ToString();

            moneyVillageText.text = village.Gold.ToString();
            woodVillageText.text = village.Wood.ToString();
            ironVillageText.text = village.Iron.ToString();
            foodVillageText.text = village.Food.ToString();

            totalAmountText.text = totalAmount.ToString();
        }
    }

    public void StartTrade(Caravan caravanf, Village villagef)
    {
        Wrap.SetActive(true);
        caravan = caravanf;
        village = villagef;
        Time.timeScale = 0f;
        totalAmount = 0;

        isTrading = true;

    }

    public void StopTrade()
    {
        Wrap.SetActive(false);
        Time.timeScale = 1f;

        isTrading = false;

    }

    public void AddCaravanWood()
    {
        if (caravan.Gold > village.valueWood && village.Wood > 0)
        {
            village.Wood++;
            caravan.Wood++;

            caravan.Gold -= village.valueWood;
            village.Gold += village.valueWood;

            totalAmount -= village.valueWood;
        }
    }

    public void AddCaravanFood()
    {
        if (caravan.Gold > village.valueFood && village.Food > 0)
        {
            village.Food--;
            caravan.Food++;

            caravan.Gold -= village.valueFood;
            village.Gold += village.valueFood;

            totalAmount -= village.valueFood;
        }
    }

    public void AddCaravanIron()
    {
        if (caravan.Gold > village.valueIron && village.Iron > 0)
        {
            village.Iron--;
            caravan.Iron++;

            caravan.Gold -= village.valueIron;
            village.Gold += village.valueIron;

            totalAmount -= village.valueIron;
        }
    }

    public void AddVillageWood()
    {
        if (village.Gold > village.valueIron && caravan.Wood > 0)
        {
            village.Wood++;
            caravan.Wood--;

            caravan.Gold += village.valueWood;
            village.Gold -= village.valueWood;

            totalAmount += village.valueIron;
        }
    }

    public void AddVillageFood()
    {
        if (village.Gold > village.valueFood && caravan.Food > 0)
        {
            village.Food++;
            caravan.Food--;

            caravan.Gold += village.valueFood;
            village.Gold -= village.valueFood;

            totalAmount += village.valueFood;
        }
    }

    public void AddVillageIron()
    {
        if (village.Gold > village.valueIron && caravan.Iron > 0)
        {
            village.Iron++;
            caravan.Iron--;

            caravan.Gold += village.valueIron;
            village.Gold -= village.valueIron;

            totalAmount += village.valueIron;
        }
    }
}
