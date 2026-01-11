using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BossBubble : MonoBehaviour
{

    public GameObject UnpoppedSprite;
    public GameObject PoppedSprite;
    [SerializeField] private int maxRandomDigits = 250;

    public bool isClickable = false;
    public TextMeshProUGUI HappinessCounter;
    public PopCounter HappinessPopCounter;
    public ObjectExploder Exploder;

    private void OnMouseDown()
    {
        if (!isClickable) return;
        isClickable = false;

        UnpoppedSprite.SetActive(false);
        PoppedSprite.SetActive(true);
        PlayRandomPopSound();

        StartCoroutine(TriggerEndCutscene());
    }

    void PlayRandomPopSound()
    {
        List<AudioClip> PopSounds = GlobalReferenceLibrary.library.PopSounds;
        if (PopSounds.Count == 0) return;
        int index = UnityEngine.Random.Range(0, PopSounds.Count);
        AudioClip clip = PopSounds[index];

        // Create a temporary audio source to add pitch and volume variation
        GameObject tempAudio = new GameObject("TempPopAudio");
        tempAudio.transform.position = transform.position;
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f); // Slight pitch variation

        audioSource.pitch = UnityEngine.Random.Range(0.2f, 0.4f);

        audioSource.volume = UnityEngine.Random.Range(0.4f, 1.0f); // Slight volume variation
        audioSource.volume *= Bubble.PopVolume; // Apply global pop volume
        audioSource.Play();
        Destroy(tempAudio, clip.length / audioSource.pitch);
    }

    private IEnumerator TriggerEndCutscene()
    {
        yield return new WaitForSeconds(2f);

        if (HappinessPopCounter != null)
        {
            HappinessPopCounter.enabled = false;
        }

        if (HappinessCounter == null) yield break;

        for (int i = 0; i < 20; i++)
        {
            int digits = Mathf.Clamp(GetFibonacci(i + 1), 1, maxRandomDigits);

            var builder = new StringBuilder("Happiness: ");
            for (int d = Mathf.FloorToInt(GlobalController.Instance.CurrentPops).ToString().Length; d < digits; d++)
            {
                builder.Append(Random.Range(0, 10));
            }

            HappinessCounter.text = builder.ToString();
            yield return new WaitForSeconds(0.2f);
        }



        Exploder.ExplodeAll();


        yield return new WaitForSeconds(5f);
    }

    private int GetFibonacci(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 1;

        int previous = 0;
        int current = 1;

        for (int i = 2; i <= n; i++)
        {
            int next = previous + current;
            previous = current;
            current = next;

            if (current >= maxRandomDigits)
            {
                return maxRandomDigits;
            }
        }

        return current;
    }
}
