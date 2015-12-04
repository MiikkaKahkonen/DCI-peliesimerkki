using UnityEngine;
using System.Collections;


namespace DCI.Roles
{
	public static class ObserverTrait
	{
		public static void Observe(this Observer self, Observable target)
		{
			target.AddObserver(self);
		}
		public static void QuitObserving(this Observer self, Observable target)
		{
			target.RemoveObserver(self);
		}
	}
}