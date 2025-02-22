using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    [System.Serializable]
    public class BossPhase
    {
        public GameObject bossPhaseTrigger;
        public float triggerDistance;
    }

    [SerializeField] private List<BossPhase> bossPhases;

    private void Start()
    {
        for (int i = 0; i < bossPhases.Count; i++)
        {
            bossPhases[i].bossPhaseTrigger.SetActive(true);
            bossPhases[i].bossPhaseTrigger.transform.position = new Vector3(0, 0, bossPhases[i].triggerDistance);
        }
        bossPhases.Clear();
    }

}
