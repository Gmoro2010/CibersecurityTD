using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Configuración de Búsqueda")]
    public string targetTag = "Enemigo"; 
    public float range = 10f;          
    public float updateRate = 0.2f;    
    public float desface = 0f;    

    [Header("Ajustes de Disparo")]
    public GameObject bulletPrefab;    
    [Tooltip("Arrastra aquí uno o más objetos hijos de la torreta que indiquen las puntas de los cañones")]
    public Transform[] firePoints;        
    public float fireRate = 1f;        

    [Header("Ajustes de Rotación")]
    private Transform target;
    private float fireCountdown = 0f;

    void Start()
    {
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning($"¡Falta asignar al menos un FirePoint en el inspector de {gameObject.name}!");
        }

        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
    }

    void Update()
    {
        if (target == null)
            return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + desface);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoints == null || firePoints.Length == 0) return;

        foreach (Transform fp in firePoints)
        {
            if (fp != null)
            {
                Instantiate(bulletPrefab, fp.position, fp.rotation);
            }
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        
        GameObject bestTarget = null;
        int highestWaypointIndex = -1;
        float shortestDistanceToNextPoint = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            // 1. Verificamos primero si está dentro del rango de la torre
            float distanceToTurret = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToTurret <= range)
            {
                // 2. Obtenemos el script de movimiento para leer su avance en el camino
                SeguirCamino caminoScript = enemy.GetComponent<SeguirCamino>();

                if (caminoScript != null && caminoScript.PuntosDelCamino != null)
                {
                    int currentIndex = caminoScript.IndiceSiguientePunto;
                    Transform[] puntos = caminoScript.PuntosDelCamino;

                    // Evitamos errores si el índice excede el arreglo
                    if (currentIndex < puntos.Length)
                    {
                        float distanceToNext = Vector2.Distance(enemy.transform.position, puntos[currentIndex].position);

                        // Regla de selección: 
                        // - Prioridad 1: Quien tenga el índice más alto (más avanzado en los waypoints).
                        // - Prioridad 2: Si tienen el mismo índice, el que esté más cerca del siguiente punto.
                        if (currentIndex > highestWaypointIndex)
                        {
                            highestWaypointIndex = currentIndex;
                            shortestDistanceToNextPoint = distanceToNext;
                            bestTarget = enemy;
                        }
                        else if (currentIndex == highestWaypointIndex)
                        {
                            if (distanceToNext < shortestDistanceToNextPoint)
                            {
                                shortestDistanceToNextPoint = distanceToNext;
                                bestTarget = enemy;
                            }
                        }
                    }
                }
            }
        }

        // Asignamos al objetivo encontrado
        if (bestTarget != null)
        {
            target = bestTarget.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}