using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UpdatePicture : MonoBehaviour
{
	public enum TabletIndexEnum
	{
		Tablet1 = 1,
		Tablet2 = 2
	};
	
	[SerializeField] private TabletIndexEnum tabletIndex = TabletIndexEnum.Tablet1;
	[SerializeField] private Sprite tabletConnected = null;

	private Image image;

	private void Start () {
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().SoundTheme();
		// Get the component of type Image.
		image = gameObject.GetComponent<Image>();

		if (tabletConnected == null)
			Debug.LogError (GetType().Name + " : No sprite given for the green check mark");

		// If there is already a tablet connected (come back to main menu from the game).
		if (Network.connections.Length > 0)
			MarkAsChecked (Network.connections.Length);
	}

	private void OnPlayerConnected () {
		// Change the red cross in the green checked mark when a tablet connect.
		MarkAsChecked (Network.connections.Length);
	}

	public void MarkAsChecked (int numClients) {
		// Set sprite to the other sprite.
		if (((int) tabletIndex) == numClients)
			image.sprite = tabletConnected;
	}
}