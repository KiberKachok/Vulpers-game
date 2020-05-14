using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShareTab : MonoBehaviour
{

    public float moneyCaravan;
    public float ironCaravan;
    public float woodCaravan;
    public float foodCaravan;

    public float moneyVillage;
    public float ironVillage;
    public float woodVillage;
    public float foodVillage;

    public TextMeshProUGUI woodCaravanText;
    public TextMeshProUGUI moneyCaravanText;
    public TextMeshProUGUI foodCaravanText;
    public TextMeshProUGUI ironCaravanText;

    public TextMeshProUGUI woodVillageText;
    public TextMeshProUGUI moneyVillageText;
    public TextMeshProUGUI foodVillageText;
    public TextMeshProUGUI ironVillageText;

    [SerializeField]
    GameObject Wrap;

    Caravan caravan;
    Village village;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        woodCaravanText.text = woodCaravan.ToString();
        moneyCaravanText.text = moneyCaravan.ToString();
        foodCaravanText.text = foodCaravan.ToString();
        ironCaravanText.text = ironCaravan.ToString();

        woodVillageText.text = woodVillage.ToString();
        moneyVillageText.text = moneyVillage.ToString();
        foodVillageText.text = foodVillage.ToString();
        ironVillageText.text = ironVillage.ToString();
    }

    public void StartShare(Caravan caravanf, Village villagef)
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
    }

    public void StopShare()
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
    }

    public void AddCaravanGold()
    {
        if (moneyVillage > 0)
        {
            moneyVillage--;
            moneyCaravan++;
        }
    }

    public void AddCaravanWood()
    {
        if (woodVillage > 0)
        {
            woodVillage -= 1;
            woodCaravan += 1;
        }
    }

    public void AddCaravanFood()
    {
        if (foodVillage > 0)
        {
            foodVillage--;
            foodCaravan++;
        }
    }

    public void AddCaravanIron()
    {
        if (ironVillage > 0)
        {
            ironVillage--;
            ironCaravan++;
        }
    }

    public void AddVillageGold()
    {
        if (moneyCaravan > 0)
        {
            moneyVillage++;
            moneyCaravan--;
        }
    }

    public void AddVillageWood()
    {
        if (woodCaravan > 0)
        {
            woodVillage++;
            woodCaravan--;
        }
    }

    public void AddVillageFood()
    {
        if (foodCaravan > 0)
        {
            foodVillage++;
            foodCaravan--;
        }
    }

    public void AddVillageIron()
    {
        if (foodCaravan > 0)
        {
            foodVillage++;
            foodCaravan--;
        }
    }
}
