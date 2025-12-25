using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreNumberRenderer : MonoBehaviour
{
    [SerializeField] private Sprite[] digitSprites;
    [SerializeField] private Image digitPrefab;
    [SerializeField] private Transform container;

    private readonly List<Image> digits = new();

    public void SetScore(int score)
    {
        string scoreStr = score.ToString();

        while(digits.Count < scoreStr.Length)
        {
            Image img = Instantiate(digitPrefab, container);
            digits.Add(img);
        }

        for(int i = 0; i<digits.Count; i++)
        {
            if(i< scoreStr.Length)
            {
                int num = scoreStr[i] - '0';
                digits[i].sprite = digitSprites[num];
                digits[i].gameObject.SetActive(true);
            }

            else
            {
                digits[i].gameObject.SetActive(false);
            }
        }
    }
}
