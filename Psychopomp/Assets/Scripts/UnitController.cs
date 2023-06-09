using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public static UnitController Instance { get; private set; }

    [SerializeField] GameObject unitToDestroy;
    public List<GameObject> units = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Find gameobjects with tag Unit
        GameObject unit = GameObject.FindGameObjectWithTag("Unit");
        units.Add(unit);
        unitToDestroy = unit;
    }

    void Update()
    {
        if (units.Count == 4)
        {
            units.Remove(unitToDestroy);
            Destroy(unitToDestroy);
        }

        if (unitToDestroy == null)
        {
            unitToDestroy = units.Find(x => x != null);
        }
    }

    public void addUnit(GameObject unit)
    {
        units.Add(unit);
    }
}
