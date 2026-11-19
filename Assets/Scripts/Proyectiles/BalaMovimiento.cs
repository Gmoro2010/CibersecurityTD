using UnityEngine;

public class MovimientoBala : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 15f;
    [SerializeField] private float tiempoDeVidaMaximo = 5f; // Seguridad por si no llega la configuración
    public float Velocidad => velocidad;

    private Vector3 posicionTorre;
    private float rangoMaximo = Mathf.Infinity;
    private bool tieneRestriccionDeRango = false;

    // Método que llama la torre al instanciar la bala
    public void ConfigurarRango(Vector3 origenTorre, float rangoTorre)
    {
        posicionTorre = origenTorre;
        rangoMaximo = rangoTorre;
        tieneRestriccionDeRango = true;
    }

    void Start()
    {
        // Destruir la bala automáticamente si pasa demasiado tiempo sin alcanzar el límite
        Destroy(gameObject, tiempoDeVidaMaximo);
    }

    void Update()
    {
        // Avanza en línea recta en la dirección hacia la que apunta (eje X / derecha local)[cite: 16]
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);

        // Comprobar si la bala ha superado el rango máximo desde la torre
        if (tieneRestriccionDeRango)
        {
            float distanciaALaTorre = Vector3.Distance(transform.position, posicionTorre);
            if (distanciaALaTorre > rangoMaximo)
            {
                Destroy(gameObject); // Desaparece al rebasar el rango
            }
        }
    }
}
