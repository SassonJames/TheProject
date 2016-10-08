using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastUIItem : MonoBehaviour {

	public Text hotkey;
	public Text preSelect;
	public Text postSelect;
	public Text selection;
	public string[] options;

	public void setHotkey (string s) {
		if (s == null)
			return;
		hotkey.text = s;
	}

	public void setPreSelection(string s) {
		if (s == null)
			return;
		preSelect.text = s;
	}

	public void setSelection(string s) {
		if (s == null)
			return;
		selection.text = s;
	}

	public void setPostSelection(string s) {
		if (s == null)
			return;
		postSelect.text = s;
	}

	public string getSelection() {
		return selection.text;
	}
}
