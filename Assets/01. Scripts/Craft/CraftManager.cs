using System.Collections.Generic;

public class CraftManager
{
    public List<Item> items;

    public void Init()
    {
        // TODO - �÷��̾�Լ� ������ ���� �������� 
        items = Managers.INVENTORY.GetItemInfo();
    }
}
