using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InviteFriends : MonoBehaviour
{
    public TextMeshProUGUI linkToCopy;
    public TMP_InputField inputField;
    public TMP_Dropdown suggestionsDropdown;
    // Start is called before the first frame update
    void Start()
    {
        // Register a callback for the input field's value change event
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // Initialize the suggestions dropdown with an empty list
        UpdateSuggestionsDropdown(new List<string>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CopyTextToClipboard()
    {
        // Ensure there is text to copy
        if (linkToCopy != null)
        {
            // Get the text from the Text component
            string text = linkToCopy.text;

            // Copy the text to the clipboard
            GUIUtility.systemCopyBuffer = text;

            Debug.Log("Text copied to clipboard: " + text);
        }
        else
        {
            Debug.LogWarning("Text component not assigned.");
        }
    }
    private List<string> playerList = new List<string>
    {
        "Player1",
        "Player2",
        "Player3",
        // Add more player names as needed
    };
    private void OnInputFieldValueChanged(string newValue)
    {
        //// Filter player names based on the input text
        //List<string> filteredPlayers = playerList.FindAll(player =>
        //    player.ToLower().Contains(newValue.ToLower()));

        //// Update the suggestions dropdown with the filtered player names
        //UpdateSuggestionsDropdown(filteredPlayers);
    }

    private void UpdateSuggestionsDropdown(List<string> suggestions)
    {
        // Clear existing options
        suggestionsDropdown.ClearOptions();

        // Add new options based on the filtered player names
        suggestionsDropdown.AddOptions(suggestions);

        // Show or hide the dropdown based on whether there are suggestions
        suggestionsDropdown.gameObject.SetActive(suggestions.Count > 0);
    }

    // Callback for selecting a suggestion from the dropdown
    public void OnSuggestionSelected(int index)
    {
        // Set the input field text to the selected suggestion
        inputField.text = suggestionsDropdown.options[index].text;

        // Hide the suggestions dropdown
        suggestionsDropdown.gameObject.SetActive(false);
    }

}
