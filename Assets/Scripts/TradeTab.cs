using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TradeTab : MonoBehaviour
{
    public float moneyCaravan;
    public float ironCaravan;
    public float woodCaravan;
    public float foodCaravan;

    public float moneyVillage;
    public float ironVillage;
    public float woodVillage;
    public float foodVillage;

    public float totalAmount;

    public TextMeshProUGUI woodCaravanText;
    public TextMeshProUGUI moneyCaravanText;
    public TextMeshProUGUI foodCaravanText;
    public TextMeshProUGUI ironCaravanText;

    public TextMeshProUGUI woodVillageText;
    public TextMeshProUGUI moneyVillageText;
    public TextMeshProUGUI foodVillageText;
    public TextMeshProUGUI ironVillageText;

    public TextMeshProUGUI totalAmountText;

    [SerializeField]
    GameObject Wrap;

    Caravan caravan;
    Village village;

    public bool trade = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trade)
        {
            woodCaravanText.text = woodCaravan.ToString();
            moneyCaravanText.text = caravan.Gold.ToString();
            foodCaravanText.text = foodCaravan.ToString();
            ironCaravanText.text = ironCaravan.ToString();

            woodVillageText.text = woodVillage.ToString();
            moneyVillageText.text = village.Gold.ToString();
            foodVillageText.text = foodVillage.ToString();
            ironVillageText.text = ironVillage.ToString();

            totalAmountText.text = totalAmount.ToString();
        }
    }

    public void StartTrade(Caravan caravanf, Village villagef)
    {
        Wrap.SetActive(true);
        caravan = caravanf;
        village = villagef;
        Time.timeScale = 0.0001f;

        moneyCaravan = caravan.Gold;
        ironCaravan = caravan.Iron;
        foodCaravan = caravan.Food;
        woodCaravan = caravan.Wood;

        moneyVillage = village.Gold;
        ironVillage = village.Iron;
        foodVillage = village.Food;
        woodVillage = village.Wood;

        totalAmount = 0;

        trade = true;
    }

    public void StopTrade()
    {
        Wrap.SetActive(false);
        caravan.Gold = moneyCaravan;
        caravan.Iron = ironCaravan;
        caravan.Food = foodCaravan;
        caravan.Wood = woodCaravan;

        village.Gold = moneyVillage;
        village.Iron = ironVillage;
        village.Food = foodVillage;
        village.Wood = woodVillage;

        Time.timeScale = 1f;

        trade = false;
    }

    public void AddCaravanWood()
    {
        if (caravan.Gold > village.valueWood && woodVillage > 0)
        {
            woodVillage -= 1;
            woodCaravan += 1;

            caravan.Gold -= village.valueWood;
            village.Gold += village.valueWood;

            totalAmount -= village.valueWood;
        }
    }

    public void AddCaravanFood()
    {
        if (caravan.Gold > village.valueFood && foodVillage > 0)
        {
            foodVillage--;
            foodCaravan++;

            caravan.Gold -= village.valueFood;
            village.Gold += village.valueFood;

            totalAmount -= village.valueFood;
        }
    }

    public void AddCaravanIron()
    {
        if (caravan.Gold > village.valueIron && ironVillage > 0)
        {
            ironVillage--;
            ironCaravan++;

            caravan.Gold -= village.valueIron;
            village.Gold += village.valueIron;

            totalAmount -= village.valueIron;
        }
    }

    public void AddVillageWood()
    {
        if (village.Gold > (10 - village.valueIron) && woodCaravan > 0)
        {
            woodVillage++;
            woodCaravan--;

            caravan.Gold += (10 - village.valueWood);
            village.Gold -= (10 - village.valueWood);

            totalAmount += (10 - village.valueIron);
        }
    }

    public void AddVillageFood()
    {
        if (village.Gold > (10 - village.valueFood) && foodCaravan > 0)
        {
            foodVillage++;
            foodCaravan--;

            caravan.Gold += (10 - village.valueFood);
            village.Gold -= (10 - village.valueFood);

            totalAmount += (10 - village.valueFood);
        }
    }

    public void AddVillageIron()
    {
        if (village.Gold > (10 - village.valueIron) && ironCaravan > 0)
        {
            ironVillage++;
            ironCaravan--;

            caravan.Gold += (10 - village.valueIron);
            village.Gold -= (10 - village.valueIron);

            totalAmount += (10 - village.valueIron);
        }
    }
}
