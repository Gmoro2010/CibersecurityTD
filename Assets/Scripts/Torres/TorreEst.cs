using UnityEngine;

public class TurretEstatica : MonoBehaviour
{
    [Header("Configuración de Búsqueda")]
    public string targetTag = "Enemigo"; 
    public float range = 10f;          
    public float updateRate = 0.2f;    

    [Header("Ajustes de Disparo")]
    public GameObject bulletPrefab;    
    public Transform[] firePoints;       
    public float fireRate = 1f;        

    [Header("Control de Fuego")]
    private Transform target;
    private float fireCountdown = 0f;
    public BaseTorre Base { get; set; }

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
        if (Time.timeScale == 0f || (Base != null && Base.Inhabilitada)) return;
        // Reducir el temporizador de disparo constantemente
        if (fireCountdown > 0f)
        {
            fireCountdown -= Time.deltaTime;
        }

        // Si no hay un objetivo en rango, la torreta no dispara
        if (target == null || !target.gameObject.activeInHierarchy ||
            ((Vector2)(target.position - transform.position)).sqrMagnitude > range * range)
            return;

        // Disparar cuando el contador llegue a cero (sin rotar la torre)
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoints == null || firePoints.Length == 0) return;

        foreach (Transform fp in firePoints)
        {
            if (fp != null)
            {
                // Instanciamos la bala respetando la posición y la rotación exacta del FirePoint[cite: 15]
                GameObject balaGO = Instantiate(bulletPrefab, fp.position, fp.rotation);
                
                // Le pasamos la posición de la torre y su rango a la bala para que se destruya al salir de este
                MovimientoBala movimientoBala = balaGO.GetComponent<MovimientoBala>();
                if (movimientoBala != null)
                {
                    movimientoBala.ConfigurarRango(transform.position, range);
                }
            }
        }
    }

    void UpdateTarget()
    {
        if (target != null)
        {
            float distanceToCurrentTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToCurrentTarget <= range)
            {
                return;
            }
            else
            {
                target = null;
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
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
