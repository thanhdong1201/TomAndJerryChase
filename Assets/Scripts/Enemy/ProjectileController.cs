using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float flightTime = 3f;
    [SerializeField] private float targetOffset = 5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject warningEffectPrefab;
    [SerializeField] private Vector3 damageForce;

    private Tween missileTween;
    private Vector3 startPos;
    private Vector3 targetPos;

    public void LauchToTarget(Transform target, Vector3 playerVelocity)
    {
        //Calculate the missile's destination
        Vector3 playerFuturePos = target.position + (playerVelocity * flightTime * targetOffset);
        targetPos = new Vector3(playerFuturePos.x, 0f, playerFuturePos.z);

        //Create a warning effect
        Instantiate(warningEffectPrefab, targetPos, Quaternion.identity);

        //Get start position
        startPos = new Vector3(target.position.x, 3f, target.position.z - 10f);
        transform.position = startPos;

        //Create a middle point for parabol
        Vector3 midPoint = (startPos + targetPos) / 2 + Vector3.up * 100f; 

        missileTween = transform.DOPath(new Vector3[] { midPoint, targetPos }, flightTime, PathType.CatmullRom).SetEase(Ease.InOutQuad).OnComplete(Explode);   
    }

    private void Explode()
    {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerCollider>().TakeDamage(damageForce);
            }
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        missileTween?.Kill();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPos, explosionRadius);
    }
}
