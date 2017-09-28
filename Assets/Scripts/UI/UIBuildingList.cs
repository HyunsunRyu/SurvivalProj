using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingList : IGUI
{
    [SerializeField] private List<UIItemBuildingList> itemList;

    public override void Init()
    {
        base.Init();
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        for (int i = 0, max = itemList.Count; i < max; i++)
        {
            itemList[i].UpdateUI();
        }
    }
}
