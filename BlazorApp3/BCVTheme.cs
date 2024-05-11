namespace BlazorApp3
{
    public class BCVTheme
    {
        public MudTheme THEME { get; } = new MudTheme
        {
            Palette = new PaletteLight()
            {
                Primary = "#022f40", // Pursian Blue
                PrimaryContrastText = "#f2f2f2", // Air Force Blue
                Dark = "#f2f2f2", // Reversed this since there is no Light. need it to make the links white.
                AppbarText = Colors.Shades.White,
                Secondary = "#780000", // want burguendy
                AppbarBackground = "#588390",
                Black = "#444444",
                TextPrimary = "#444444",
                Background = "#f2f2f2",
                Surface = "#fefefe",
                Success = "#28a745",
                Info = "#cccccc"

            },
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = new[] { "Courier New", "Helvetica", "Arial", "sans-serif" }
                }
            },
            LayoutProperties = new LayoutProperties { }
        };
    }
}
