using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DocumentReaderWriter : MonoBehaviour
{
	private static string path = "Assets/Documents/SampleDocument.txt";

	private void Start()
	{
		ReadString();
		WriteString();
	}

	[MenuItem("Tools/Read file")]
	static private void ReadString()
	{
		// Read the text directly from the .txt file
		StreamReader reader = new StreamReader(path);
		Debug.Log(reader.ReadToEnd());
		reader.Close();
	}

	[MenuItem("Tools/Write file")]
	static private void WriteString()
	{
		StreamWriter saveFile = new StreamWriter(path, false);
		try
		{
			saveFile.WriteLine("Text that is wring over\n");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
		finally
		{
			saveFile.Close();
		}
	}
}
