using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoatSailHandler : PlayerBoatBehaviour
{
    List<SailMode> modeList = (System.Enum.GetValues(typeof(SailMode)) as SailMode[]).ToList();

    void OnReel(InputValue value)
    {
        int i = modeList.FindIndex(m => m == player.sailMode);
        i = Mathf.Clamp(i - (int)value.Get<float>(), 0, modeList.Count - 1);
        player.sailMode = modeList[i];
        Debug.Log(player.sailMode);
    }
}