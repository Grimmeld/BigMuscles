using UnityEngine;

[CreateAssetMenu(fileName = "NewExpression", menuName = "Character/eye")]
public class Eye : ScriptableObject
{
    public string keyName;
    public Texture2D eyeImage;
    public int eyeWidth;
    public int eyeHeight;

}
