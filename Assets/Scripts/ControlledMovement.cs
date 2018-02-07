using UnityEngine;
using System.Collections;

public class ControlledMovement : MonoBehaviour {
	
	public float speed;
	
	public enum directions{
		Up,
		Down,
		Right,
		Left,
		None
	}public directions direction = directions.None, selectedDirection;
	
	// Update is called once per frame
	void Update () {
		Move();
	}
	
	void Move () {
		switch (direction)
		{
			case directions.Up:
				transform.Translate(Vector3.up * (Time.deltaTime * speed), Space.World);
			break;
			case directions.Down:
				transform.Translate(Vector3.up * (-Time.deltaTime * speed), Space.World);
			break;
			case directions.Right:
				transform.Translate(Vector3.right * (Time.deltaTime * speed), Space.World);
			break;
			case directions.Left:
				transform.Translate(Vector3.right * (-Time.deltaTime * speed), Space.World);
			break;
		}
	}
	
	// public void UpdateDir (directions d) {
	public void UpdateDir (string d) {
		direction = (directions) System.Enum.Parse(typeof(directions), d);
	}
}
