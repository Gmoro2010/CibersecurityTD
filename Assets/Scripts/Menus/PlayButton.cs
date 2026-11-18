using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [Header("Paneles del menú")]
    [SerializeField] private GameObject panelBotones;     // el objeto "opciones" (PLAY, OPTIONS, QUIT)
    [SerializeField] private GameObject panelNiveles;     // el objeto "PanelNiveles"

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