using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpGeneric : MonoBehaviour {

    public void ShowPopup(Type _popupType, Values _values) {
        switch (_popupType) {
            case Type.InfoText:
                if (_values.Text == string.Empty) {
                    
                }
                break;
            default:
                break;
        }
    }

    

    public enum Type {
        InfoText,
    }

    public class Values {
        public string Text = string.Empty;
        public Color PopColor = Color.black;

        public static Values Warning(string _text) {
            return new Values() { PopColor = Color.yellow };
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ShowPopup(Type.InfoText, new Values() { Text = "Test popup!", PopColor = Color.red });
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ShowPopup(Type.InfoText, Values.Warning("Ocio!"));
        }
    }
}
