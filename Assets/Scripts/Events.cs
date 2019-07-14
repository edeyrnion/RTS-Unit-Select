using UnityEngine.Events;

public class EventUnitDragSelect : UnityEvent<Unit[], bool> { }
public class EventUnitSelect : UnityEvent<Unit, bool> { }
public class EventUnitDeselect : UnityEvent { }

public static class Events
{
    public static EventUnitDragSelect UnitDragSelect = new EventUnitDragSelect();
    public static EventUnitSelect UnitSelect = new EventUnitSelect();
    public static EventUnitDeselect UnitDeselect = new EventUnitDeselect();
}
