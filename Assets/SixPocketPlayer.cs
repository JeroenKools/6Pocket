using UnityEngine;
using System.Collections;

public class SixPocketPlayer : MonoBehaviour {
		public string playername;
		public bool isIn;
		public Color color;
		public int strokes;
	
		void Awake () {
				isIn = false;
				playername = "";
				strokes = 0;
		}
		
		public override bool Equals (object b) {
				SixPocketPlayer playerB = b as SixPocketPlayer;
				return playername == playerB.playername;
		}
}
