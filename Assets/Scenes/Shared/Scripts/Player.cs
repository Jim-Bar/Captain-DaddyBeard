using UnityEngine;
using System.Collections;

public static class Player {

	public class Score {
		private int score = 0;
		private int scorePhase = 0;
		public int Get () { return score + scorePhase; }

		public void Add (int quantity) {
			scorePhase += quantity;
		}

		public void Reset () {
			score = 0;
		}

		// Add the score earned during this phase to the general score.
		public void Commit () {
			score += scorePhase;
			scorePhase = 0;
		}

		// Reset the score earned during this phase and subtract a malus.
		public void Wipe () {
			scorePhase = 0;
			score -= 500;
			if (score < 0)
				score = 0;
		}
	}

	public class Health {
		private const int maxHealth = 6;
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

		public void FillUp () {
			health = maxHealth;
		}
	}

	public class Energy {
		private const int maxEnergy = 1000;
		private int energy = maxEnergy;
		public int Get () { return energy;	}

		public void FillUp () {
			energy = maxEnergy;
		}

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

	public class Weapon {
		private int weapon = 1;
		public int Get () { return weapon; }
		public void Set (int newWeapon) { weapon = newWeapon; }
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

	// Weapon of player 1 and player 2.
	public static Weapon weapon1 = new Weapon ();
	public static Weapon weapon2 = new Weapon ();

	// Player identifier. 0 means server, 1 player 1, 2 player 2.
	public static Identifier id = new Identifier ();
}
