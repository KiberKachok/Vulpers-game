﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Magic;
public class Hex : MonoBehaviour
{
    public Vector2 Coords;
    public Hex[] neighbours = new Hex[6];

    [HideInInspector]
    GameController gameController;

    //[HideInInspector]
    public Structure aboveStructure;

    //[HideInInspector]
    public Unit aboveUnit;

    protected void Awake()
    {
        AddHex();
    }

    public void OnStep(Unit unit)
    {
        if (aboveStructure && aboveStructure as Field)
        {
            (aboveStructure as Field).farm.fields.Remove(aboveStructure as Field);
            Destroy(aboveStructure.gameObject);
            if (unit.team != Team.Neutral)
            {
                unit.FindNearestVillage(unit.team).Food++;
            }
        }

        foreach(Hex hex in neighbours)
        {
            if (hex)
            {
                hex.OnNeighbourStep(unit);
            }
        }
    }

    public void OnNeighbourStep(Unit unit)
    {

    }

    [ContextMenu("Добавить в Хекс в список")]
    public void AddHex()
    {
        if (!gameController)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        if (!gameController.HexDict.ContainsKey(Coords))
        {
            gameController.HexDict.Add(Coords, this);
            //gameController.HexDict.Add(Coords), this);
        }
    }

    [ContextMenu("Выровнять")]
    public void Align()
    {
        if (!gameController)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        int y = Mathf.RoundToInt(transform.position.z / 1.5f);
        int x = Mathf.RoundToInt((transform.position.x - (Mathf.Sqrt(3) / 2f) * Math.Abs(y % 2)) / Mathf.Sqrt(3));
        Coords = new Vector2(x, y);

        transform.position = new Vector3(x * Mathf.Sqrt(3) + (Mathf.Sqrt(3) / 2f) * Math.Abs(y % 2), 0, y * 1.5f);
    }
}
