using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.UI;

public class CreatorsSceneManager : MonoBehaviour
{
    public GameObject placeAddedUnitPrefab;
    public GameObject pathsAddedUnitPrefab;
    public Text consoleAddedNew;
    public Text consoleAddedLast;
    public string LastAdded = "";
    public float unitHeight = 130;
    public RectTransform pathsAdded;
    public RectTransform placesAdded;
    public int noOfPlacesAdded = 0;
    public int noOfPathsAdded = 0;

    //deletes elements in paths added and paths places and resets everything in case the mapping goes wrong
    public void Reset()
    {
        foreach (Transform t in pathsAdded.GetComponentsInChildren<Transform>())
            Destroy(t.gameObject);

        foreach (Transform t in placesAdded.GetComponentsInChildren<Transform>())
            Destroy(t.gameObject);

        placesAdded.sizeDelta = new Vector2(placesAdded.sizeDelta.x, 0);
        pathsAdded.sizeDelta = new Vector2(pathsAdded.sizeDelta.x, 0);

        noOfPathsAdded = 0;
        noOfPlacesAdded = 0;
    }

    public void AddPlace(string placeName)
    {
        noOfPlacesAdded++;
        GameObject temp = Instantiate(placeAddedUnitPrefab, placesAdded);
        temp.name = placeName;
        temp.GetComponentInChildren<Text>().text = placeName;
        placesAdded.sizeDelta = new Vector2(placesAdded.sizeDelta.x, noOfPlacesAdded * unitHeight);
    }

    public void AddPath(string pathName)
    {
        noOfPathsAdded++;
        GameObject temp = Instantiate(pathsAddedUnitPrefab, pathsAdded);
        temp.name = pathName;
        temp.GetComponentInChildren<Text>().text = pathName;
        pathsAdded.sizeDelta = new Vector2(pathsAdded.sizeDelta.x, noOfPathsAdded * unitHeight);
    }

    public void StartMapping()
    {
        //start the mapping
        Debug.Log("start mapping called");
    }

    public void EndMapping()
    {
        SaveData();
        DataManagerCreator.ins.UploadeFile();
        //end the mapping and upload the file
        Debug.Log("end mapping called");
    }

    public void WriteToConsole(string txt)
    {
        consoleAddedNew.text = txt;
        consoleAddedLast.text = LastAdded;
        LastAdded = txt;
    }

    public void SaveData()
    {
        mapData md = new mapData();
        GameObject[] gbPaths = GameObject.FindGameObjectsWithTag("path");
        GameObject[] gbPlaces = GameObject.FindGameObjectsWithTag("place");
        GameObject[] gbtags = GameObject.FindGameObjectsWithTag("product");

        md.noOfPaths = gbPaths.Length;
        md.noOfPlaces = gbPlaces.Length;
        md.noOftags = gbtags.Length;
        foreach (GameObject gb in gbPaths)
        {
            paths p = new paths();

            p.name = gb.name;
            p.position = gb.transform.position;
            p.noOfPositions = gb.GetComponent<LineRenderer>().positionCount;
            p.lineRendPositions = new Vector3[p.noOfPositions];

            gb.GetComponent<LineRenderer>().GetPositions(p.lineRendPositions);

            md.pathsList.Add(p);
        }

        foreach (GameObject gb in gbPlaces)
        {
            places p = new places();

            p.name = gb.name;
            p.position = gb.transform.position;

            md.placesList.Add(p);
        }
        foreach (GameObject gb in gbtags)
        {
            tags p = new tags();

            p.name = gb.name;
            p.position = gb.transform.position;

            md.tagsList.Add(p);
        }

        //xml saving script

        XmlSerializer serializer = new XmlSerializer(typeof(mapData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/mapdata.xml", FileMode.Create);
        serializer.Serialize(stream, md);
        Debug.Log("File created");
        stream.Close();

        DataManagerCreator.ins.UploadeFile();

    }
    public void closeapp()
    {
        Application.Quit();
    }
}
