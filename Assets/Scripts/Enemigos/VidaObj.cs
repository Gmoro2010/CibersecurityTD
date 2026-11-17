using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VidaObj : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private Slider barraDeVida;
    [SerializeField] private int recompensaDinero = 25;
    
    [Header("Efecto de Daño (Color Rojo)")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Arrastra aquí el SpriteRenderer o se buscará automático
    [SerializeField] private Color colorDano = Color.red;   // Color al recibir daño
    [SerializeField] private float duracionEfectoRojo = 0.15f; // Cuánto dura el color rojo
    private Color colorOriginal;

    private float vidaActual;
    private OleadaManip waveManager;
    private bool muerto;

    public float VidaActual => vidaActual;
    public float VidaMaxima => vidaMaxima;

    void Start()
    {
        vidaActual = vidaMaxima;

        // Si no se asignó manualmente en el inspector, lo busca en el mismo objeto
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color; // Guardamos el color original del sprite
        }

        // Buscamos automáticamente el WaveManager en la escena al aparecer
        waveManager = FindAnyObjectByType<OleadaManip>(); 

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }
    }

    public void RecibirDano(float cantidad)
    {
        if (muerto) return;

        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0, vidaMaxima);
        
        Debug.Log(gameObject.name + " tiene " + vidaActual + " de vida restante.");

        if (barraDeVida != null)
        {
            barraDeVida.value = vidaActual;
        }

        // Activar el efecto visual de daño
        if (spriteRenderer != null)
        {
            StopAllCoroutines(); // Detiene corrutinas previas para evitar conflictos si recibe daño muy seguido
            StartCoroutine(EfectoRojoRutina());
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    IEnumerator EfectoRojoRutina()
    {
        spriteRenderer.color = colorDano;
        yield return new WaitForSeconds(duracionEfectoRojo);
        spriteRenderer.color = colorOriginal;
    }

    private void Morir()
    {
        muerto = true;
        if (waveManager != null)
        {
            waveManager.RegisterEnemyDeath(gameObject);
        }

        // Recompensa al jugador por eliminar al enemigo
        if (BuildManager.Instance != null)
        {
            BuildManager.Instance.GanarDinero(recompensaDinero);
        }

        Destroy(gameObject);
    }
}
