﻿using UnityEngine;
using System.Collections;

public class UnlockImbue : MonoBehaviour {

	public string imbueName;

	// Add the new imbue to the unlocked imbues list
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {

			string[] imbueList = Player.unlockedImbues;
			int newLen = imbueList.Length + 1;
			string[] newList = new string[newLen];
			for (int i=0; i<imbueList.Length; i++) {
				newList[i] = imbueList[i];
			}
			newList[newLen-1] = imbueName;
			Player.unlockedImbues = newList;
			// Update the list in the display
			GameManager.reloadImbues();

			Destroy(gameObject);
		}
	}
}
