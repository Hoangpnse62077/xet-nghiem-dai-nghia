using System.Text;
using System.Threading;
using eLTMS.Models.Models.dto;
using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace LabTechnicianWindowService
{
    public partial class Service1 : ServiceBase
    {
        string watchPath = ConfigurationManager.AppSettings["WatchPath"];
        string backupPath = ConfigurationManager.AppSettings["BackupPath"];
        static string logPath = ConfigurationManager.AppSettings["LogPath"];
        string APIDomain = ConfigurationManager.AppSettings["APIDomain"];

        public Service1()
        {
            InitializeComponent();
            fileSystemWatcher1.Created += fileSystemWatcher1_Created;
        }

        void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(4000);
                if (CheckFileExistance(watchPath, e.Name))
                {
                    // Create an HttpClient instance
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(APIDomain);

                    // Usage
                    var ltiDtos = ReadExcelAsDTO(watchPath, e.Name);

                    // Debug
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("DTO:");
                    foreach (var dto in ltiDtos)
                    {
                        dto.IsDeleted = false;
                        sb.AppendFormat("Name:{0} - Value:{1} - Status:{2} - NormalRange:{3} - Unit:{4}\n",
                            dto.IndexName, dto.IndexValue, dto.LowNormalHigh, dto.NormalRange, dto.Unit);
                    }
                    CreateLogFile(sb.ToString());

                    HttpResponseMessage response = client.PostAsJsonAsync<List<LabTestingIndexDto>>
                        ("api/labtesting/add-lab-testing-indexes", ltiDtos).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var respObj = response.Content.ReadAsAsync<ResponseObjectDto>().Result;
                        var sb2 = new StringBuilder();
                        sb2.AppendFormat("Succ:{0} - Mess:{1} \n", respObj.Success, respObj.Message);
                        CreateLogFile(sb2.ToString());
                    }
                    else
                    {
                        var sb2 = new StringBuilder();
                        sb2.AppendFormat("Code:{0} - Reason:{1} \n", (int)response.StatusCode, response.ReasonPhrase);
                        CreateLogFile(sb2.ToString());
                    }

                    // Move File To Backup
                    MoveFileToBackup(e.Name);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Message: " + ex.Message);
                sb.AppendLine("Stack Trace: " + ex.StackTrace);
                CreateLogFile(sb.ToString());
            }
        }

        private bool MoveFileToBackup(string FileName)
        {
            string path = Path.Combine(watchPath, FileName);
            string path2 = Path.Combine(backupPath, FileName);
            try
            {
                if (!File.Exists(path))
                {
                    // This statement ensures that the file is created,
                    // but the handle is not kept.
                    using (FileStream fs = File.Create(path)) { }
                }

                // Ensure that the target does not exist.
                if (File.Exists(path2))
                    File.Delete(path2);

                // Move the file.
                File.Move(path, path2);
                CreateLogFile(string.Format("{0} was moved to {1}.", path, path2));

                // See if the original exists now.
                if (File.Exists(path))
                {
                    CreateLogFile("The original file still exists, which is unexpected.");
                }
                else
                {
                    CreateLogFile("The original file no longer exists, which is expected.");
                }

            }
            catch (Exception e)
            {
                CreateLogFile(string.Format("The process failed: {0}", e.ToString()));
                return false;
            }
            return true;
        }

        private List<LabTestingIndexDto> ReadExcelAsDTO(string FullPath, string FileName)
        {
            var result = new List<LabTestingIndexDto>();

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            ExcelFile ef = ExcelFile.Load(Path.Combine(FullPath, FileName));

            // Iterate through all worksheets in an Excel workbook.
            int? labTestingId = null;            
            var sId = FileName.Split('.').First().Split('_').LastOrDefault();
            if (sId != null)
            {
                try
                {
                    labTestingId = int.Parse(sId);
                }
                catch (Exception ex)
                {
                    labTestingId = null;
                }
            }

            //foreach (ExcelWorksheet sheet in ef.Worksheets)
            var sheet = ef.Worksheets.FirstOrDefault();
            if (sheet != null)
            {
                //try
                //{
                //    labTestingId = int.Parse(sheet.Name.Trim());
                //}
                //catch (Exception ex)
                //{
                //    labTestingId = null;
                //}

                // Iterate through all rows in an Excel worksheet.
                foreach (ExcelRow row in sheet.Rows)
                {
                    if (row.Index == 0)
                    {
                        continue;
                    }

                    var resultItem = new LabTestingIndexDto();
                    resultItem.LabTestingId = labTestingId;
                    // Iterate through all allocated cells in an Excel row.
                    // Index Name
                    var cell = row.AllocatedCells.ElementAtOrDefault(0);
                    if (cell.ValueType != CellValueType.Null)
                    {
                        resultItem.IndexName = (string)cell.Value.ToString();
                    }
                    // Index Value
                    cell = row.AllocatedCells.ElementAtOrDefault(1);
                    if (cell.ValueType != CellValueType.Null)
                    {
                        resultItem.IndexValue = (string)cell.Value.ToString();
                    }
                    // Status
                    cell = row.AllocatedCells.ElementAtOrDefault(2);
                    if (cell.ValueType != CellValueType.Null)
                    {
                        resultItem.LowNormalHigh = (string)cell.Value.ToString();
                    }
                    // Normal Rnage
                    cell = row.AllocatedCells.ElementAtOrDefault(3);
                    if (cell.ValueType != CellValueType.Null)
                    {
                        resultItem.NormalRange = (string)cell.Value.ToString();
                    }
                    // Unit
                    cell = row.AllocatedCells.ElementAtOrDefault(4);
                    if (cell.ValueType != CellValueType.Null)
                    {
                        resultItem.Unit = (string)cell.Value.ToString();
                    }
                    result.Add(resultItem);
                }
            }

            return result;
        }

        private bool CheckFileExistance(string FullPath, string FileName)
        {
            // Get the subdirectories for the specified directory.'
            bool IsFileExist = false;
            DirectoryInfo dir = new DirectoryInfo(FullPath); // check directory exist
            if (!dir.Exists)
                IsFileExist = false;
            else
            {
                string FileFullPath = Path.Combine(FullPath, FileName); // check fullpath exist
                if (File.Exists(FileFullPath))
                    IsFileExist = true;
            }
            return IsFileExist;
        }/**/

        /*
        private void CreateTextFile(string FullPath, string FileName)
        {
            StreamWriter SW;
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW = File.CreateText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                    "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"));
                SW.Close();
            }
            using (SW = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW.WriteLine("File Created with Name: " + FileName + " at this location: " + FullPath);
                SW.Close();
            }
        }/**/

        public static void CreateLogFile(string content)
        {
            string Destination = logPath;
            StreamWriter SW;
            if (Directory.Exists(Destination))
            {
                Destination = System.IO.Path.Combine(Destination, "Log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                if (!File.Exists(Destination))
                {
                    SW = File.CreateText(Destination);
                    SW.Close();
                }
            }
            using (SW = File.AppendText(Destination))
            {
                SW.Write("\r\n\n");
                SW.WriteLine("Error occurs at: " + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss"));
                SW.WriteLine(content);
                SW.Close();
            }
        }/**/

        protected override void OnStart(string[] args)
        {
            try
            {
                fileSystemWatcher1.Path = watchPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnStop()
        {

        }
    }
}
