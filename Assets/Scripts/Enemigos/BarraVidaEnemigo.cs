using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Slider barraDeVida;
    [SerializeField] private VidaObj sistemaVida; // Arrastra el objeto/enemigo que tiene VidaObj

    private void Start()
    {
        // Si no asignaste el Slider en el Inspector, intenta buscarlo en este mismo GameObject
        if (barraDeVida == null)
        {
            barraDeVida = GetComponent<Slider>();
        }

        // Configuración inicial de la barra
        if (sistemaVida != null && barraDeVida != null)
        {
            barraDeVida.maxValue = sistemaVida.VidaMaxima;
            barraDeVida.value = sistemaVida.VidaActual;
        }
    }

    // Método para actualizar visualmente el Slider desde afuera
    public void ActualizarBarra(float vidaActual, float vidaMaxima)
    {
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }
    }
}