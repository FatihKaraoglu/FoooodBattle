using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    [SerializeField]
    public int UnitType;
    [SerializeField]
    public string Name;
    [SerializeField]
    public string Description;
    [SerializeField]
    public int Attack;
    [SerializeField]
    public int Health;
    [SerializeField]
    public int Energy;
    [SerializeField]
    public int partLevelCount;
    [SerializeField]
    public int partLevelMax;
    [SerializeField]
    public int Level;
    public UnitSlot unitSlot;

    public bool bought = false;

    public int Id;
}
