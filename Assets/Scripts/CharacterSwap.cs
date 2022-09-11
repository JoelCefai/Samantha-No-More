using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwap : MonoBehaviour
{
    public Transform character;
    public Transform Names;
    public List<Transform> possibleCharacters;
    public List<Transform> NamesChange;
    public int whichCharacter;
    public int whichCharacterName;
    public GameObject NameUI;

    // Start is called before the first frame update
    void Start()
    {
       
        if (character == null & possibleCharacters.Count >= 1)
            if (Names == null & NamesChange.Count >= 1)
        {
            character = possibleCharacters[0];
                Names = NamesChange[0];
        }
        Swap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(whichCharacter == 0)
                if (whichCharacterName == 0)

                {
                    whichCharacter = possibleCharacters.Count - 1;
                    whichCharacterName = NamesChange.Count - 1;

                }

                else
            {
                whichCharacter -= 1;
                    whichCharacterName -= 1;

                }
            Swap();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (whichCharacter == possibleCharacters.Count - 1)
            {
                whichCharacter = 0;
            }
            else
            {
                whichCharacter += 1;
            }
          Swap();
        }
    }

    public void Swap()
    {
        character = possibleCharacters[whichCharacter];
        character.GetComponent<FirstPersonController>().enabled = true;

        for (int i = 0; i <possibleCharacters.Count; i++)
        {
            if(possibleCharacters[i] != character)
            {
                possibleCharacters[i].GetComponent<FirstPersonController>().enabled = false;
            }
        }
    }
}
