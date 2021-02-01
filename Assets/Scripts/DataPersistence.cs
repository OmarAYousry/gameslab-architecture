using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public static class DataPersistence
{
    public static void SerializeObjectStates()
    {
        List<StatesDictionary> allStates = new List<StatesDictionary>();
        foreach (InteractableObject interObj in InteractableObject.interactables)
        {
            allStates.Add(new StatesDictionary(interObj.name, interObj.states));
        }


        XmlSerializer serializer = new XmlSerializer(typeof(List<StatesDictionary>));

        string filePath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{BuildingBehaviour.BuildingName}.xml";
        FileStream serializationStream = File.Open(filePath, FileMode.Create);
        try
        {
            serializer.Serialize(serializationStream, allStates);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            serializationStream.Close();
        }
    }

    public static List<StatesDictionary> DeSerializeObjectStates()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<StatesDictionary>));

        string filePath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{BuildingBehaviour.BuildingName}.xml";
        FileStream fs = new FileStream(filePath, FileMode.Open);

        List<StatesDictionary> allObjectStates = (List<StatesDictionary>) serializer.Deserialize(fs);

        fs.Close();

        return allObjectStates;

    }

    public static bool InitObjectStatesFromFile()
    {
        try
        {
            List<StatesDictionary> allStatesFromFile = DeSerializeObjectStates();
            foreach (InteractableObject interObj in InteractableObject.interactables)
            {
                StatesDictionary currentEntry = allStatesFromFile.Find(entry => entry.objectName == interObj.name);
                if (currentEntry != null)
                {
                    interObj.states = currentEntry.objectStates;
                    interObj.SetState(0);
                }
                else
                {
                    // value was not found; init using default values
                    // currently no-op, since values are internally initialized
                }
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Some entries were not found. Error: {e}");
            return false;
        }


    }

    [System.Serializable]
    public class StatesDictionary
    {
        public string objectName;
        public List<ObjectState> objectStates;

        public StatesDictionary()
        {
            objectName = string.Empty;
            objectStates = new List<ObjectState>();
        }

        public StatesDictionary(string objectName, List<ObjectState> objectStates)
        {
            this.objectName = objectName;
            this.objectStates = objectStates;
        }
    }

}