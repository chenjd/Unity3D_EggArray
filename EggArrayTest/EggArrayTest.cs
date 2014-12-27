/// <summary>
/// Egg array test.Based on Unity3D
/// </summary>
using UnityEngine;
using System.Collections;
using EggToolkit;
public class EggArrayTest : MonoBehaviour {
	EggArray<TargetClass> testArray = new EggArray<TargetClass>();
	// Use this for initialization
	void Start () {

		for(int i = 0; i < 10; i++)
		{
			TargetClass test = new TargetClass(i);
			testArray.Add(test);
		}
//		Test_Difference();
		Test_Invoke();
//		Test_Pluck();
//		Test_Shuffle();
//		Test_Map();
	}

	void Test_Map()
	{
		EggArray<TargetClass> newArray = testArray.Map(delegate(TargetClass item) {
			TargetClass newItem = new TargetClass(1);
			newItem.id = item.id * 10;
			return newItem;
		});
		newArray.Foreach(delegate(TargetClass item) {
			Debug.Log (item.id);
		});
	}

	void Test_Difference()
	{
		EggArray<TargetClass> differentArray = new EggArray<TargetClass>();
		differentArray.Add(testArray.Get(5));
		differentArray.Add(testArray.Get(9));
		testArray.Difference(differentArray).Foreach(delegate(TargetClass item) {
			Debug.Log(item.name);
		});

	}

	void Test_Invoke()
	{
		testArray.Invoke("Hi");
	}

	void Test_Pluck()
	{
		object[] testObj = testArray.Pluck("id");
		string testString = string.Empty;
		for(int i = 0; i < testObj.Length; i++)
		{
			testString = testObj[i].ToString();
			Debug.Log (testString);
		}
	}

	void Test_Shuffle()
	{
		TargetClass[] test = testArray.Shuffle();
		for(int i = 0; i < test.Length; i++)
		{
			Debug.Log (test[i].name);
		}
	}
	
	void Update () {
	
	}
}

public class TargetClass
{
	public int id;
	public string name;

	public TargetClass(int id)
	{
		this.id = id;
		this.name = "NO. " + id;
	}

	public void Hi()
	{
		Debug.Log ("say hi");
	}
}
