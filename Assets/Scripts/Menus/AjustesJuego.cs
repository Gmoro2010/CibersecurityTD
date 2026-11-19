using UnityEngine;

public static class AjustesJuego
{
    private const string ClaveBarrasEnemigas = "ajustes.barrasEnemigas";
    private const string ClaveReducirDestellos = "ajustes.reducirDestellos";
    private const string ClavePausaAlPerderFoco = "ajustes.pausaAlPerderFoco";

    public static bool MostrarBarrasEnemigas
    {
        get => PlayerPrefs.GetInt(ClaveBarrasEnemigas, 1) == 1;
        set => Guardar(ClaveBarrasEnemigas, value);
    }

    public static bool ReducirDestellos
    {
        get => PlayerPrefs.GetInt(ClaveReducirDestellos, 0) == 1;
        set => Guardar(ClaveReducirDestellos, value);
    }

    public static bool PausarAlPerderFoco
    {
        get => PlayerPrefs.GetInt(ClavePausaAlPerderFoco, 0) == 1;
        set => Guardar(ClavePausaAlPerderFoco, value);
    }

    private static void Guardar(string clave, bool activo)
    {
        PlayerPrefs.SetInt(clave, activo ? 1 : 0);
        PlayerPrefs.Save();
    }
}
