using System.Collections.Generic;
using UnityEngine;

/*
    ID Player   1000
    ID Enemy    5000
    ID NPC      9000
*/

public class CreatureManager
{
    private Dictionary<int, Creature> _creatureTypeDictionary = new Dictionary<int, Creature>();
    private int _playerId = 0;
    private int _enemyId = 0;
    private int _npcId = 0;

    public void Init()
    {
        Managers.EVENT.creatureEvents.onCreatureCreate += AddCreatureId;
    }   

    public void AddCreatureId(Creature creature)
    {
        int id = 0;
        switch(creature.CreatureType)
        {
            case CreatureType.Player:
                _playerId++;
                id += 1000 + _playerId;
                break;
            case CreatureType.Enemy:
                _enemyId++; 
                id += 5000 + _enemyId;
                break;
            case CreatureType.NPC:
                _npcId++;
                id += 9000 + _npcId;                
                break;
        }
        //Debug.Log($"CreatureType : {creature.CreatureType}, CreatureId : {id}");
        _creatureTypeDictionary.Add(id, creature);
        creature.CreatureID = id;
    }

    public Creature GetCreature(int id)
    {
        if(_creatureTypeDictionary.TryGetValue(id, out Creature creature))
        {
            Debug.Log("Can't found Creature, Check Id");
        }
        return creature;
    }

    public void Clear()
    {
        Managers.EVENT.creatureEvents.onCreatureCreate -= AddCreatureId;
        _creatureTypeDictionary.Clear();
    }
}
