using UnityEngine;

namespace AR
{
	public class TP_Animator : MonoBehaviour
	{
		public TP_Motor charaMotor;
		public Animator animator;

		private string forwardBackwardParamName = "Vertical",
					   strafeParamName = "Horizontal", speedParamName = "Speed";


		void OnEnable()
		{
			if (this.animator == null)
				this.animator = this.GetComponent<Animator>();

			if (this.charaMotor == null)
				this.charaMotor = this.GetComponent<TP_Motor>();
		}

		void Update()
		{
			if ((this.animator == null) || (this.charaMotor == null) || (!this.charaMotor.canControl))
			{
				this.animator.SetFloat(this.speedParamName, 0f);
				return;
			}
			Vector2 v = new Vector2(this.charaMotor.GetLocalDir().x, this.charaMotor.GetLocalDir().z);


			if (!string.IsNullOrEmpty(this.speedParamName))
				this.animator.SetFloat(this.speedParamName, v.magnitude);

			if (!string.IsNullOrEmpty(this.forwardBackwardParamName))
				this.animator.SetFloat(this.forwardBackwardParamName, v.y);
			if (!string.IsNullOrEmpty(this.strafeParamName))
				this.animator.SetFloat(this.strafeParamName, v.x);
		}


	}
}

