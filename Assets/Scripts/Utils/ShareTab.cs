using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShareTab : MonoBehaviour
{
    public TextMeshProUGUI moneyCaravanText;
    public TextMeshProUGUI woodCaravanText;
    public TextMeshProUGUI ironCaravanText;
    public TextMeshProUGUI foodCaravanText;

    public TextMeshProUGUI moneyVillageText;
    public TextMeshProUGUI woodVillageText;
    public TextMeshProUGUI ironVillageText;
    public TextMeshProUGUI foodVillageText;

    [SerializeField]
    GameObject Wrap;

    Caravan caravan;
    Village village;

    bool isSharing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSharing)
        {
            moneyCaravanText.text = caravan.Gold.ToString();
            woodCaravanText.text = caravan.Wood.ToString();
            ironCaravanText.text = caravan.Iron.ToString();
            foodCaravanText.text = caravan.Food.ToString();

            moneyVillageText.text = village.Gold.ToString();
            woodVillageText.text = village.Wood.ToString();
            foodVillageText.text = village.Food.ToString();
            ironVillageText.text = village.Iron.ToString();
        }
    }

    public void StartShare(Caravan caravanf, Village villagef)
    {
        Wrap.SetActive(true);
        caravan = caravanf;
        village = villagef;
        Time.timeScale = 0f;

        isSharing = true;
    }

    public void StopShare()
    {
        Wrap.SetActive(false);
        Time.timeScale = 1f;

        isSharing = false;
    }

    public void AddCaravanGold()
    {
        if (village.Gold > 0)
        {
            village.Gold--;
            caravan.Gold++;
        }
    }

    public void AddCaravanWood()
    {
        if (village.Wood > 0)
        {
            village.Wood--;
            caravan.Wood++;
        }
    }

    public void AddCaravanIron()
    {
        if (village.Iron > 0)
        {
            village.Iron--;
            caravan.Iron++;
        }
    }

    public void AddCaravanFood()
    {
        if (village.Food > 0)
        {
            village.Food--;
            caravan.Food++;
        }
    }

    public void AddVillageGold()
    {
        if (caravan.Gold > 0)
        {
            village.Gold++;
            caravan.Gold--;
        }
    }

    public void AddVillageWood()
    {
        if (caravan.Wood > 0)
        {
            village.Wood++;
            caravan.Wood--;
        }
    }

    public void AddVillageIron()
    {
        if (caravan.Iron > 0)
        {
            village.Iron++;
            caravan.Iron--;
        }
    }

    public void AddVillageFood()
    {
        if (caravan.Food > 0)
        {
            village.Food++;
            caravan.Food--;
        }
    }
}
