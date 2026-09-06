using UnityEngine;

namespace AR {
	public class TopDownCamera : MonoBehaviour
	{

		[Tooltip("A target for the camera to follow.")]
		[SerializeField]
		public Transform target;

		[Tooltip("Target top collider. Used to calculate distance between collider top point and camera.")]
		[SerializeField]
		private Collider topCollider;

		private Vector3 currentVelocity;

		[Header("Settings")]

		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float smoothTime = 0.1f;

		[SerializeField]
		[Range(0.0f, 20.0f)]
		private float height = 8f;

		[SerializeField]
		[Range(45f, 90f)]
		private float angle = 90f;

		[SerializeField]
		[Range(0.1f, 10f)]
		private float angleFactor = 90f;

		[Header("Look Ahead")]

		[SerializeField]
		[Range(0.0f, 20.0f)]
		private float horizontalLookAhead = 5f;

		[SerializeField]
		[Range(0.0f, 20.0f)]
		private float verticalLookAhead = 5f;

		private Camera cam;
		private float verticleRotation = 0f;

		private void Awake()
		{
			cam = GetComponent<Camera>();
		}

  //      private void Start()
  //      {
		//	target = SpawnerLogic.instance.player.transform;
		//}

        private void LateUpdate()
		{
			if (target != null && topCollider != null)
			{
				Vector3 dir = target.position - transform.position;
				Debug.DrawRay(transform.position, dir, Color.red);

				// Use law of sines to calculate Z distance based in camera angle.
				var zRelativeToAngle = height / Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Sin((90 - angle) * Mathf.Deg2Rad);

				var dx = (Input.mousePosition.x - Screen.width * 0.5f) / Screen.width; ;
				var dz = (Input.mousePosition.y - Screen.height * 0.5f) / Screen.height;

				// Set Target Position
				Vector3 targetPosition = new Vector3(
					target.position.x + dx * horizontalLookAhead,
					height,
					target.position.z - zRelativeToAngle + dz * verticalLookAhead
				);

				// Set Current Position
				transform.position = Vector3.SmoothDamp(
					transform.position,
					targetPosition,
					ref currentVelocity,
					smoothTime
				);

				verticleRotation = angle - dz * angleFactor;
				verticleRotation = Mathf.Clamp(verticleRotation, 60f,90f);
				// Set Rotation
				transform.eulerAngles = new Vector3(
					verticleRotation,
					0,
					0
				);
			}
			else
			{
				Debug.LogWarning("Top Collider not found!");
			}
		}
	}
}