using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DocumentReaderWriter : MonoBehaviour
{
	public TextMeshProUGUI textObject;

	public static string path = "Assets/Documents/OfficialEULA.txt";
	private static StreamReader reader;

	public Scene mainMenu;

	private void Start()
	{
		textObject = textObject.GetComponent<TextMeshProUGUI>();

		// Read the text directly from the .txt file
		reader = new StreamReader(path);
		textObject.text = reader.ReadToEnd();

		StartCoroutine(LoadMainMenu(8f));
	}

	IEnumerator LoadMainMenu(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene("MainMenu");
	}

	public static void FinishedDisplayingEula()
	{
		reader.Close();
	}
}
