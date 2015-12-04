using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public static class GameNodeTrait
	{

		public static void MakeQuessable(this GameNode self)
		{
			if((int)self.State > (int)Manager.DCINodeState.Quessable)
			{
				self.State = Manager.DCINodeState.Quessable;
				self.UpdateObservers("HasChanged");
			}
		}

		public static void Reveal(this GameNode self)
		{
			if((int)self.State > (int)Manager.DCINodeState.Quessed)
			{
				self.State = Manager.DCINodeState.Quessed;
				self.UpdateObservers("StateChanged");
			}
		}


		public static void CreateEmptyWord(this GameNode self)
		{
			System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^ ]");
			self.EmptyWord = rgx.Replace(self.Word, "X");
			self.EmptyWord += "("+(self.Word.Replace(" ","")).Length+")";
		}

		public static string Report(this GameNode self)
		{
			string report = "Node: "+self.Id;
			report += "\nPosition: "+self.Position.ToString();
			return report;
		}

		public static void Move(this GameNode self, Vector3 pos)
		{
			self.Position = pos;
			self.UpdateObservers("moved");
		}

		public static void CycleState(this GameNode self)
		{
			int state = (int)self.State;

			if(state == 0)
			{
				self.State = Manager.DCINodeState.Hidden;
			}
			else
			{
				state--;
				self.State = (Manager.DCINodeState)state;
			}

			self.UpdateObservers("state");
		}
	}
}
