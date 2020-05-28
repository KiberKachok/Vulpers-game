using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;

public class Unit : MonoBehaviour
{
    [Header("Состояния")]
    public float currentHp = 100;
    public float maxHp = 100;
    public Team team;

    GameController gameController;

    [HideInInspector]
    public Hex underHex;

    Coroutine moveProcess;

    [HideInInspector]
    public bool isMoving = false;

    public void Start()
    {
        Align();
        if (!gameController)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    public void Move(Hex hex)
    {
        if(!isMoving && underHex.neighbours.Contains(hex) && (hex.aboveStructure == null || hex.aboveStructure as Field) && hex.aboveUnit == null)
        {
            //underHex.aboveUnit = null;
            //underHex = hex;
            //underHex.aboveUnit = this;
            //hex.OnStep(this);
            moveProcess = StartCoroutine(MoveCommand(hex));
        }
    }

    public IEnumerator MoveCommand(Hex hex)
    {
        if (!isMoving && underHex.neighbours.Contains(hex) && (hex.aboveStructure == null || hex.aboveStructure as Field) && hex.aboveUnit == null)
        {
            underHex.aboveUnit = null;
            underHex = hex;
            underHex.aboveUnit = this;
            hex.OnStep(this);
            //moveProcess = StartCoroutine(MoveCommand(hex));

            isMoving = true;
            while (transform.position != hex.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, hex.transform.position, Time.deltaTime * 2f);
                yield return null;
            }
            isMoving = false;
        }
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
            underHex.aboveUnit = this;
            transform.position = new Vector3(underHex.transform.position.x, 0, underHex.transform.position.z);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public Village FindNearestVillage(Team team)
    {
        Village village = null;
        List<Village> villages = FindObjectsOfType<Village>().ToList();

        float distanceBetween = Mathf.Infinity;
        float dist = 0;
        foreach (Village i in villages)
        {
            dist = Vector3.Distance(i.transform.position, transform.position);

            if (dist < distanceBetween && i.team == team && i.state == VillageState.Active)
            {
                village = i;
                distanceBetween = dist;
            }
        }

        return village;
    }
}
