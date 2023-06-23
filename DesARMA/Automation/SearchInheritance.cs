using DesARMA.Log;
using DesARMA.Log.Data;
using DesARMA.Models;
using DesARMA.Registers.EDR;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DesARMA.Automation
{
    public class SearchInheritance
    {
        HttpClient client = new HttpClient();
        private string? Reqstr;
        private string? code;
        private string? name;
        private string? path;
        int numberR;
        ProgresWindow progresWindow;
        Figurant figurant;
        public SearchInheritance(string? code, string? name, string path, ProgresWindow progresWindow, Figurant figurant, int numberR)
        {
            try
            {
                this.code = code;
                this.name = name;
                this.path = path;
                this.numberR = numberR;
                this.progresWindow = progresWindow;
                this.figurant = figurant;

                if (!Directory.Exists(path))
                    throw new Exception("Програма не може знайти шлях: " + path);

                SetParamsInReqstr();
                GetInfo();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }
        private void SetParamsInReqstr()
        {
            this.Reqstr = GetStr();

            if (this.Reqstr == null)
            {
                throw new Exception("string in config error");
            }

            if(code != null)
            {
                this.Reqstr = this.Reqstr.SetTypeSpadk("idn").SetDataSpadk(code);
            }
            else
            {
                this.Reqstr = this.Reqstr.SetTypeSpadk("pib").SetDataSpadk(name);
            }
        }
        private string? GetStr()
        {
            var appVal = ConfigurationManager.AppSettings["rsSpad"];
            if (appVal != null)
            {
                string shif = appVal.ToString();
                List<byte> arrByteReturn = new List<byte>();
                List<byte> arrByteReturnDecrypt = new List<byte>();
                for (int i = 3; i <= shif.Length; i += 3)
                {
                    var subStr = shif.Substring(i - 3, 3);
                    arrByteReturn.Add(Convert.ToByte(subStr));
                }

                List<byte> key = new List<byte>();
                for (int i = arrByteReturn.Count - 8; i < arrByteReturn.Count; i++)
                {
                    key.Add(arrByteReturn[i]);
                }

                for (int i = 0; i < arrByteReturn.Count - key.Count; i++)
                {
                    arrByteReturnDecrypt.Add(Convert.ToByte(arrByteReturn[i] ^ key[i % key.Count]));
                }

                string result2 = System.Text.Encoding.UTF8.GetString(arrByteReturnDecrypt.ToArray());
                return result2;
            }
            return null;
        }
        public async Task GetInfo()
        {
            byte[]? response;

            try
            {
                var searchInheritedTask = Task.Run(() =>
                {
                    try
                    {
                        response = GetDocFile();
                        if (response != null)
                        {
                            string nameFolder = $"\\{GetWhithout(name)}_{code}";
                            string nameFile;
                            if (code == null || code == "")
                                nameFile = $"\\Витяг_Спадковий реєстр_{GetWhithout(name)}.docx";
                            else
                                nameFile = $"\\Витяг_Спадковий реєстр_{code}.docx";


                            if (!Directory.Exists(path + nameFolder))
                                Directory.CreateDirectory(path + nameFolder);

                            SaveByteToFile(response, path + nameFolder + nameFile);
                            progresWindow.SetDoneFigNow(figurant);
                            App.CurUser.LogInf(new InheritanceData(App.CurUser.LoginName, TypeLogData.Access, $"Знайдено інфу по фігуранту: {figurant.Fio}, {figurant.Ipn}. Вх№:{figurant.NumbInput}"));
                        }
                        else
                        {
                            progresWindow.NotDataFigur(figurant);
                            App.CurUser.LogInf(new InheritanceData(App.CurUser.LoginName, TypeLogData.Access, $"Не знайдено інфу по фігуранту: {figurant.Fio}, {figurant.Ipn}. Вх№:{figurant.NumbInput}"));
                        }
                        ToCheckFigInTree(numberR - 1, response != null);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                });

                await searchInheritedTask;

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public byte[]? GetDocFile()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Reqstr!)
            };
            var response = client.SendAsync(request).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsByteArrayAsync().Result;
                return content;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Request failed: {response.StatusCode}");
            }
        }
        public void SaveByteToFile(byte[] fileBytes, string filePath)
        {
            File.WriteAllBytes(filePath, fileBytes);
        }
        private string? GetWhithout(string? str)
        {
            return str?.Replace('\'', '-')?.
                Replace('\"', '-')?
                .Replace('\\', '-')?
                .Replace('/', '-')?
                .Replace('*', '-')?
                .Replace(':', '-')?
                .Replace('?', '-')?
                .Replace('«', '-')?
                .Replace('<', '-')?
                .Replace('>', '-')?
                .Replace('|', '-');
        }
        public void ToCheckFigInTree(int indR, bool isHaveData)
        {
            var modelContext = new ModelContext();
            if (figurant.Control == null)
            {
                figurant.Control = new string('0', Reest.abbreviatedName.Count + 1);
            }
            if (figurant.Shema == null)
            {
                figurant.Shema = new string('0', Reest.abbreviatedName.Count + 1);
            }
            var listC = AllDirectories.GetBoolsFromString(figurant.Control);
            var listS = AllDirectories.GetBoolsFromString(figurant.Shema);
            if (isHaveData)
            {
                listC[indR] = true;
                listS[indR] = false;
            }
            else
            {
                listC[indR] = false;
                listS[indR] = true;
            }
            figurant.Control = AllDirectories.GetStringFromBools(listC);
            figurant.Shema = AllDirectories.GetStringFromBools(listS);
            modelContext.SaveChanges();
        }
    }
}
