using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSpike : SpikeFather
{
    protected override void OnSpikeCollided()
    {
        healthBar.TakeDamage(15);
    }
}
