using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFinish : MonoBehaviour
{
    [SerializeField] ScoreBoard scoreBoard;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Seed") && scoreBoard.gameObject.activeSelf == false)
        {
            scoreBoard.SetActiveTrue();
        }
    }
}
