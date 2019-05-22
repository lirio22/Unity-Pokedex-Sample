using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexDataSetter : MonoBehaviour
{
    [SerializeField] private PokedexController pokedex;

    [SerializeField] private Image pokemonSprite;
    [SerializeField] private TextMeshProUGUI pokemonTopName;
    [SerializeField] private TextMeshProUGUI pokemonBottonName;
    [SerializeField] private TextMeshProUGUI pokemonDescription;

    public void SetData()
    {
        pokemonTopName.text = string.Format("{0}{1}{2}{3}", "#", pokedex.pokemon.id.ToString(), " - ", pokedex.pokemon.name);
        pokemonBottonName.text = string.Format("{0}{1}{2}{3}", "#", pokedex.pokemon.id.ToString(), " - ", pokedex.pokemon.name);
        pokemonDescription.text = pokedex.pokemonInfo.flavor_text_entries[1].flavor_text;
        Debug.Log(pokedex.pokemonInfo.flavor_text_entries[1].flavor_text);
    }

    public void SetSprite()
    {
        pokemonSprite.sprite = pokedex.pokemon.sprite;
        pokemonSprite.SetNativeSize();
        pokemonSprite.rectTransform.localScale = new Vector3(10.0f, 10.0f, 0);
    }
}
