using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventUnitDragSelect : UnityEvent<GameObject[], bool> { }
    [System.Serializable] public class EventUnitSelect : UnityEvent<GameObject, bool> { }
    [System.Serializable] public class EventUnitDeselect : UnityEvent { }
}
