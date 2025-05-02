  using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateJokersButton : MonoBehaviour
{
    public Transform activatePanel;
    public AudioSource activateSound; // ✅ Add this field for sound

    public void ActivateSelectedJokers()
    {
        List<string> selectedJokers = new List<string>();

        foreach (Transform child in activatePanel)
        {
            JokerCardUI card = child.GetComponentInChildren<JokerCardUI>();
            if (card == null)
            {
                Debug.LogWarning($"No JokerCardUI found on: {child.name}");
                continue;
            }

            if (string.IsNullOrEmpty(card.jokerName))
            {
                Debug.LogWarning($"Card '{child.name}' has no jokerName set!");
                continue;
            }

            selectedJokers.Add(card.jokerName);
        }

        if (selectedJokers.Count > 0)
        {
            // ✅ Play sound before activating
            if (activateSound != null)
            {
                activateSound.Play();
            }

            JokerManager.Instance.ActivateJokers(selectedJokers);
        }
        else
        {
            Debug.LogWarning("No jokers selected for activation.");
        }
    }
}
