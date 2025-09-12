using TMPro;
using UnityEngine;

public class NewTextMeshPro : TextMeshProUGUI
{
    public string Text
    {
        get
        {
            return this.text;
        }
        set
        {
            this.text = $"<mspace=0.6em>{value}";
            ForceMeshUpdate();
        }
    }
}
