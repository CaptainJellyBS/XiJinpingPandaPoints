using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] possibleAtrocities, possibleRandomObjects;
    public CanCoverType canBeCoveredBy;

    private void Start()
    {
        ResetSpawner();
    }

    public CanCoverType SpawnAtrocity()
    {
        GameObject go = Utility.Pick(possibleAtrocities);
        go.SetActive(true);
        return canBeCoveredBy;
    }

    public CanCoverType SpawnAtrocity(int index)
    {
        GameObject go = possibleAtrocities[index];
        go.SetActive(true);
        return canBeCoveredBy;
    }

    public CanCoverType SpawnRandomObject()
    {
        GameObject go = Utility.Pick(possibleRandomObjects);
        go.SetActive(true);
        return canBeCoveredBy;
    }

    public CanCoverType SpawnRandomObject(int index)
    {
        GameObject go = possibleRandomObjects[index];
        go.SetActive(true);
        return canBeCoveredBy;
    }

    public void ResetSpawner()
    {
        foreach(GameObject go in possibleRandomObjects)
        {
            go.SetActive(false);
            go.GetComponent<CoverableObject>().enabled = true;
        }
        foreach(GameObject go in possibleAtrocities)
        {
            go.SetActive(false);
            go.GetComponent<CoverableObject>().enabled = true;
        }
    }
}
