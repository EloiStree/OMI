using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HIDPlugUnplugObserverMono : GenericNewOldListMono<InputDevice>
{
    [ContextMenu("Refresh and notify")]
    public void RefreshAndNotify() {

        base.PushAndRefreshAndNotify(InputSystem.devices.ToArray());
    }
}
