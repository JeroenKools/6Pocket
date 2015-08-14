using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SixPocket : MonoBehaviour {

		public enum states {
				SetupNames,
				SetupOrder,
				Main,
				Standing
		}
		private states state;
		private Transform setup;
		private Transform main;
		private Transform standing;
		private Transform setupPanel;
		private Transform backBtn;
		private Transform mainHeader;
		private Transform playerQueue;
		private Transform switchBtn;
		
		private int currentPocket;

		void Start () {
				setup = transform.Find ("SetupScreen");
				setupPanel = setup.Find ("Canvas/Panel");
				main = transform.Find ("MainScreen");
				standing = transform.Find ("StandingScreen");
				backBtn = setup.Find ("Canvas/Back");
				
				mainHeader = main.Find ("Canvas/Panel/Header");
				playerQueue = main.Find ("Canvas/Panel/PlayerQueue");
				switchBtn = main.Find ("Canvas/Panel/Switch");
		
				state = states.SetupNames;
				
				currentPocket = 1;
				
				ColorSetup ();
		}

		void Update () {
				if (Input.GetKeyDown (KeyCode.Tab) && state == states.SetupNames) {
						Selectable current = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable> ();
						Selectable next = current.FindSelectableOnDown ();
			
						if (next != null) {
								InputField inputfield = next.GetComponent<InputField> ();
								if (inputfield != null)
										inputfield.OnPointerClick (new PointerEventData (EventSystem.current));  //if it's an input field, also set the text caret
				
								EventSystem.current.SetSelectedGameObject (next.gameObject, new BaseEventData (EventSystem.current));
						}
				}	
				
				if (state == states.Main) {
						int i = 0;
						mainHeader.GetComponent<Text> ().text = string.Format ("Pocket {0} - It's {1}'s turn!", currentPocket, Global.instance.players [0].playername);
						
						foreach (Transform t in playerQueue) {
								if (i < Global.instance.players.Count) {
										t.GetComponent<Image> ().color = Global.instance.players [i].color;
										t.Find ("Playername").GetComponent<Text> ().text = Global.instance.players [i].playername;
										t.Find ("Strokes").GetComponent<Text> ().text = Global.instance.players [i].strokes.ToString ();
										i++;
								} else {
										t.GetComponent<Image> ().color = Color.black;
								}
						}
				}				
		}
		
		private void ColorSetup () {
				setupPanel.gameObject.GetComponent<Image> ().color = Global.instance.backgroundColor;
		
				int i = 0;
				foreach (Transform t in setupPanel) {
						Transform ballT = t.Find ("Ball");
						if (ballT != null) {
								ballT.gameObject.GetComponent<Image> ().color = Global.instance.colors [i];
								i++;
						}
				}
		}
		
		public void SetupNext () {
				if (state == states.SetupNames) { 
						state = states.SetupOrder;
						Transform title = setupPanel.Find ("Title");
						title.GetComponent<Text> ().text = "Tap the balls to set the initial playing order ";
						backBtn.gameObject.SetActive (true);
						foreach (Transform t in setupPanel) {
								Transform nameT = t.Find ("Playername");
								if (nameT != null) {
										nameT.gameObject.GetComponent<InputField> ().enabled = false;
								}
						}						
				} else if (state == states.SetupOrder) {
						ToMain ();
				}
		}
		
		public void SetupBack () {
				Transform title = setupPanel.Find ("Title");
				title.GetComponent<Text> ().text = "Please enter the players' names";
				state = states.SetupNames;
				backBtn.gameObject.SetActive (false);
		}
		
		public void ToMain () {
				int i = 0;
				foreach (Transform t in setupPanel) {
						Transform nameT = t.Find ("Playername");
						if (nameT != null) {
								string playername = t.Find ("Playername").gameObject.GetComponent<InputField> ().text;
								Global.instance.scores [playername] = new List<int> ();
						}
						i++;
				}
		
		
				setup.gameObject.SetActive (false);
				main.gameObject.SetActive (true);
				state = states.Main;
		}
		
		public void ClearNames () {
				if (state == states.SetupNames) {
						foreach (Transform t in setupPanel) {
								Transform nameT = t.Find ("Playername");
								if (nameT != null) {
										nameT.GetComponent<Text> ().text = "";
										nameT.GetComponent<InputField> ().text = "";
								}
						}
				} else if (state == states.SetupOrder) {
						// TODO:
				}	
		}
		
		public void PlayerClicked (int i) {
				if (state == states.SetupOrder) {
						Transform playerPanel = setupPanel.GetChild (i - 1);
						string playername = playerPanel.Find ("Inputfield/Playername").GetComponent<InputField> ().text;
						print (string.Format ("Player {0}:{1} clicked!", i, playername));
				
						GameObject playerObj = new GameObject ("Player");
						playerObj.AddComponent<SixPocketPlayer> ();
						playerObj.transform.parent = transform.Find ("Players");
						SixPocketPlayer p = playerObj.GetComponent<SixPocketPlayer> ();
						p.playername = playername;
						p.color = Global.instance.colors [i - 1];
				
						if (! Global.instance.players.Contains (p) && playername != "") {
								Global.instance.players.Add (p);
								Transform ball = playerPanel.Find ("Ball");
								Text ballLabel = ball.Find ("BallLabel").GetComponent<Text> ();
								ballLabel.text = Global.instance.players.Count.ToString ();
								
						} else {
								print ("Player already exists?");	
						}		
				} 
		}
		
		public void SwitchMainStanding () {
				if (state == states.Main) {
						state = states.Standing;
						main.gameObject.SetActive (false);
						standing.gameObject.SetActive (true);
				} else {
						state = states.Main;
						main.gameObject.SetActive (true);
						standing.gameObject.SetActive (false);
				}
		}
		
		public void Stroke () {
		}
		
		public void Foul () {
		}
		
		public void Quit () {
				// TODO:
		}
}
