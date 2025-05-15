using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedSpike : SpikeFather
{
    protected override void OnSpikeCollided()
    {
        healthBar.BeginDOTDamage(10, 3);
    }
}
