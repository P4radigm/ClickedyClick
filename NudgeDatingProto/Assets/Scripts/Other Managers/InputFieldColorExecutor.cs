using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldColorExecutor : MonoBehaviour
{
    public ColorManager.ColorType colorTypeCaret;
    public ColorManager.ColorType colorTypeSelection;
    private TMP_InputField ownInputField;

    public void UpdateColor(Color[] newColors)
    {
        if (ownInputField == null) { ownInputField = GetComponent<TMP_InputField>(); }

        Color currentCaretColor = ownInputField.caretColor;
        Color currentSelectionColor = ownInputField.selectionColor;
        ownInputField.caretColor = new Color(newColors[(int)colorTypeCaret].r, newColors[(int)colorTypeCaret].g, newColors[(int)colorTypeCaret].b, currentCaretColor.a);
        ownInputField.selectionColor = new Color(newColors[(int)colorTypeSelection].r, newColors[(int)colorTypeSelection].g, newColors[(int)colorTypeSelection].b, currentSelectionColor.a);
    }
}
