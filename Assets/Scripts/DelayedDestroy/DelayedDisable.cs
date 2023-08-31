using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDisable : DelayedDestruction
{
    protected override void Start() { }

    private void OnEnable()
    {
        DelayedSetActiveFalse();
    }
}
