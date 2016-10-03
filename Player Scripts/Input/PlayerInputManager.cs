using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using InControl;

namespace PlayerBindings
{
	public class PlayerActions : PlayerActionSet
	{

		public PlayerAction Menu;
		public PlayerAction Map;
		public PlayerAction Backpack;
		public PlayerAction Sprint;
		public PlayerAction Jump;
		public PlayerAction UseWorld;
        public PlayerAction UseItem;
        public PlayerAction PickUpDropThrow;
		public PlayerAction Swap;
		public PlayerAction BeardAbility;
		public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;
		public PlayerAction Punch;

		public PlayerTwoAxisAction Move;


		public PlayerActions()
		{
			Menu = CreatePlayerAction("Menu");
			Map = CreatePlayerAction("Map");
			Backpack = CreatePlayerAction("Backpack");
			Sprint = CreatePlayerAction("Sprint");
			Jump = CreatePlayerAction("Jump");
			UseWorld = CreatePlayerAction("Use World");
            UseItem = CreatePlayerAction("Use Item");
			PickUpDropThrow = CreatePlayerAction("Pick Up / Drop / Throw");
			Swap = CreatePlayerAction("Swap");
			BeardAbility = CreatePlayerAction("Beard Ability");
			Left = CreatePlayerAction("Move Left");
			Right = CreatePlayerAction("Move Right");
			Up = CreatePlayerAction("Move Up");
			Down = CreatePlayerAction("Move Down");
			Punch = CreatePlayerAction("Punch");
			Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
		}
	}


	public class PlayerInputManager : MonoBehaviour
	{
		
		public PlayerActions playerInput;
		string saveData;


		void OnEnable()
		{
			playerInput = new PlayerActions();

			playerInput.Menu.AddDefaultBinding(Key.Escape);
			playerInput.Menu.AddDefaultBinding(InputControlType.Start);

			playerInput.Map.AddDefaultBinding(Key.M);
            playerInput.Map.AddDefaultBinding(InputControlType.RightStickButton);

			playerInput.Backpack.AddDefaultBinding(Key.B);
			playerInput.Backpack.AddDefaultBinding(InputControlType.Back);

			playerInput.Sprint.AddDefaultBinding(Key.LeftShift);
			playerInput.Sprint.AddDefaultBinding(InputControlType.RightTrigger);

			playerInput.BeardAbility.AddDefaultBinding(InputControlType.Action2);		

			playerInput.Jump.AddDefaultBinding(Key.Space);
			playerInput.Jump.AddDefaultBinding(InputControlType.Action1);

			playerInput.UseWorld.AddDefaultBinding(InputControlType.LeftBumper);
			playerInput.UseWorld.AddDefaultBinding(Key.A);

            playerInput.PickUpDropThrow.AddDefaultBinding(InputControlType.RightBumper);
            playerInput.PickUpDropThrow.AddDefaultBinding(Key.S);

            playerInput.UseItem.AddDefaultBinding(InputControlType.Action4);
            playerInput.UseItem.AddDefaultBinding(Key.Z);

			playerInput.Punch.AddDefaultBinding(InputControlType.Action3);
            playerInput.Punch.AddDefaultBinding(Key.X);	
			
			playerInput.Swap.AddDefaultBinding(InputControlType.LeftTrigger);
			playerInput.Swap.AddDefaultBinding(Key.Tab);

			/*********************
		 	****** MOVEMENT ******
			**********************/

		
			playerInput.Up.AddDefaultBinding(Key.UpArrow);
			playerInput.Up.AddDefaultBinding(InputControlType.LeftStickUp);
			playerInput.Up.AddDefaultBinding(InputControlType.DPadUp);

			playerInput.Right.AddDefaultBinding(Key.RightArrow);
			playerInput.Right.AddDefaultBinding(InputControlType.LeftStickRight);
			playerInput.Right.AddDefaultBinding(InputControlType.DPadRight);

			playerInput.Down.AddDefaultBinding(Key.DownArrow);
			playerInput.Down.AddDefaultBinding(InputControlType.LeftStickDown);
			playerInput.Down.AddDefaultBinding(InputControlType.DPadDown);

			playerInput.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
			playerInput.Left.AddDefaultBinding(InputControlType.DPadLeft);
			playerInput.Left.AddDefaultBinding(Key.LeftArrow);

			playerInput.ListenOptions.MaxAllowedBindings = 5;

			playerInput.ListenOptions.OnBindingFound = (action, binding) =>
			{
				if (binding == new KeyBindingSource(Key.Escape))
				{
					action.StopListeningForBinding();
					return false;
				}
				return true;
			};

			playerInput.ListenOptions.OnBindingAdded += (action, binding) =>
			{
				Debug.Log("Added binding... " + binding.DeviceName + ": " + binding.Name);
			};

			LoadBindings();
		}


