using UnityEngine;
using System.Collections;

public static class Player {

	public class Score {
		private int score = 0;
		public int Get () { return score; }

		public void Add (int quantity) {
			score += quantity;
		}

		public void Reset () {
			score = 0;
		}
	}

	public class Health {
		private const int maxHealth = 3;
		private int health = maxHealth;
		public int Get () { return health; }

		// Loose some life. Return 'true' if the players are still alive.
		public bool Subtract (int quantity) {
			health -= quantity;
			if (health <= 0)
			{
				health = 0;
				return false;
			}

			return true;
		}

		// Increase life.
		public void Add (int quantity) {
			health += quantity;
			if (health > maxHealth)
				health = maxHealth;
		}
	}

	public class Energy {
		private const int maxEnergy = 100;
		private int energy = maxEnergy;
		public int Get () { return energy;	}

		public void Add (int quantity) {
			energy += quantity;
			if (energy > maxEnergy)
				energy = maxEnergy;
		}

		public void Burn (int quantity) {
			energy -= quantity;
			if (energy < 0)
				energy = 0;
		}
	}

	// Identifier, can only modify it on Android.
	public class Identifier {
		private int id = 0;
		public int Get () { return id; }

		#if UNITY_ANDROID
		private bool initialized = false;

		public void Set (int newId) {
			if (!initialized)
				id = newId;
			Debug.Log ("Identifier : " + id);
			initialized = true;
		}
		#endif
	}

	// Scores of player 1 and player 2.
	public static Score score1 = new Score ();
	public static Score score2 = new Score ();

	// Common health of both players.
	public static Health health = new Health ();

	// Energy of player 1 and player 2.
	public static Energy energy1 = new Energy ();
	public static Energy energy2 = new Energy ();

	// Player identifier. 0 means server, 1 player 1, 2 player 2.
	public static Identifier id = new Identifier ();
}
