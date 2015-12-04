using UnityEngine;
using System.Collections;

public interface IMyEvent {

	string MyName {get;}
	int Id {get;}
	string Data {get;}
	object Sender {get;}


}
