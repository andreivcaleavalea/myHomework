using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public static class FilesManager
{
    private static readonly string HomeworksPath = Application.persistentDataPath + "/homeworks.json";
    public static List<HomeworkModel> GetHomeworksFromJson()
    {
        if (File.Exists(HomeworksPath))
        {
            var homeworks = File.ReadAllText(HomeworksPath);
            var homeworksList = JsonConvert.DeserializeObject<List<HomeworkModel>>(homeworks);
            for (var i = 0; i < homeworksList.Count; i++)
            {
                homeworksList[i].homeworkIndex = i;
            }
            return homeworksList;
        }

        var lists = new List<HomeworkModel>();
        return lists;
    }
    public static void RefreshHomeworksList(List<HomeworkModel> homeworksList)
    {
        var x = JsonConvert.SerializeObject(homeworksList);
        File.Delete(HomeworksPath);
        File.WriteAllText(HomeworksPath, x);
    }
}
