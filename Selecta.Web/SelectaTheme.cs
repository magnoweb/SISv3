using MudBlazor;

namespace Selecta.Web;

/// <summary>
/// Paleta aproximada da identidade visual do SIS v2 (laranja/vermelho quente,
/// ver capturas de tela partilhadas — botão "Entrar", barra lateral, título).
/// Os valores hex são uma aproximação visual, não uma extração exata da
/// marca; ajustar aqui se surgirem os valores oficiais (ex.: guia de marca,
/// CSS do v2).
/// </summary>
public static class SelectaTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#E8590C",
            Secondary = "#D84315",
            AppbarBackground = "#D84315",
            AppbarText = "#FFFFFF",
            Background = "#F5F5F5",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#F2762E",
            Secondary = "#E8590C",
        },
    };
}
