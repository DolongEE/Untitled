using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyntheticManager
{
    public List<Item> items;

    public void Init()
    {
        // TODO - �÷��̾�Լ� ������ ���� �������� 
        items = Managers.INVENTORY.GetItemInfo();
    }
}
