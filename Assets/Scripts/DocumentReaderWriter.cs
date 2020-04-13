using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DocumentReaderWriter : MonoBehaviour
{
	public static string path = "Assets/Documents/OfficialEULA.txt";

	[MenuItem("Tools/Read file")]
	public static void ReadString()
	{
		// Read the text directly from the .txt file
		StreamReader reader = new StreamReader(path);
		Debug.Log(reader.ReadToEnd());
		reader.Close();
	}
}
