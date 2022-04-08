using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public GameObject[] characters;
    public GameObject shield_prefab;
    public Transform playerStartPosition;
    public string menuScene = "Character Selection Menu";
    private string selectedCharacterDataName = "SelectedCharacter";
    int selectedCharacter;
    public GameObject playerObject;
    GameObject shield;
    static int shield_count;

    // Start is called before the first frame update
    public void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt(selectedCharacterDataName,0);
        playerObject = Instantiate(characters[selectedCharacter],playerStartPosition.position,characters[selectedCharacter].transform.rotation);
        shield = Instantiate(shield_prefab, playerStartPosition.position, shield_prefab.transform.rotation);
        shield.transform.parent = playerObject.transform;
        shield.SetActive(false);
        shield_count = 0;
    }

    public void Update()
    {
        if (shield_count < 0)
        {
            shield_count = 0;
        }
        else if (shield_count > 3)
        {
            shield_count = 3;
        }

        if (shield_count > 0)
        {
            shield.SetActive(true);
        }
        else {
            shield.SetActive(false);
        }
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(menuScene);

    }
    public static void toggleShield(int add)
    {
        shield_count += add;

    }


}
