using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //formats code into binary

public static class saveDataScript
{

    public static void saveGame(playerController player){
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/player.saved";
        FileStream stream = new FileStream(savePath, FileMode.Create); //creates the file (use FileMode.Open to open a file)

        savedData data = new savedData(player); //gets the data

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static savedData loadGame(){
        string savePath = Application.persistentDataPath + "/player.saved";
        if(File.Exists(savePath)){ //checks if theres a file at the saved path
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            savedData data = formatter.Deserialize(stream) as savedData; //unloads all the saved info into data
            stream.Close();

            return data;
        }else{
            return null;
        }
    }
}
