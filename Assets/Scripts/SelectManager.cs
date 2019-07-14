using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public static IReadOnlyList<Unit> UnitsSelected => _unitsSelected;
    private static readonly List<Unit> _unitsSelected = new List<Unit>();

    private void Start()
    {
        Events.UnitSelect.AddListener(UnitSelectHandler);
        Events.UnitDragSelect.AddListener(UnitDragSelectHandler);
        Events.UnitDeselect.AddListener(UnitDeselectHandler);
    }

    private void UnitDeselectHandler()
    {
        foreach (Unit unit in _unitsSelected)
        {
            unit.DeselectUnit();
        }
        _unitsSelected.Clear();
    }

    private void UnitDragSelectHandler(Unit[] units, bool useModifier)
    {
        if (useModifier)
        {
            foreach (Unit u in units)
            {
                u.SelectUnit();
            }
            _unitsSelected.AddRange(units);
        }
        else
        {
            foreach (Unit u in _unitsSelected)
            {
                u.DeselectUnit();
            }
            _unitsSelected.Clear();

            foreach (Unit u in units)
            {
                u.SelectUnit();
            }
            _unitsSelected.AddRange(units);
        }
    }

    private void UnitSelectHandler(Unit unit, bool useModifier)
    {
        if (useModifier)
        {
            if (_unitsSelected.Contains(unit))
            {
                unit.DeselectUnit();
                _unitsSelected.Remove(unit);
            }
            else
            {
                unit.SelectUnit();
                _unitsSelected.Add(unit);
            }
        }
        else
        {
            foreach (Unit u in _unitsSelected)
            {
                u.DeselectUnit();
            }
            _unitsSelected.Clear();

            unit.SelectUnit();
            _unitsSelected.Add(unit);
        }
    }
}
