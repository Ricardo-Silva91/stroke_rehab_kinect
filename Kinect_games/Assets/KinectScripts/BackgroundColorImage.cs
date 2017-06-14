using UnityEngine;
using System.Collections;

public class BackgroundColorImage : MonoBehaviour 
{
	[Tooltip("GUI-texture used to display the color camera feed on the scene background.")]
	public GUITexture backgroundImage;


	void Update () 
	{
		KinectManager manager = KinectManager.Instance;

		if (manager && manager.IsInitialized()) 
		{
			if (backgroundImage && (backgroundImage.texture == null)) 
			{
				backgroundImage.texture = manager.GetUsersClrTex();
			}
		}	
	}
}
