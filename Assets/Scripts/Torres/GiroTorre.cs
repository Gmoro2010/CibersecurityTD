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
    [Tooltip("Orienta cada FirePoint hacia el objetivo usando su eje X de disparo")]
    public bool apuntarFirePointsAlObjetivo = false;
    [SerializeField] private bool anticiparMovimiento;

    [Header("Ajustes de Rotación")]
    private Transform target;
    private float fireCountdown = 0f;
    public BaseTorre Base { get; set; }
    private float velocidadProyectil;
    private float distanciaBoca;

    void Start()
    {
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning($"¡Falta asignar al menos un FirePoint en el inspector de {gameObject.name}!");
        }

        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
        if (anticiparMovimiento && bulletPrefab != null)
        {
            if (bulletPrefab.TryGetComponent<MovimientoBala>(out var bala)) velocidadProyectil = bala.Velocidad;
            else if (bulletPrefab.TryGetComponent<BalaArea>(out var area)) velocidadProyectil = area.Velocidad;

            if (firePoints != null && firePoints.Length > 0 && firePoints[0] != null)
                distanciaBoca = Vector2.Distance(transform.position, firePoints[0].position);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f || (Base != null && Base.Inhabilitada)) return;

        fireCountdown -= Time.deltaTime;
        if (target == null || !target.gameObject.activeInHierarchy ||
            ((Vector2)(target.position - transform.position)).sqrMagnitude > range * range)
            return;

        Vector2 puntoObjetivo = target.position;
        if (velocidadProyectil > 0f && target.TryGetComponent<SeguirCamino>(out var camino))
        {
            for (int intento = 0; intento < 2; intento++)
            {
                float vuelo = Mathf.Max(0f, Vector2.Distance(transform.position, puntoObjetivo) - distanciaBoca) / velocidadProyectil;
                puntoObjetivo = camino.PredecirPosicion(vuelo);
            }
        }

        Vector2 direction = puntoObjetivo - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + desface);

        if (apuntarFirePointsAlObjetivo)
        {
            ApuntarFirePoints(puntoObjetivo);
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

    }

    void ApuntarFirePoints(Vector2 puntoObjetivo)
    {
        if (firePoints == null) return;

        foreach (Transform fp in firePoints)
        {
            if (fp == null) continue;

            // Usar la posición de cada boca para que los disparos converjan en el enemigo.
            Vector2 direction = puntoObjetivo - (Vector2)fp.position;
            if (direction.sqrMagnitude > 0f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                fp.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
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
        float distanciaRestanteMinima = Mathf.Infinity;

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
                    float distanciaRestante = caminoScript.DistanciaRestante;
                    if (distanciaRestante < distanciaRestanteMinima)
                    {
                        distanciaRestanteMinima = distanciaRestante;
                        bestTarget = enemy;
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
