using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static List<GameObject> AllUnits = new List<GameObject>();

    public Material Normal;
    public Material Selected;

    private Renderer _renderer;

    private void OnEnable()
    {
        AllUnits.Add(gameObject);
    }

    private void Start()
    {
        Events.UpdateUnits.AddListener(UpdateUnitsHandler);
        _renderer = GetComponent<Renderer>();
    }

    private void UpdateUnitsHandler(List<GameObject> units)
    {
        if (units.Contains(gameObject))
        {
            _renderer.material = Selected;
        }
        else
        {
            _renderer.material = Normal;
        }
    }

    private void OnDisable()
    {
        AllUnits.Remove(gameObject);
    }
}
