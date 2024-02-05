using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Creature
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = CreatureType.NPC;
        Managers.EVENT.creatureEvents.CreatureCreate(this);

        return true;
    }
}
