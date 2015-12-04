using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DCI.Roles
{
	public static class ObservableTrait
	{
		public static void AddObserver(this Observable self,Observer observer)
		{
			self.Observers.Add(observer);
		}
		public static void RemoveObserver(this Observable self,Observer observer)
		{
			self.Observers.Remove(observer);
		}
		public static List<Observer> GetObservers(this Observable self)
		{
			return self.Observers;
		}
		public static void SetObservers(this Observable self,List<Observer> observers)
		{
			self.Observers = observers;
		}

		public static void UpdateObservers(this Observable self,string message)
		{
			foreach(Observer observer in (self.Observers).Reverse<Observer>())
			{
				observer.Update(message);
			}

			self.Changed = false;
		}

		public static void HasChanged(this Observable self)
		{
			self.Changed = true;
		}
	}
}