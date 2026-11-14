using UnityEngine;

public class BalaArea : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float tiempoDeVidaMaximo = 5f; 

    [Header("Ajustes de Daño en Área (AoE)")]
    [SerializeField] private float radioExplosion = 3.5f; 
    [SerializeField] private float danoDeAtaque = 25f;     
    [SerializeField] private string tagEnemigo = "Enemigo";

    private bool haExplotado = false;
    private Vector3 posicionTorre;
    private float rangoMaximo = Mathf.Infinity;
    private bool tieneRestriccionDeRango = false;

    // Método que llama la torre al disparar para asignarle los límites
    public void ConfigurarRango(Vector3 origenTorre, float rangoTorre)
    {
        posicionTorre = origenTorre;
        rangoMaximo = rangoTorre;
        tieneRestriccionDeRango = true;
    }

    void Start()
    {
        // Destruir la bala automáticamente si pasa demasiado tiempo sin impactar
        Destroy(gameObject, tiempoDeVidaMaximo);
    }

    void Update()
    {
        if (haExplotado) return;

        // Moverse constantemente hacia adelante según la rotación de su FirePoint
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);

        // Comprobar si la bala ha superado el rango máximo desde la torre
        if (tieneRestriccionDeRango)
        {
            float distanciaALaTorre = Vector3.Distance(transform.position, posicionTorre);
            if (distanciaALaTorre > rangoMaximo)
            {
                Destroy(gameObject); // Desaparece sin explotar al rebasar el rango
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con un enemigo, detona la nube tóxica
        if (other.CompareTag(tagEnemigo) && !haExplotado)
        {
            Explotar();
        }
    }

    void Explotar()
    {
        haExplotado = true;

        // Detectar todos los colliders dentro del radio de la nube tóxica en la posición actual
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioExplosion);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag(tagEnemigo))
            {
                VidaObj sistemaVidaEnemigo = col.GetComponent<VidaObj>();

                if (sistemaVidaEnemigo != null)
                {
                    sistemaVidaEnemigo.RecibirDano(danoDeAtaque);
                    Debug.Log($"¡Nube tóxica dañó a {col.gameObject.name} por {danoDeAtaque} de daño!");
                }
            }
        }

        // Destruir el proyectil al explotar
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}