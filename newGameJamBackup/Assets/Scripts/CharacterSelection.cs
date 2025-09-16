using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    
    
    [SerializeField] private GameObject femalePrefab;
    [SerializeField] private GameObject characterSelectUI;
    [SerializeField] private Transform spawnPoint;

    private GameObject selectedPrefab;
    private GameObject currentCharacter;

   

    public void SelectFemale()
    {
        selectedPrefab = femalePrefab;
    }

    public void PlayGame()
    {
        if (selectedPrefab == null) return; 

        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        characterSelectUI.SetActive(false); 
    }


}