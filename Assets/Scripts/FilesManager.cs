using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public static class FilesManager
{
    private static string homeworksPath = Application.persistentDataPath + "/homeworks.json";
    public static List<HomeworkModel> GetHomeworksFromJson()
    {
        if (File.Exists(homeworksPath))
        {
            var homeworks = File.ReadAllText(homeworksPath);
            var homeworksList = JsonConvert.DeserializeObject<List<HomeworkModel>>(homeworks);
            for (int i = 0; i < homeworksList.Count; i++)
            {
                homeworksList[i].homeworkIndex = i;
            }
            return (List<HomeworkModel>)homeworksList;
        }

        List<HomeworkModel> lists = new List<HomeworkModel>();
        return lists;
    }
    public static void RefreshHomeworksList(List<HomeworkModel> homeworksList)
    {
        var x = JsonConvert.SerializeObject(homeworksList);
        File.Delete(homeworksPath);
        File.WriteAllText(homeworksPath, x);
    }
}
