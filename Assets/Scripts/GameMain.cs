﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour
{

	public float timer = 2.0f;

	bool isChatting = true;
	public Vector2 scrollPosition;

	public float scrollVelocity = 0f;
	public float timeTouchPhaseEnded = 0f;
	public float inertiaDuration = 0.5f;
	public Vector2 lastDeltaPos;

	public Queue<Message> messageQueue = new Queue<Message> ();
	public Queue<Message> unShownMessageQueue = new Queue<Message> ();
	public Queue<Message> shownMessageQueue = new Queue<Message> ();

	public AudioSource audioSource;
	public AudioClip bgm;
	public AudioClip buttonSound;
	public AudioClip msgSound;
	public float bgmVolume = 1.0f;
	public float soundVolume = 1.0f;


	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = bgm;

		messageQueue.Enqueue (new Message (0, "通话接入..."));
		messageQueue.Enqueue (new Message (0, "建立连接..."));
		messageQueue.Enqueue (new Message (0, "接收讯息..."));
		messageQueue.Enqueue (new Message (2, "有人吗？"));
		messageQueue.Enqueue (new Message (2, "这玩意能用吗？"));
		messageQueue.Enqueue (new Message (2, "有人能收到我的消息吗？"));
		messageQueue.Enqueue (new Message (1, "收到，你是哪位？"));
		messageQueue.Enqueue (new Message (2, "呜呜,让我先哭一会！！！太感谢能收到你的消息了！"));
		messageQueue.Enqueue (new Message (2, "万幸林博士在我耳中植入了RUR芯片，向外界发送脑电波，我已经发送了好久，只有你收到了。"));
		messageQueue.Enqueue (new Message (2, "我曾经。。是中关村秘密研究院的实习生，名字是严明超，你好。"));
		messageQueue.Enqueue (new Message (2, "请你务必要帮我，如果这件事出了点差错，你我都会消失。"));
		messageQueue.Enqueue (new Message (1, "什么事？这么严重？"));
		messageQueue.Enqueue (new Message (2, "这件事有些不可思议"));
		messageQueue.Enqueue (new Message (2, "你信不信这世界上有飞碟？信不信有尼斯湖水怪！"));
		messageQueue.Enqueue (new Message (2, "你信不信爱因斯坦的相对论呢影响了后世很多成功的科学理论！但F=ma是不对的！"));
		messageQueue.Enqueue (new Message (1, "我信，F=ma这个我物理不太好诶！"));
		messageQueue.Enqueue (new Message (2, "可以交流！"));
		messageQueue.Enqueue (new Message (2, "呜呜,让我先哭一会！！！太感谢能收到你的消息了！"));
		messageQueue.Enqueue (new Message (2, "呜呜,让我先哭一会！！！太感谢能收到你的消息了！"));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeScale > 0 && Input.GetKeyDown (KeyCode.Escape)) {
			Time.timeScale = 0;
		}
		UpdateHandleTouch ();
		PlayerBGM ();
	}

	void PlayerBGM ()
	{
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
		audioSource.volume = bgmVolume;
	}

	void UpdateHandleTouch ()
	{
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				scrollPosition.y += Input.GetTouch (0).deltaPosition.y;
				lastDeltaPos = Input.GetTouch (0).deltaPosition;
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				print ("End:" + lastDeltaPos.y + "|" + Input.GetTouch (0).deltaTime);
				if (Mathf.Abs (lastDeltaPos.y) > 20.0f) {
					scrollVelocity = (int)(lastDeltaPos.y * 0.5 / Input.GetTouch (0).deltaTime);
					print (scrollVelocity);
				}
				timeTouchPhaseEnded = Time.time;
			}
		} else {
			if (scrollVelocity != 0.0f) {
				// slow down
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				float frameVelocity = Mathf.Lerp (scrollVelocity, 0, t);
				scrollPosition.y += frameVelocity * Time.deltaTime;
				
				if (t >= inertiaDuration)
					scrollVelocity = 0;
			}
		}
	}

	void OnGUI ()
	{
		float ratioW = System.Convert.ToSingle (Screen.width) / 540f;
		float ratioH = System.Convert.ToSingle (Screen.height) / 960f;
		GUI.matrix = Matrix4x4.TRS (new Vector3 (0, 0, 0), 
			Quaternion.identity, new Vector3 (ratioW, ratioH, 1));
		if (!isChatting || Time.timeScale == 0) {
			MainMenu ();
		} else {
			Chat ();
		}
		GUI.matrix = Matrix4x4.identity;
	}

	void MainMenu ()
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (100);

		GUILayout.BeginHorizontal ();
		GUILayout.Space (150);
		GUI.skin.label.fontSize = 50;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUILayout.Label ("文字游戏", GUILayout.Width (240), GUILayout.Height (150));
		GUILayout.Space (150);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Space (150);
		GUI.skin.label.fontSize = 30;
		//GUI.skin.label.alignment = TextAnchor.LowerCenter;
		GUILayout.Label ("音乐", GUILayout.Width (100), GUILayout.Height (100));
		GUILayout.BeginVertical ();
		GUILayout.Space (20);
		bgmVolume = GUILayout.HorizontalSlider (bgmVolume, 0f, 1.0f, GUILayout.Width (140), GUILayout.Height (100));
		GUILayout.EndVertical ();
		GUILayout.Space (150);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Space (150);
		GUI.skin.label.fontSize = 30;
		GUILayout.Label ("音效", GUILayout.Width (100), GUILayout.Height (100));
		GUILayout.BeginVertical ();
		GUILayout.Space (20);
		soundVolume = GUILayout.HorizontalSlider (soundVolume, 0f, 1.0f, GUILayout.Width (140), GUILayout.Height (100));
		GUILayout.EndVertical ();
		GUILayout.Space (150);
		GUILayout.EndHorizontal ();

		GUILayout.Space (100);

		GUILayout.BeginHorizontal ();
		GUILayout.Space (180);
		GUI.skin.button.fontSize = 40;
		if (GUILayout.Button ("返回", GUILayout.Width (180), GUILayout.Height (50))) {
			audioSource.PlayOneShot (buttonSound, soundVolume * 1 / bgmVolume);
			Time.timeScale = 1;
			isChatting = true;
		}
		GUILayout.Space (180);
		GUILayout.EndHorizontal ();

		GUILayout.Space (100);

		GUILayout.BeginHorizontal ();
		GUILayout.Space (180);
		GUI.skin.button.fontSize = 40;
		if (GUILayout.Button ("离开", GUILayout.Width (180), GUILayout.Height (50))) {
			audioSource.PlayOneShot (buttonSound, soundVolume * 1 / bgmVolume);
			Application.Quit ();
		}
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();
	}

	void Chat ()
	{
		GUILayout.BeginArea (new Rect (0, 0, 540, 960));
		print (scrollPosition);
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (540), GUILayout.Height (960));
		print (scrollPosition);
		foreach (Message msg in shownMessageQueue) {
			GUILayout.BeginVertical ();
			GUILayout.ExpandWidth (false);
			if (msg.sender == 0) {
				GUI.color = Color.white;
				GUI.skin.label.fontSize = 25;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label (msg.content);
			} else if (msg.sender == 1) {
				GUILayout.BeginHorizontal ();
				GUI.color = Color.green;
				GUI.skin.label.fontSize = 25;
				GUILayout.Space (100);	
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUILayout.Label (msg.content);
				GUILayout.EndHorizontal ();
			} else {
				GUI.color = Color.gray;
				GUI.skin.label.fontSize = 15;
				GUI.skin.label.alignment = TextAnchor.LowerLeft;
				GUILayout.Label ("[严明超]:");

				GUILayout.BeginHorizontal ();
				GUI.color = Color.green;
				GUI.skin.label.fontSize = 25;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUILayout.Label (msg.content);
				GUILayout.Space (100);	
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}
		GUILayout.EndScrollView ();

		print (messageQueue.Count);
		timer -= Time.deltaTime;
		if (timer <= 0 && 0 != messageQueue.Count) {
			shownMessageQueue.Enqueue ((Message)messageQueue.Dequeue ());
			scrollPosition.y += 999;
//			Debug.Log (string.Format ("Timer1 is up !!! time=${0}", Time.time));
			timer = 2.0f;
			audioSource.PlayOneShot (msgSound, soundVolume * 1 / bgmVolume);
		}

//		print (messageQueue.Count);
//		if (GUI.Button (new Rect (0, 800, 150, 50), "next") && 0 != messageQueue.Count) {
//			shownMessageQueue.Enqueue ((Message)messageQueue.Dequeue ());
//			scrollPosition.y += 999;
//		}
		GUILayout.EndArea ();
	}
}
