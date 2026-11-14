using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [Header("Paneles del menú")]
    [SerializeField] private GameObject panelBotones;     // el objeto "opciones" (PLAY, OPTIONS, QUIT)
    [SerializeField] private GameObject panelNiveles;     // el objeto "PanelNiveles"

    [Header("Escenas")]
    [SerializeField] private string nombreEscenaMenu = "Menu principal";

    // Márcalo solo en el botón PLAY. En un botón VOLVER déjalo desmarcado.
    [SerializeField] private bool abrirSelectorAlHacerClic = true;

    private void Start()
    {
        if (!abrirSelectorAlHacerClic) return;

        Button boton = GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.AddListener(CargarSelectorNiveles);
        }
    }

    // Oculta los botones del menú y muestra el selector de niveles
    public void CargarSelectorNiveles()
    {
        if (panelBotones != null) panelBotones.SetActive(false);
        if (panelNiveles != null) panelNiveles.SetActive(true);
    }

    // Vuelve del selector a los botones del menú (dentro de la misma escena)
    public void VolverDesdeNiveles()
    {
        if (panelNiveles != null) panelNiveles.SetActive(false);
        if (panelBotones != null) panelBotones.SetActive(true);
    }

}