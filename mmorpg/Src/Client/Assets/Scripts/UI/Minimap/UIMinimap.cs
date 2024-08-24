using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour {

	public Collider minimapBoudingBox;
	public Image minimap;
	public Image arrow;
	public Text mapName;
	private Transform playerTransform;

	void Start () 
	{
		MinimapManager.Instance.minimap = this;
		this.UpdateMap();
	}

	public void UpdateMap()
	{
        this.mapName.text = User.Instance.CurrentMapData.Name;
		this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

		this.minimap.SetNativeSize();
		this.minimap.transform.localPosition = Vector3.zero;
		this.minimapBoudingBox = MinimapManager.Instance.MinimapBoundingBox;
		this.playerTransform = null;
    }

	void Update()
	{
		if (playerTransform == null)		
			playerTransform = MinimapManager.Instance.PlayerTransform;		

		if(minimapBoudingBox == null || playerTransform == null)
			return;

		float realWidth = minimapBoudingBox.bounds.size.x;
		float realHeight = minimapBoudingBox.bounds.size.z;

		float relaX = playerTransform.position.x - minimapBoudingBox.bounds.min.x;
		float relaY = playerTransform.position.z - minimapBoudingBox.bounds.min.z;

		float pivotX = relaX / realWidth;
		float pivotY = relaY / realHeight;

		this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
		this.minimap.rectTransform.localPosition = Vector2.zero;
		this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
	}
}
