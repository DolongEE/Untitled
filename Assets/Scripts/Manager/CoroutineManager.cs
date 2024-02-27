using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager
{
    Dictionary<float, WaitForSeconds> _time = new Dictionary<float, WaitForSeconds>();

    public WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_time.TryGetValue(seconds, out wfs))
            _time.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }

    public WaitForSeconds WaitForSeconds(Animator anim)
    {
        WaitForSeconds wfs;
        float length = anim.GetCurrentAnimatorStateInfo(0).length;
        if (!_time.TryGetValue(length, out wfs))
            _time.Add(length, wfs = new WaitForSeconds(length - 0.1f));
        return wfs;
    }

    public void Clear()
    {
        _time.Clear();
    }
}
