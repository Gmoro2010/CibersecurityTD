using UnityEngine;
using UnityEngine.UI;

public class OpcionesMenu : MonoBehaviour
{
    public Slider sliderVolumen;
    public Toggle togglePantallaCompleta;

    void Start()
    {
        // Cargar valores guardados
        float vol = PlayerPrefs.GetFloat("volumen", 0.5f);
        bool full = PlayerPrefs.GetInt("pantallaCompleta", Screen.fullScreen ? 1 : 0) == 1;

        sliderVolumen.SetValueWithoutNotify(vol);
        togglePantallaCompleta.SetIsOnWithoutNotify(full);

        AplicarVolumen(vol);
        Screen.fullScreen = full;
    }

    public void AplicarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
    }

    public void AplicarPantallaCompleta(bool activo)
    {
        Screen.fullScreen = activo;
        PlayerPrefs.SetInt("pantallaCompleta", activo ? 1 : 0);
    }
}