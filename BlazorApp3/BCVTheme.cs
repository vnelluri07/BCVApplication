namespace BlazorApp3
{
    public class BCVTheme
    {
        public MudTheme THEME { get; } = new MudTheme
        {
            Palette = new PaletteLight()
            {
                Primary = "#1a1a1a",
                PrimaryContrastText = "#ffffff",
                Secondary = "#6b6b6b",
                AppbarText = "#1a1a1a",
                AppbarBackground = "rgba(255,255,255,0.95)",
                Black = "#1a1a1a",
                TextPrimary = "#1a1a1a",
                TextSecondary = "#6b6b6b",
                Background = "#ffffff",
                Surface = "#ffffff",
                Success = "#2d8a4e",
                Error = "#c0392b",
                Info = "#999999"
            },
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = new[] { "Inter", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "Roboto", "sans-serif" }
                },
                H1 = new H1() { FontFamily = new[] { "Playfair Display", "Georgia", "serif" } },
                H2 = new H2() { FontFamily = new[] { "Playfair Display", "Georgia", "serif" } },
                H3 = new H3() { FontFamily = new[] { "Playfair Display", "Georgia", "serif" } },
                H4 = new H4() { FontFamily = new[] { "Playfair Display", "Georgia", "serif" } }
            },
            LayoutProperties = new LayoutProperties { }
        };
    }
}
