using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public readonly List<GameObject> SelectedUnits = new List<GameObject>();

    private void Start()
    {
        Events.UnitSelect.AddListener(UnitSelectHandler);
        Events.UnitDragSelect.AddListener(UnitDragSelectHandler);
        Events.UnitDeselect.AddListener(UnitDeselectHandler);
    }

    private void UnitDeselectHandler()
    {
        SelectedUnits.Clear();
        Events.UpdateUnits.Invoke(SelectedUnits);

        Debug.Log("UnitDeselectHandler");
    }

    private void UnitDragSelectHandler(GameObject[] units, bool useModifier)
    {
        if (useModifier)
        {
            SelectedUnits.AddRange(units);
        }
        else
        {
            SelectedUnits.Clear();
            SelectedUnits.AddRange(units);
        }

        Events.UpdateUnits.Invoke(SelectedUnits);

        Debug.Log("UnitDragSelectHandler");
    }

    private void UnitSelectHandler(GameObject unit, bool useModifier)
    {
        if (useModifier)
        {
            if (SelectedUnits.Contains(unit))
            {
                SelectedUnits.Remove(unit);
            }
            else
            {
                SelectedUnits.Add(unit);
            }
        }
        else
        {
            SelectedUnits.Clear();
            SelectedUnits.Add(unit);
        }

        Events.UpdateUnits.Invoke(SelectedUnits);

        Debug.Log("UnitSelectHandler");
    }
}
