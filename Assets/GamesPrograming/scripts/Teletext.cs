using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teletext : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();


        int visChars = text.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visCount = counter % (visChars + 1);

            text.maxVisibleCharacters = visCount;

            if (visCount >= visChars)
            {
                yield return new WaitForSeconds(1.0f);
            }
            counter++;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
