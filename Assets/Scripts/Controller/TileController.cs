using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
