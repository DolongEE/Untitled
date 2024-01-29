using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private Dictionary<MonsterType, MonsterInfoSO>  _monsterDictionary;
    public Dictionary<MonsterType, MonsterInfoSO> MonsterInfoDic
    {
        get { return _monsterDictionary; }
    }

    public void Init()
    {
        _monsterDictionary = CreateMonsterDictionary();
    }

    private Dictionary<MonsterType, MonsterInfoSO> CreateMonsterDictionary()
    {
        MonsterInfoSO[] allMonsters = Resources.LoadAll<MonsterInfoSO>("Monsters");

        Dictionary<MonsterType, MonsterInfoSO> typeToMonsterDic = new Dictionary<MonsterType, MonsterInfoSO>();
        foreach (MonsterInfoSO monsterInfo in allMonsters)
        {
            if (typeToMonsterDic.ContainsKey(monsterInfo.type))
            {
                Debug.LogWarning($"Duplicate Monster Type exist : {monsterInfo.type}");
            }
            typeToMonsterDic.Add(monsterInfo.type, monsterInfo);
        }
        return typeToMonsterDic;
    }
}
