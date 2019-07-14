using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class EventUnitDragSelect : UnityEvent<GameObject[], bool> { }
public class EventUnitSelect : UnityEvent<GameObject, bool> { }
public class EventUnitDeselect : UnityEvent { }
public class EventUpdateUnits : UnityEvent<List<GameObject>> { }

public static class Events
{
    public static EventUnitDragSelect UnitDragSelect = new EventUnitDragSelect();
    public static EventUnitSelect UnitSelect = new EventUnitSelect();
    public static EventUnitDeselect UnitDeselect = new EventUnitDeselect();
    public static EventUpdateUnits UpdateUnits = new EventUpdateUnits();
}
