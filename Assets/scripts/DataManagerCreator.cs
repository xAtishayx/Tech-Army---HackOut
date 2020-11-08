using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Firebase.Storage;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;


public class DataManagerCreator : MonoBehaviour
{
    public static DataManagerCreator ins;

    [SerializeField]
    private string ID;

    public void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        if(ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoginToCreator()
    {
        ID = GameObject.FindGameObjectWithTag("ID").GetComponent<TextMeshProUGUI>().text;
    }

    public string CreateHash(string id)
    {
        MD5 md5Hash = MD5.Create();
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(id));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        Debug.Log(id + " " + sBuilder.ToString());

        return sBuilder.ToString();
    }

    public void UploadeFile()
    {
        Debug.Log("uploading file");

        // Get a reference to the storage service, using the default Firebase App
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://pathiktestsih.appspot.com");

        // File located on disk
        string local_file = "file://" + Application.persistentDataPath + "/mapdata.xml";

        Debug.Log(local_file);

        // Create a reference to the file you want to upload
        // StorageReference file_ref = storage_ref.Child("Map Data/" + CreateHash(Random.Range(100f, 1000f).ToString()) + "/mapdata.xml");

        // Create file metadata including the content type
        var new_metadata = new MetadataChange();
        new_metadata.ContentType = "text/xml";


        var task = storage_ref.Child("Map Data/" + CreateHash(ID) + "/mapdata.xml")
    .PutFileAsync(local_file, null,
      new StorageProgress<UploadState>(state => {
          // called periodically during the upload
          Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
                             state.BytesTransferred, state.TotalByteCount));
      }), CancellationToken.None, null);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Upload finished.");
                ID = "null";
            }
            else
            {
                Debug.Log(task.Exception.ToString());
            }
        });
    }

}
 public void Internalshift()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
               }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        if(ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

public class mapData
{
    public int noOfPaths;
    public int noOfPlaces;
    public int noOftags;
    public List<paths> pathsList = new List<paths>();
    public List<places> placesList = new List<places>();
    public List<tags> tagsList = new List<tags>();
}

public class paths
{
    public string name;
    public Vector3 position;

    //Line renderer positions

    public int noOfPositions;
    public Vector3[] lineRendPositions;
}

public class places
{
    public string name;
    public Vector3 position;
}

public class tags
{
    public string name;
    public Vector3 position;
}
