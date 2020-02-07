using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[Tooltip("The character the camera is following.")]
	public GameObject target;

	[Tooltip("The speed the camera moves at.")]
	public float speed;

	[Header("Camera Boundaries")]
	[Tooltip("Whether to use camera boundaries. Can only be set if boundingObject is set.")]
	public bool usesCameraBounds = false;

	[Tooltip("The object the camera is bounded by.")]
	public GameObject boundingObject;

	// make sure there's a bounding object before enabling the useCameraBounds flag
	void OnValidate(){
		if (!boundingObject) {
			usesCameraBounds = false;
		}
	}

	// the x/y boundaries for the camera
	private float _xBoundaryMin;
	private float _xBoundaryMax;
	private float _yBoundaryMin;
	private float _yBoundaryMax;


	[Header("Paralax Settings")]
	[Tooltip("Paralaxing Objects.")]
	public GameObject[] paralax;

	[Tooltip("The master scroll speed of paralaxed objects.")]
	public float masterScrollSpeed;

	// the camera's initial position
	public Vector3 initialPosition;

	void Awake() {
		// calculate camera height and width
		float cameraHeight = GetComponent<Camera>().orthographicSize;
		float cameraWidth = cameraHeight * ( (float) Screen.width / (float) Screen.height ) ;

		// if the bounding object is assigned
		if (boundingObject != null) {

			// the center of the bounding object
			float boundaryCenterX = boundingObject.GetComponent<SpriteRenderer>().bounds.center.x;
			float boundaryCenterY = boundingObject.GetComponent<SpriteRenderer>().bounds.center.y;

			// the height and width of the bounding object
			float boundaryExtentsX = boundingObject.GetComponent<SpriteRenderer>().bounds.extents.x;
			float boundaryExtentsY = boundingObject.GetComponent<SpriteRenderer>().bounds.extents.y;

			// calculate the min and max boundaries
			_xBoundaryMax = boundaryCenterX + boundaryExtentsX - cameraWidth;
			_yBoundaryMax = boundaryCenterY + boundaryExtentsY - cameraHeight;
			_xBoundaryMin = boundaryCenterX + cameraWidth  - boundaryExtentsX;
			_yBoundaryMin = boundaryCenterY + cameraHeight - boundaryExtentsY;
		}

		initialPosition = transform.position;
	}

	// LateUpdate is called after Update each frame
	// Updates camera position based on player position, limited by boundingObject it usesCameraBounds
	void LateUpdate ()
	{
		paralaxing();
		slideToward( target);
//		snapTo( player );
	}

	void paralaxing() {
		Vector3 cameraPositionDifferenc = this.initialPosition - this.transform.position;

		foreach(GameObject p in paralax) {
			
			BackgroundForeground bf = p.GetComponent<BackgroundForeground>();
			Vector3 bfInitialPosition = bf.initialPosition();

			// move 
			p.transform.position = bfInitialPosition - ( cameraPositionDifferenc * bf.scrollSpeed * masterScrollSpeed ) ;

			// reset z location 
			p.transform.position = new Vector3( bf.transform.position.x, bf.transform.position.y, bfInitialPosition.z );
		}
	}

	/// <summary>
	/// Slides toward the passed object.
	/// </summary>
	/// <param name="gObj">G object.</param>
	public void slideToward( GameObject moveTowardThis ) {
		
		// the fraction of the distance toward the player a single move of assigned speed would take;
		float slideDist = speed / Vector3.Distance( this.transform.position, moveTowardThis.transform.position );

		// the vector of movement of slideDist toward toe target position; note that if slideDist > 1, this will point to the exact target position
		Vector3 moveVector = Vector3.Lerp( this.transform.position, moveTowardThis.transform.position, slideDist );

		// TODO: handle when the character dies (note that the level will restart when the character dies)
		transform.position = positionBounded( moveVector );
	}

	/// <summary>
	/// If using camera bounds, clamp the camera position to within the bounding object
	/// </summary>
	/// <returns>The bounded.</returns>
	/// <param name="position">The position bounded by the Bounding Object.</param>
	Vector3 positionBounded(Vector3 position){

		// the current x, y & z positions
		float x = position.x;
		float y = position.y;
		float z = initialPosition.z;		// note that this will not be altered

		if (usesCameraBounds) {
			
			// clamped to camera boundaries
			x = Mathf.Clamp( position.x, _xBoundaryMin, _xBoundaryMax );
			y = Mathf.Clamp( position.y, _yBoundaryMin, _yBoundaryMax );
		}

		return new Vector3( x, y, z );
	}

	/// <summary>
	/// Snaps the position to the sent object
	/// </summary>
	/// <param name="gObj">G object.</param>
	public void snapTo(GameObject gObj) {

		// Set the position of the camera's transform to be the same as the player's
		Vector3 pos = gObj.transform.position;	

		transform.position = positionBounded( pos );
	}
}