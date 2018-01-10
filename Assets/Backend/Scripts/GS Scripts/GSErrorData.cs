using UnityEngine;
using System;

[Serializable]
public class RegistrationError
{
	public string error;
}

[Serializable]
public class GSErrorData
{
	public string error;

	public GSErrorData(string str)
	{
		error = str;
	}
}