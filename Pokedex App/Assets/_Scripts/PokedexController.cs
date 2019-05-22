using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class PokedexController : MonoBehaviour
{
    [SerializeField] PokedexDataSetter pokedexData;

    public Pokemon pokemon;
    public PokemonInfoRoot pokemonInfo;

    private const string API_SITE = "http://pokeapi.co/api/v2/";
    private const string POKEMON_MAIN = "pokemon/";
    private const string POKEMON_INFO = "pokemon-species/";
    private const string POKEMON_SPRITE = "http://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/";

    private string entryIndex = "1";    

    private HttpWebRequest request;
    private HttpWebResponse response;

    private StreamReader reader;
    private string jsonResponse;

    private UnityWebRequest spriteRequest;

    [SerializeField] private TMP_InputField idSearch;

    private bool canGetNext = true;

    private void Start()
    {
        GetPokemon();
    }

    private void GetPokemon()
    {
        if (canGetNext)
        {
            StartCoroutine(WaitResponse());
        }
    }

    public void NextPokemon()
    {
        entryIndex = (int.Parse(entryIndex) + 1).ToString();
        GetPokemon();
    }

    public void PreviousPokemon()
    {
        if ((int.Parse(entryIndex) > 1))
        {
            entryIndex = (int.Parse(entryIndex) - 1).ToString();
            GetPokemon();
        }
    }

    public void SearchPokemon()
    {
        if (idSearch.text != "" && idSearch.text != "0")
        {
            entryIndex = idSearch.text;
            idSearch.text = "";
            GetPokemon();
        }
        else
        {
            idSearch.text = "";
        }
    }

    private IEnumerator WaitResponse()
    {
        canGetNext = false;
        //Gets pokémon
        request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}{2}", API_SITE, POKEMON_MAIN, entryIndex));
        response = (HttpWebResponse)request.GetResponse();

        reader = new StreamReader(response.GetResponseStream());
        jsonResponse = reader.ReadToEnd();
        pokemon = JsonUtility.FromJson<Pokemon>(jsonResponse);

        //Gets pokémon info
        request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}{2}", API_SITE, POKEMON_INFO, entryIndex));
        response = (HttpWebResponse)request.GetResponse();

        reader = new StreamReader(response.GetResponseStream());
        jsonResponse = reader.ReadToEnd();
        pokemonInfo = JsonUtility.FromJson<PokemonInfoRoot>(jsonResponse);
        //Usage to get dex description in english: pokemonInfo.flavor_text_entries[1].flavor_text;

        pokedexData.SetData();

        //Gets pokémon sprite
        spriteRequest = UnityWebRequestTexture.GetTexture(string.Format("{0}{1}{2}", POKEMON_SPRITE, entryIndex, ".png"));

        yield return spriteRequest.SendWebRequest();

        var texture = DownloadHandlerTexture.GetContent(spriteRequest);
        texture.filterMode = FilterMode.Point;
        pokemon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

        pokedexData.SetSprite();

        canGetNext = true;
    }
}
