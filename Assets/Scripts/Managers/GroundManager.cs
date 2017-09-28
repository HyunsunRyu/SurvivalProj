using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : Singleton<GroundManager>
{
    protected override void Init()
    {
        MapData.Instance.CreateTiles();
    }
}
