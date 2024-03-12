using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    AiController aiController;

    private void Start()
    {
        aiController = GetComponent<AiController>();
    }
    bool hasTurned = false;
    public void MakeTurn()
    {
        hasTurned = true;
        aiController.ProcessTurn(this);
    }
    public void NewTurn()
    {
        hasTurned = false;
    }
}
