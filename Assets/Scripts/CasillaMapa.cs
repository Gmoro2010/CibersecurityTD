using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el Nuevo Input System

[RequireComponent(typeof(Collider2D))]
public class CasillaMapa : MonoBehaviour
{
    private GameObject torreInstalada;
    private TorreItem torreItemInstalado; // Guarda los datos de la torre para saber su costo

    [Header("Configuración de Venta")]
    [Range(0f, 1f)] [SerializeField] private float porcentajeReembolso = 0.8f; // 80% del valor original devuelto

    // El clic izquierdo mediante OnMouseDown sigue funcionando por compatibilidad con Collider2D
    private void OnMouseDown()
    {
        // Si la casilla está vacía, intenta construir la torre seleccionada
        if (torreInstalada == null && BuildManager.Instance != null && BuildManager.Instance.PuedeConstruir)
        {
            torreItemInstalado = BuildManager.Instance.TorreParaConstruir;
            torreInstalada = BuildManager.Instance.ConstruirTorreEn(transform.position);
        }
    }

    // Detecta el cursor sobre la casilla usando el Nuevo Input System
    private void OnMouseOver()
    {
        // Comprueba si el ratón existe y si se presionó el botón derecho en este frame exacto
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame && torreInstalada != null)
        {
            VenderTorre();
        }
    }

    private void VenderTorre()
    {
        if (torreItemInstalado != null && BuildManager.Instance != null)
        {
            // Calcular el reembolso basado en el costo del ScriptableObject
            int reembolso = Mathf.RoundToInt(torreItemInstalado.costo * porcentajeReembolso);
            BuildManager.Instance.GanarDinero(reembolso);
            Debug.Log($"Torre vendida con clic derecho. Reembolso obtenido: ${reembolso}");
        }

        // Destruir el objeto de la torreta en la escena
        if (torreInstalada != null)
        {
            Destroy(torreInstalada);
        }

        // Limpiar referencias para dejar la casilla libre nuevamente
        torreInstalada = null;
        torreItemInstalado = null;
    }
}