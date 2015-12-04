using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public interface TextFileStorage
	{
		ITextFileSystem System{get;set;}
	}
}