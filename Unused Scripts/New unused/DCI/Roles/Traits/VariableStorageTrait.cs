using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DCI.Roles
{
	public static class VariableStorageTrait	
	{
		public static bool BoolVar(this VariableStorage self, string name)
		{
			if(self.BoolVariables.ContainsKey(name))
				return self.BoolVariables[name];
			else
				return false;
		}
		public static void BoolVar(this VariableStorage self, string name, bool data)
		{
			if(self.BoolVariables.ContainsKey(name))
				self.BoolVariables[name] = data;
			else
				self.BoolVariables.Add(name,data);
		}

		public static float FloatVar(this VariableStorage self, string name)
		{
			if(self.FloatVariables.ContainsKey(name))
				return self.FloatVariables[name];
			else
				return float.NaN;
		}
		public static void FloatVar(this VariableStorage self, string name, float data)
		{
			if(self.FloatVariables.ContainsKey(name))
				self.FloatVariables[name] = data;
			else
				self.FloatVariables.Add(name,data);
		}
		public static int IntVar(this VariableStorage self, string name)
		{
			if(self.IntVariables.ContainsKey(name))
				return self.IntVariables[name];
			else
				return -1;
		}
		public static void IntVar(this VariableStorage self, string name, int data)
		{
			if(self.IntVariables.ContainsKey(name))
				self.IntVariables[name] = data;
			else
				self.IntVariables.Add(name,data);
		}
		public static Vector3 Vector3Var(this VariableStorage self, string name)
		{
			if(self.Vector3Variables.ContainsKey(name))
				return self.Vector3Variables[name];
			else
				return Vector3.zero;
		}
		public static void Vector3Var(this VariableStorage self, string name, Vector3 data)
		{
			if(self.Vector3Variables.ContainsKey(name))
				self.Vector3Variables[name] = data;
			else
				self.Vector3Variables.Add(name,data);
		}

		public static float TimerVar(this VariableStorage self, string name)
		{
			if(self.Timers.ContainsKey(name))
				return self.Timers[name];
			else
				return float.NaN;
		}
		public static void TimerVar(this VariableStorage self, string name, float data)
		{
			if(self.Timers.ContainsKey(name))
				self.Timers[name] = data;
			else
				self.Timers.Add(name,data);
		}
	}
}