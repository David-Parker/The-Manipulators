using UnityEngine;
using System.Collections;

public class StateDisplay : MonoBehaviour {

	bool billBoardActive = false;
	public int textSize = 32;
	private GameObject textBillBoard;

    void Update()
    {
        if (billBoardActive)
        {
            textBillBoard.transform.rotation = Quaternion.EulerRotation(45, 0, 0);
        }
    }

	public void UpdateState (string displayText) {
	
		if (displayText.Length > 0) {
			if(!billBoardActive)
			{
				billBoardActive = true;
				textBillBoard = new GameObject("StateBillBoard");
			    //textBillBoard.transform.parent = transform;

				TextMesh textMesh = textBillBoard.AddComponent(typeof(TextMesh)) as TextMesh;
				textBillBoard.AddComponent(typeof(MeshRenderer));

				MeshRenderer meshRenderer = textBillBoard.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
				meshRenderer.material = Resources.GetBuiltinResource(typeof(Material), "Arial.ttf") as Material; //Resources.Load("Arial", typeof(Material)) as Material;  // For custom font

				Font font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;//Resources.Load("ARIAL", typeof(Font)) as Font; // For custom font
				textMesh.font = font;
				textMesh.characterSize = .2f;
				textMesh.fontSize = textSize;
				textMesh.alignment = TextAlignment.Center;
				textMesh.anchor = TextAnchor.LowerCenter;

				//float objHeight = renderer.bounds.extents.y;
                float objHeight = collider.bounds.extents.y * 2;

				textBillBoard.transform.position = new Vector3(this.transform.position.x , this.transform.position.y + objHeight, transform.position.z);
				textMesh.transform.position = textBillBoard.transform.position;

				textBillBoard.GetComponent<TextMesh>().text = "Unit: " + transform.name + "\nState: " + displayText;
			}
			else{
				textBillBoard.GetComponent<TextMesh>().text = transform.name + ":\n" + displayText;
			}

		    textBillBoard.transform.parent = transform;
		}
	}

	public void ClearState (){
		billBoardActive = false;
		Destroy (textBillBoard);
	}
}
