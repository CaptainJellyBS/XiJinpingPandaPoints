using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Spawner[] spawners;

    public void SpawnNewLevel(int atrocities, int randomObjects)
    {
        foreach(Spawner s in spawners) { s.ResetSpawner(); }

        Utility.FisherYates(ref spawners);
        for (int i = 0; i < atrocities && i < spawners.Length; i++)
        {
            CanCoverType cct = spawners[i].SpawnAtrocity();
            switch(cct)
            {
                case CanCoverType.Neither: throw new System.ArgumentException("Atrocity should never be uncoverable. Duh.");
                case CanCoverType.Plushie: GameManager.Instance.AmountOfPlushies++; break;
                case CanCoverType.Poster: GameManager.Instance.AmountOfPosters++; break;
                case CanCoverType.Both:
                    if(Random.Range(0.0f,1.0f) >= 0.5f) 
                    { GameManager.Instance.AmountOfPlushies++; break; } 
                    else { GameManager.Instance.AmountOfPosters++; break; }
            }
        }

        for (int i = 0; i < randomObjects && i < spawners.Length - atrocities; i++)
        {
            spawners[i + atrocities].SpawnRandomObject();
        }
    }

    public void SpawnNewLevelTutorial(int atrocities, int randomObjects, Spawner[] _spawners)
    {
        foreach (Spawner s in _spawners) { s.ResetSpawner(); }

        for (int i = 0; i < atrocities && i < _spawners.Length; i++)
        {
            CanCoverType cct = _spawners[i].SpawnAtrocity();
            switch (cct)
            {
                case CanCoverType.Neither: throw new System.ArgumentException("Atrocity should never be uncoverable. Duh.");
                case CanCoverType.Plushie: GameManager.Instance.AmountOfPlushies++; break;
                case CanCoverType.Poster: GameManager.Instance.AmountOfPosters++; break;
                case CanCoverType.Both:
                    if (Random.Range(0.0f, 1.0f) >= 0.5f)
                    { GameManager.Instance.AmountOfPlushies++; break; }
                    else { GameManager.Instance.AmountOfPosters++; break; }
            }
        }

        for (int i = 0; i < randomObjects && i < _spawners.Length - atrocities; i++)
        {
            _spawners[i + atrocities].SpawnRandomObject();
        }
    }
}
