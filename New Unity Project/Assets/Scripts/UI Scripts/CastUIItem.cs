using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastUIItem : MonoBehaviour {

	public Text hotkey;
	public Text preSelect;
	public Text postSelect;
	public Text selection;
	public string[] options;
	public int page;

	public void setHotkey (string s) {
		if (s == null)
			return;
		hotkey.text = s;
	}

	// Set the state to what it should be on first load.
	public void reset() {
		preSelect.text = "";
		selection.text = "";
		postSelect.text = "";
		page = 0;

		if (options == null)
			return;

		if (options.Length > 0)
			selection.text = options [0];
		if (options.Length > 1)
			postSelect.text = options [1];
	}

	// Set the text in the game objects to what they should be based on the page
	public void refresh() {
		if (options == null)
			return;

		if (page > 0 && options.Length > page - 1) 
			setPreSelection (options [page - 1]);
		else
			setPreSelection ("");
		
		if (options.Length > page)
			setSelection (options [page]);
		else
			setSelection ("");
		
		if (options.Length > page + 1)
			setPostSelection (options [page + 1]);
		else
			setPostSelection ("");
	}

	public void setPreSelection(string s) {
		if (s == null)
			preSelect.text = "";
		preSelect.text = s;
	}

	public void setSelection(string s) {
		if (s == null)
			selection.text = "";
		selection.text = s;
	}

	public void setPostSelection(string s) {
		if (s == null)
			postSelect.text = "";
		postSelect.text = s;
	}

	public string getSelection() {
		return selection.text;
	}

	public void scrollUp() {
		page++;
		refresh ();
	}

	public void scrollDown() {
		page--;
		refresh ();
	}
}
