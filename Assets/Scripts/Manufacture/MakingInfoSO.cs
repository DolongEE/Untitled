using UnityEngine;

[CreateAssetMenu(fileName = "MakingInfoSO", menuName = "ScriptableObjects/MakingInfoSO", order = 5)]
public class MakingInfoSO : ScriptableObject
{
    [Header("Material Name")]
    public string materialName;

    [Header("Image")]
    public Sprite sprite;
}
