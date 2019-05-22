[System.Serializable]
public class PokemonInfo
{
    public string flavor_text;
}

[System.Serializable]
public class PokemonInfoRoot
{
    public PokemonInfo[] flavor_text_entries;
}
