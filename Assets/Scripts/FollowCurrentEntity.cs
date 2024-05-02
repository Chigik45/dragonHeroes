using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurrentEntity : MonoBehaviour
{
    BoardGlobalHolder boardGlobalHolder;
    private void Start()
    {
        boardGlobalHolder = FindObjectOfType<BoardGlobalHolder>();
    }
    private void Update()
    {
        if (boardGlobalHolder.GetCurrentEntity() != null && boardGlobalHolder.GetCurrentEntity().needToBeSimulated)
        {
            transform.position = boardGlobalHolder.tileHolders[boardGlobalHolder.GetCurrentEntity().GetCurrTile()].transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}
