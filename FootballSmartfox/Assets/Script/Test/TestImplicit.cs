using UnityEngine;
using System.Collections;
class Program : MonoBehaviour
{
	void Start()
	{
		Digit dig = new Digit(7);
		//This call invokes the implicit "double" operator
		double num = dig;
		//This call invokes the implicit "Digit" operator
		Digit dig2 = 12;
		Debug.Log (num);
		Debug.Log (dig2);
		//	Debug.Log("num = {0} dig2 = {1}", num, dig2.val);

	}
}
public class Digit {
	public Digit(double d) { val = d; }
	public double val;
	// ...other members

	// User-defined conversion from Digit to double
	// implicit คือการทำให้ค่าที่ได้รับแปลี่ยนแปลงโดยปริยาย   implicit = โดยปริยาย
	public static implicit operator double(Digit d)
	{
		Debug.Log ("implicit operator double");
		return d.val;
	}
	//  User-defined conversion from double to Digit
	public static implicit operator Digit(double d)
	{
		Debug.Log ("implicit operator Digit");
		return new Digit(d);
	}
}


