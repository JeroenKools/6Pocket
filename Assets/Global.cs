using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Global : MonoBehaviour
{

		public static Global instance;
		public Dictionary<string,List<int>> scores;
		public List<SixPocketPlayer> players;
		public Color[] colors;
		
		public Color backgroundColor;
		public Color highlightColor;

		void Awake ()
		{
				instance = this;
				scores = new Dictionary<string, List<int>> ();
				players = new List<SixPocketPlayer> ();
		}

		
		void Start ()
		{
	
		}
	

		void Update ()
		{
	
		}
}
