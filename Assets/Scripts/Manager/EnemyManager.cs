using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private Dictionary<EnemyType, EnemyInfoSO>  _enemyDictionary;        

    public void Init()
    {
        _enemyDictionary = CreateEnemyDictionary();
    }

    private Dictionary<EnemyType, EnemyInfoSO> CreateEnemyDictionary()
    {
        EnemyInfoSO[] allMonsters = Resources.LoadAll<EnemyInfoSO>("Monsters");

        Dictionary<EnemyType, EnemyInfoSO> typeToMonsterDic = new Dictionary<EnemyType, EnemyInfoSO>();
        foreach (EnemyInfoSO monsterInfo in allMonsters)
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
