using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    readonly float STAY_TIME = 5.0f;

    bool finishFlag = false;

    [SerializeField] ScoreBoard scoreBoard;

    List<GameObject> seedObjLists;

    private void Start()
    {
        seedObjLists = new List<GameObject>();
    }

    private void Update()
    {
        CountStayTime();
        if (isFinishTime() 
            && !finishFlag 
            && scoreBoard.gameObject.activeSelf == false)
        {
            finishFlag = true;
            GameMain.Instance.SetIsDrop(false);
            scoreBoard.SetActiveTrue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddSeedList(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveSeedList(collision.gameObject);
    }

    private bool isFinishTime()
    {
        foreach (GameObject var in seedObjLists)
        {
            if(var.GetComponent<Seed>().stayTime > STAY_TIME)
                return true;
        }
        return false;
    }

    private void CountStayTime()
    {
        if (seedObjLists.Count == 0)
            return;

        foreach (GameObject var in seedObjLists)
        {
            var.GetComponent<Seed>().stayTime += Time.deltaTime;
        }
    }

    private void ResetStayTime(GameObject obj)
    {
        obj.GetComponent<Seed>().stayTime = 0;
    }

    private void AddSeedList(GameObject obj)
    {
        seedObjLists.Add(obj);
    }

    private void RemoveSeedList(GameObject obj)
    {
        ResetStayTime(obj);
        seedObjLists.Remove(obj);
    }

    private bool isSeedList(GameObject obj)
    {
        if (seedObjLists == null)
            return false;

        foreach (GameObject var in seedObjLists)
        {
            if(var == obj)
                return true;
        }
        return false;
    }
}