		void Update()
		{
			//transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * playerInput.Move.X, Space.World);
			//transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * playerInput.Move.Y, Space.World);

			//var fireColor = playerInput.Fire.IsPressed ? Color.red : Color.white;
			//var jumpColor = playerInput.Jump.IsPressed ? Color.green : Color.white;

			//GetComponent<Renderer>().material.color = Color.Lerp(fireColor, jumpColor, 0.5f);
		}


		void SaveBindings()
		{
			saveData = playerInput.Save();
			PlayerPrefs.SetString("Bindings", saveData);
		}


		void LoadBindings()
		{
			if (PlayerPrefs.HasKey("Bindings"))
			{
				saveData = PlayerPrefs.GetString("Bindings");
				playerInput.Load(saveData);
			}
		}


		void OnApplicationQuit()
		{
			PlayerPrefs.Save();
		}


//		void OnDrawGizmos()
//		{
//			Gizmos.color = Color.blue;
//			var lz = new Vector2(-3.0f, -1.0f);
//			var lp = lz + (InputManager.ActiveDevice.Direction.Vector * 2.0f);
//			Gizmos.DrawSphere(lz, 0.1f);
//			Gizmos.DrawLine(lz, lp);
//			Gizmos.DrawSphere(lp, 1.0f);
//
//			Gizmos.color = Color.red;
//			var rz = new Vector2(+3.0f, -1.0f);
//			var rp = rz + (InputManager.ActiveDevice.RightStick.Vector * 2.0f);
//			Gizmos.DrawSphere(rz, 0.1f);
//			Gizmos.DrawLine(rz, rp);
//			Gizmos.DrawSphere(rp, 1.0f);
//		}
//
//
//		void OnGUI()
//		{
//			const float h = 22.0f;
//			var y = 10.0f;
//
//			GUI.Label(new Rect(10, y, 300, y + h), "Last Input Type: " + playerInput.LastInputType.ToString());
//			y += h;
//
//			var actionCount = playerInput.Actions.Count;
//			for (int i = 0; i < actionCount; i++)
//			{
//				var action = playerInput.Actions[i];
//
//				var name = action.Name;
//				if (action.IsListeningForBinding)
//				{
//					name += " (Listening)";
//				}
//				GUI.Label(new Rect(10, y, 300, y + h), name);
//				y += h;
//
//				var bindingCount = action.Bindings.Count;
//				for (int j = 0; j < bindingCount; j++)
//				{
//					var binding = action.Bindings[j];
//
//					GUI.Label(new Rect(45, y, 300, y + h), binding.DeviceName + ": " + binding.Name);
//					if (GUI.Button(new Rect(20, y + 3.0f, 20, h - 5.0f), "-"))
//					{
//						action.RemoveBinding(binding);
//					}
//					y += h;
//				}
//
//				if (GUI.Button(new Rect(20, y + 3.0f, 20, h - 5.0f), "+"))
//				{
//					action.ListenForBinding();
//				}
//
//				if (GUI.Button(new Rect(50, y + 3.0f, 50, h - 5.0f), "Reset"))
//				{
//					action.ResetBindings();
//				}
//
//				y += 25.0f;
//			}
//
//			if (GUI.Button(new Rect(20, y + 3.0f, 50, h), "Load"))
//			{
//				LoadBindings();
//			}
//
//			if (GUI.Button(new Rect(80, y + 3.0f, 50, h), "Save"))
//			{
//				SaveBindings();
//			}
//
//			if (GUI.Button(new Rect(140, y + 3.0f, 50, h), "Reset"))
//			{
//				playerInput.Reset();
//			}
//		}
	}

}

