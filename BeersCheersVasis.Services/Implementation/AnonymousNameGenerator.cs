namespace BeersCheersVasis.Services.Implementation;

public sealed class AnonymousNameGenerator : IAnonymousNameGenerator
{
    private static readonly string[] Names =
    [
        // Greek mythology
        "Apollo", "Athena", "Hermes", "Artemis", "Poseidon", "Hera", "Zeus", "Persephone",
        "Odysseus", "Achilles", "Icarus", "Orpheus", "Medusa", "Phoenix", "Atlas", "Pandora",
        // Norse mythology
        "Odin", "Thor", "Loki", "Freya", "Valkyrie", "Fenrir", "Baldur", "Heimdall",
        // Pop culture / movies
        "Gandalf", "Yoda", "Neo", "Morpheus", "Trinity", "Ripley", "Groot", "Rocket",
        "Stark", "Leia", "Solo", "Chewie", "Spock", "Picard", "Mulder", "Scully",
        // Literature
        "Gatsby", "Sherlock", "Watson", "Atticus", "Frodo", "Aragorn", "Hermione", "Dumbledore",
        // Anime / games
        "Saitama", "Goku", "Naruto", "Zelda", "Link", "Mario", "Kirby", "Pikachu",
        // Misc cool names
        "Maverick", "Cipher", "Echo", "Nova", "Raven", "Shadow", "Blaze", "Phantom",
        "Cosmo", "Nebula", "Quasar", "Vortex", "Zenith", "Onyx", "Pixel", "Glitch"
    ];

    private static readonly Random Rng = new();

    public string Generate()
    {
        var name = Names[Rng.Next(Names.Length)];
        var suffix = Rng.Next(100, 9999);
        return $"{name}{suffix}";
    }
}
