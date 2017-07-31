using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragObj : MonoBehaviour {
	
	public bool mouseon;
	public bool fade=false;
	public AudioClip selection;
	public bool nomatch = true;
	public float scaleValue;//0.7f
	public float finalScale;// 0.425f
	private Rigidbody2D r;
	private AudioSource camAudio;
	private float distanceFromCamera;
	private bool firstTime = true;

	private SpriteRenderer sr;
	private BoxCollider2D childCollider;
	private Vector3 stageDimensions;
	void Awake(){
		
		//transform.position = new Vector3 (stageDimensions.x, transform.position.y,transform.position.z);
	}

	// Use this for initialization
	void Start () {
		r = transform.GetComponent<Rigidbody2D>();
		camAudio = Camera.main.GetComponent<AudioSource> ();
		sr = transform.GetComponent<SpriteRenderer> ();
		childCollider = transform.GetChild (0).GetComponent<BoxCollider2D> ();
		stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));distanceFromCamera = Vector3.Distance (transform.position,  Camera.main.transform.position);
	}
		
	// Update is called once per frame

	void Update () {
		if (transform.position.x > stageDimensions.x-0.5f) {
			r.velocity =new Vector3(-3.0f,0.0f,0.0f); 
		}
		if (transform.position.x < -stageDimensions.x+0.5f) {
			r.velocity =new Vector3(3.0f,0.0f,0.0f); 
		}
		if (transform.position.y > stageDimensions.y) {
			r.velocity =new Vector3(0.0f,-3.0f,0.0f); 
		}
		if (transform.position.y < -stageDimensions.y) {
			transform.position = new Vector3 (transform.position.x, -stageDimensions.y+1.0f,0.0f);
		}
		
		if (mouseon) {
			Vector3 pos = Input.mousePosition;
			pos.z = distanceFromCamera;
			pos = Camera.main.ScreenToWorldPoint (pos);
			r.velocity = (pos - transform.position) * 10;


		}
		if(!nomatch){
			sr.sortingOrder = 2;
		}
		if (fade) {
			transform.localScale = new Vector3 (scaleValue, scaleValue, scaleValue);	
			if (scaleValue >=finalScale) {
				scaleValue = scaleValue - 0.025f;
			} else {
				childCollider.enabled = true;
				fade = false;
			}
		}
	}

	void OnMouseDown(){
		
		mouseon = true;
		if (nomatch) {
			transform.localScale = new Vector3 (finalScale + 0.025f, finalScale + 0.025f, finalScale + 0.025f);
			camAudio.clip = selection ;
			camAudio.Play();
		}
	
	}
	void OnMouseUp(){
		mouseon = false;
		if (nomatch) {
			transform.localScale = new Vector3 (finalScale - 0.025f, finalScale - 0.025f, finalScale - 0.025f);
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.name == "Floor") {
			if (firstTime) {
				fade = true;
				firstTime = false;
			}
		}
	}
}
