using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAssetBundleData : MonoBehaviour {
	// AssetBundleのキャッシュ
	private AssetBundle assetBundleCache;
	public GameObject speare;
	//永続的なフォルダ
	string AssetBundlePersistentPath;

	public IEnumerator Start() {
		AssetBundlePersistentPath = Application.persistentDataPath + "/" + "myfirstassetbundle";
		//キャッシュクリア
		Caching.CleanCache();

		// ファイルが無かったらやめる
		if (!System.IO.File.Exists(AssetBundlePersistentPath))
		{
			Debug.Log("File does NOT exist!!, path = " + AssetBundlePersistentPath);
			speare.GetComponent<Renderer> ().material.color = Color.red;
			yield break;
		}

		// Asset Bundleのロード処理
		yield return StartCoroutine(LoadAssetBundleCoroutine());
		// ロード完了後、Assetが取り出せるようになる
		speare.GetComponent<Renderer>().material.mainTexture = GetSpriteFromAssetBundle("Alexs_Apt_8k").texture;
	}
		
	IEnumerator LoadAssetBundleCoroutine()
	{
		// Asset BundleのURL
		var url = "";
		print (Application.persistentDataPath + "/" + "myfirstassetbundle");
		#if UNITY_EDITOR
		//streamingAssetsPath
		url =  "file://" + Application.streamingAssetsPath + "/" + "myfirstassetbundle";
		//persistentDataPath
		//url = "file://" + Application.persistentDataPath + "/" + "myfirstassetbundle";
		#elif UNITY_ANDROID
		//streamingAssetsPath
		//url = "jar:file://" + Application.dataPath + "!/assets" + "/"+ "myfirstassetbundle";
		//persistentDataPath
		url = "file://" + Application.persistentDataPath + "/" + "myfirstassetbundle";
		#endif

		// ダウンロード処理
		var www = WWW.LoadFromCacheOrDownload(url, 0);
		while (!www.isDone)
		{
			yield return null;
		}

		// TODO エラー処理とか

		// Asset Bundleをキャッシュ
		assetBundleCache = www.assetBundle;

		// リクエストは開放
		www.Dispose();
	}

	// Asset BundleからSpriteを取得します
	public Sprite GetSpriteFromAssetBundle(string assetName)
	{
		try
		{
			return assetBundleCache.LoadAsset<Sprite>(string.Format("{0}.jpg", assetName));
		}
		catch (NullReferenceException e)
		{
		Debug.Log(e.ToString());
		return null;
		}
	}
	/*
	public void logSave(){
		StreamWriter sw;
		FileInfo fi;
		fi = new FileInfo(Application.persistentDataPath + "/FileName.csv");
		sw = fi.AppendText();
		sw.WriteLine("test output");
		sw.Flush();
		sw.Close();
	}
	*/
}
