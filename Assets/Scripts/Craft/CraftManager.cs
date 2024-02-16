using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager
{
    public List<Item> items;

    public void Init()
    {
        // TODO - 플레이어에게서 아이템 정보 가져오기 
        items = Managers.INVENTORY.GetItemInfo();
    }
}
