using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EcoScanner.Services
{
    public static class History
    {
        public static Dictionary<DateTime, float> historyData = new Dictionary<DateTime, float>();
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Liste";
        static string filePath = path + "/History";
        public static Dictionary<DateTime, float> getHistory()
        {
            //read file
            setup();
            historyData = JsonSerializer.Deserialize<Dictionary<DateTime, float>>(File.ReadAllText(filePath));
            return historyData;
        }
        /// <summary>
        /// Constructor - makes sure file always exists - Mandatory to call
        /// </summary>
        public static void setup()
        {
            Directory.CreateDirectory(path);
            if (!File.Exists(filePath))
            {
                historyData = new Dictionary<DateTime, float>();
                string json = JsonSerializer.Serialize(historyData);
                File.WriteAllText(filePath, json);
            }
        }
        public static void addToHistory()
        {
            setup();
            float sum = Liste.getSum();
            if (sum == 0)
            {
                return;
            }

            //read file, then add to end of file.
            Dictionary<DateTime, float> oldData = JsonSerializer.Deserialize<Dictionary<DateTime, float>>(File.ReadAllText(filePath));
            if (oldData.ContainsKey(DateTime.Today))
            {
                oldData[DateTime.Today] += sum;
            }
            else
            {
                oldData.Add(DateTime.Today, sum);
            }
            historyData = oldData;
            string json = JsonSerializer.Serialize(oldData);
            File.WriteAllText(filePath, json);
            ListeViewModel.invokeClearList();
        }
        public static void clearHistory()
        {

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Trace.WriteLine("---- Removed file -----");
            }
            else
            {
                Trace.WriteLine("--- No file to remove ---");
            }

        }
        public static void cheatadd()
        {
            setup();
            float sum = 10;
            if (sum == 0)
            {
                return;
            }

            //read file, then add to end of file.
            Dictionary<DateTime, float> oldData = JsonSerializer.Deserialize<Dictionary<DateTime, float>>(File.ReadAllText(filePath));
            if (oldData.ContainsKey(new DateTime(2023, 4, 27)))
            {
                oldData[new DateTime(2023, 4, 27)] += sum;
            }
            else
            {
                oldData.Add(new DateTime(2023, 4, 27), sum);
            }
            historyData = oldData;
            string json = JsonSerializer.Serialize(oldData);
            File.WriteAllText(filePath, json);
            ListeViewModel.invokeClearList();
        }
    }
}
