using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Structure : MonoBehaviour
{

    [Header("Состояния")]
    public float currentHp = 100;
    public float maxHp = 100;
    public Team team = Team.Neutral;

    GameController gameController;
    [HideInInspector]
    public Hex underHex;

    // Start is called before the first frame update
    protected void Start()
    {
        Align();
        if (!gameController)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Выровнять")]
    public void Align()
    {
        LayerMask layerMask = 1 << 8;

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            underHex = hit.collider.GetComponent<Hex>();
        }

        if (underHex)
        {
            underHex.aboveStructure = this;
        }

        if (underHex)
        {
            transform.position = new Vector3(underHex.transform.position.x, 0, underHex.transform.position.z);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHp > damage)
        {
            currentHp -= damage;
        }
        else
        {
            currentHp = 0;
        }

        if (currentHp <= 0)
        {
            Village thisVillage = this as Village;
            //Destroy(this.gameObject);
            if (thisVillage)
            {
                team = Team.Neutral;
                (thisVillage).state = VillageState.Ruined;
                (thisVillage).Deactivate();
            }
            else
            {
                if(this is IRes)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
