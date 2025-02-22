using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossPhase : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private GameObject projectilePrefab;
    [Header("Broadcast from Events")]
    [SerializeField] private string warningText;
    [SerializeField] private StringEventChannelSO setWarningEvent;

    private void OnTriggerEnter(Collider other)
    {
        enemyController.SetUpProjectile(projectilePrefab);
        enemyController.SetState(new MissleAttackState(enemyController));
        setWarningEvent.RaiseEvent(warningText);
    }
}
