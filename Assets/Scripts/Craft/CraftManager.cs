using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager
{
    public List<Item> items;

    public void Init()
    {
        // TODO - �÷��̾�Լ� ������ ���� �������� 
        items = Managers.INVENTORY.GetItemInfo();
    }
}
