using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private Character playerCharacterPrefab;
    [SerializeField] private Character enemyCharacterPrefab;

    private Dictionary<CharacterType, Queue<Character>> disabledCharacters
        = new Dictionary<CharacterType, Queue<Character>>();

    private List<Character> activeCharacter = new List<Character>();

    public Character Player { get; private set; }
    public List<Character> ActiveCharacter => activeCharacter;

    public Character GetCharacter(CharacterType type)
    {
        Character character = null;
        if (disabledCharacters.ContainsKey(type) && disabledCharacters[type].Count > 0)
        {
            character = disabledCharacters[type].Dequeue();
        }
        else if (!disabledCharacters.ContainsKey(type))
        {
            disabledCharacters.Add(type, new Queue<Character>());
        }

        if (character == null)
        {
            character = InstantiateCharacter(type);
        }

        activeCharacter.Add(character);
        return character;
    }

    public void ReturnCharacter(Character character)
    {
        Queue<Character> characters = disabledCharacters[character.CharacterType];
        character.Enqueue(character);
        character.gameObject.SetActive(false);

        activeCharacter.Remove(character);
    }

    private Character InstantiateCharacter(CharacterType type)
    {
        Character character = null;
        switch (type)
        {
            case CharacterType.Player:
                character = Instantiate(playerCharacterPrefab);
                Player = character;
                break;

            case CharacterType.DefaultEnemy:
                character = Instantiate(enemyCharacterPrefab);
                break;

            default:
                Debug.LogError("Unknown character type: " + type);
                break;
        }
        return character;
    }
}
