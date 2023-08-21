
using Eloi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HIDPlugUnplugObserverPathStringMono : GenericNewOldListMono<string>
{
    [ContextMenu("Refresh and notify")]
    public void RefreshAndNotify()
    {
        InputSystem.FlushDisconnectedDevices();
        base.PushAndRefreshAndNotify(InputSystem.devices.Select(k=>k.path).ToArray());
    }
}

