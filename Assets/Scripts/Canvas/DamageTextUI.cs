using System.Collections;
using UnityEngine;
using TMPro;

public class DamageTextUI : MonoBehaviour
{
    public TextMeshPro damageText;
    public Color textColor;

    public float floatDuration = 0.5f;
    public float disappearTime = 1f;

    private void Start()
    {
        damageText = transform.GetComponent<TextMeshPro>();
        textColor = damageText.color;
    }

    public void SetText(string text)
    {
        damageText.enabled = true;
        damageText.text = text;
        StartCoroutine(FloatDamageText());
    }

    private IEnumerator FloatDamageText()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 1.0f, 0f);

        damageText.fontSize = 5f;

        float elapsedTime = 0f;

        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / floatDuration);

            textColor.a = disappearTime;
            damageText.alpha = textColor.a;

            damageText.transform.LookAt(Camera.main.transform);
            damageText.transform.Rotate(0, 180, 0);

            elapsedTime += Time.deltaTime;
            disappearTime -= Time.deltaTime;
            yield return null;
        }

        damageText.enabled = false;
        Destroy(gameObject);
    }
}