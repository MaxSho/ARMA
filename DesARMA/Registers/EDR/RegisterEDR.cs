using DesARMA.Entities;
using DesARMA.Models;
using IronXL;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace DesARMA.Registers.EDR
{
    class RegisterEDR: Register
    {
        public RequestProgram requestProgram = null!;
        public override void GetData()
        {
            if(requestProgram != null)
            {
                var listFigurants = requestProgram.GetAllFigurants();

                //пройти усіх фігурантів ...
                foreach (var itemFigurant in listFigurants)
                {
                    if (itemFigurant != null)
                    {
                        var code = itemFigurant.Code;
                        //var name = itemFigurant.Name;
                        //var passpor;

                        string resp = RunEDR(code!, "", "");

                        var obj = JsonConvert.DeserializeObject<EDRClass[]>(resp);

                        foreach (var itemObj in obj)
                        {
                            if (itemObj != null)
                            {
                                GetVirtualFile((int)itemObj.id, itemObj.name);
                            }
                        }
                    }

                }
            }
        }
        private static string RunEDR(string code, string name, string passport)
        {
            string Response = "";
            try
            {
                HttpWebRequest _request;
                string _adress = $"https://34997e73492e46a0521e844f46fbf385.nais.gov.ua/1.0/subjects?code={code}&name={name}&passport={passport}";
                _request = WebRequest.Create(_adress) as HttpWebRequest;
                _request.Method = "GET";


                _request.Headers.Add("Authorization", "Token 73bf9d80246079612df5095f8b5421dc8c54b529");


                using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        Response = new StreamReader(stream).ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Response;
        }

        private static string RunEDR_All(int id)
        {
            string Response = "";
            try
            {
                HttpWebRequest _request;
                string _adress = $"https://34997e73492e46a0521e844f46fbf385.nais.gov.ua/1.0/subjects/{id}";
                _request = WebRequest.Create(_adress) as HttpWebRequest;
                _request.Method = "GET";


                _request.Headers.Add("Authorization", "Token 73bf9d80246079612df5095f8b5421dc8c54b529");

                using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        Response = new StreamReader(stream).ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Response;
        }

        public static void GetVirtualFile(int id, string nameF)
        {
            var response = RunEDR_All(id);
            var subject = JsonConvert.DeserializeObject<Subject>(response);
            if (response == null) return;

            WorkBook workbook = WorkBook.Create(ExcelFileFormat.XLSX);
            var sheet = workbook.CreateWorkSheet("EDR");

            var excelCounter = new ExcelCounter();
            var carrentExcelCounter = "";
            //1
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Найменування юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.names.display;

            //2
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Cкорочене найменування юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.names.@short;

            //3
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Ідентифікаційний код юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.code;

            //4
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Актуальний стан на фактичну дату та час формування";
            sheet[carrentExcelCounter + "2"].Value = subject.state_text;

            //5
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Організаційно-правова форма юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.olf_name;

            //6
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить юридична особа публічного права або який здійснює функції з управління корпоративними правами держави у відповідній юридичній особі";
            sheet[carrentExcelCounter + "2"].Value = subject.executive_power.name;

            //7
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Код ЄДРПОУ. Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить юридична особа публічного права або який здійснює функції з управління корпоративними правами держави у відповідній юридичній особі";
            sheet[carrentExcelCounter + "2"].Value = subject.executive_power.code;

            //8
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Місцезнаходження юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.address.address;

            //9
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Основний вид економічної діяльності";
            sheet[carrentExcelCounter + "2"].Value = $"{subject.primary_activity_kind.code} {subject.primary_activity_kind.name}";

            //10
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Інші види економічної діяльності";

            int i = 2;
            if(subject.activity_kinds !=null)
            foreach (var item in subject.activity_kinds)
            {
                    if (item != null)
                    {
                        var is_primary = item.is_primary;
                        if (is_primary != null)
                        {
                            if (!(bool)is_primary)
                            {
                                sheet[carrentExcelCounter + $"{i++}"].Value = $"{item.code} {item.name}";
                            }
                        }
                    }
                
            }

            //11
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Назви органів управління юридичної особи";
            sheet[carrentExcelCounter + "2"].Value = subject.management;

            //12-15
            carrentExcelCounter = excelCounter.GetStringCounter();
            int tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "ПІБ засновників(учасників) юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Країна громадянства засновників(учасників) юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Місцезнаходження засновників(учасників) юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Розмір частки засновників(учасників) юридичної особи";

            i = 2;
            foreach (var item in subject.founders)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + $"{i}"].Value = item.name;
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + $"{i}"].Value = item.country;
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                if (item.address != null)
                    sheet[carrentExcelCounter + $"{i}"].Value = item.address.address;
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + $"{i}"].Value = item.capital;
                i++;
                // sheet[$"K{i++}"].Value = $"{item.name}, Країна громадянства: {item.country}, Місцезнаходження: {item.address.address}, Розмір частки засновника (учасника): {item.capital}";
            }

            //16
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Вид установчого документа";
            sheet[carrentExcelCounter + "2"].Value = subject.founding_document;

            //17
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань";
            sheet[carrentExcelCounter + "2"].Value = subject.registration.record_date;

            //18
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Номер запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань";
            sheet[carrentExcelCounter + "2"].Value = subject.registration.record_number;

            //19
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Місце зберігання реєстраційної справи в паперовій формі";
            sheet[carrentExcelCounter + "2"].Value = subject.object_name;

            //20-23
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "Дата взяття на облік";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Номер взяття на облік";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Назва органу(облік)";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Ідентифікаційний код органу";

            i = 2;
            foreach (var item in subject.registrations)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + $"{i}"].Value = item.start_date;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + $"{i}"].Value = item.start_num;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                sheet[carrentExcelCounter + $"{i}"].Value = item.name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + $"{i}"].Value = item.code;

                i++;
            }

            //24
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Перелік контактних телефонів";
            sheet[carrentExcelCounter + "2"].Value = subject.contacts.tel;

            i = 2;
            foreach (var item in subject.contacts.tel)
            {
                sheet[$"{carrentExcelCounter}{i++}"].Value = item;
            }

            //25
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Електронна адреса";
            sheet[carrentExcelCounter + "2"].Value = subject.contacts.email;

            //26-37
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "Найменування відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Код ЄДРПОУ відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Роль по відношенню до пов’язаного суб’єкта";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Тип відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата запису про створення відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "ПІБ керівника відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата призначення керівника відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Наявність обмежень щодо представництва від імені юридичної особи керівника відокремленого підрозділу";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Адреса відокремленого підрозділу юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Перелік контактних телефонів відокремленого підрозділу юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Електронна адреса відокремленого підрозділу юридичної особи";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Інтернет сайт відокремленого підрозділу юридичної особи";

            i = 2;
            foreach (var item in subject.branches)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + $"{i}"].Value = item.name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + $"{i}"].Value = item.code;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                sheet[carrentExcelCounter + $"{i}"].Value = item.role_text;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + $"{i}"].Value = item.type_text;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 4);
                sheet[carrentExcelCounter + $"{i}"].Value = item.create_date;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 5);
                Head head = Funks.GetTheNewestHead(item.heads);
                if (head != null)
                {
                    sheet[carrentExcelCounter + $"{i}"].Value = head.name + " " + head.first_middle_name;

                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 6);
                    sheet[carrentExcelCounter + $"{i}"].Value = head.appointment_date;

                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 7);
                    sheet[carrentExcelCounter + $"{i}"].Value = head.restriction;
                }
                if (item.address != null)
                {
                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 8);
                    sheet[carrentExcelCounter + $"{i}"].Value = item.address.address;
                }
                if (item.contacts != null)
                {
                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 9);
                    string str = "";

                    foreach (var itemTel in item.contacts.tel)
                    {
                        str += itemTel + "; ";
                    }
                    sheet[carrentExcelCounter + $"{i}"].Value = str;

                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 10);
                    sheet[carrentExcelCounter + $"{i}"].Value = item.contacts.email;

                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 11);
                    sheet[carrentExcelCounter + $"{i}"].Value = item.contacts.web_page;
                }


                i++;
            }

            //38
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)";

            if (subject.termination != null)
            {
                sheet[carrentExcelCounter + "2"].Value = subject.termination.date;
            }

            //39
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Підстава для внесення запису про державну реєстрацію припинення юридичної особи";

            if (subject.termination != null)
            {
                sheet[carrentExcelCounter + "2"].Value = subject.termination.cause;
            }

            //40-41
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "ПІБ. Відомості про комісію з припинення";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Адреса. Відомості про комісію з припинення";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Роль. Відомості про комісію з припинення";


            i = 2;
            foreach (var item in subject.heads)
            {
                if (item.role == 7 || item.role == 8 || item.role == 13 || item.role == 18)
                {
                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                    sheet[carrentExcelCounter + $"{i}"].Value = item.last_name + " " + item.first_middle_name;


                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                    if (item.address != null)
                        sheet[carrentExcelCounter + $"{i}"].Value = item.address.address;

                    carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                    sheet[carrentExcelCounter + $"{i}"].Value = item.role_text;

                    i++;
                }
            }

            //43
            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог";
            sheet[carrentExcelCounter + "2"].Value = subject.prev_registration_end_term;

            //44-48 bankruptcy 
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "Номер провадження про банкрутство";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата провадження про банкрутство";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Дата набуття чинності";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Найменування суду";

            if (subject.bankruptcy != null)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + "2"].Value = subject.bankruptcy.doc_number;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + "2"].Value = subject.bankruptcy.doc_date;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                sheet[carrentExcelCounter + "2"].Value = subject.bankruptcy.date_judge;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + "2"].Value = subject.bankruptcy.court_name;
            }

            //49-54
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "Повна назва суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Адреса. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "ПІБ. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа";


            i = 2;
            foreach (var item in subject.assignees)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + $"{i}"].Value = item.name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + $"{i}"].Value = item.code;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                if (item.address != null)
                    sheet[carrentExcelCounter + $"{i}"].Value = item.address.address;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + $"{i}"].Value = item.last_name + " " + item.first_middle_name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 4);
                sheet[carrentExcelCounter + $"{i}"].Value = item.role_text;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 5);
                sheet[carrentExcelCounter + $"{i}"].Value = item.url;
            }

            //55-60
            carrentExcelCounter = excelCounter.GetStringCounter();
            tempValue = excelCounter.counter - 1;
            sheet[carrentExcelCounter + "1"].Value = "Повна назва суб’єкта. Дані про юридичних осіб – правонаступників";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб – правонаступників";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Адреса. Дані про юридичних осіб – правонаступників";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "ПІБ. Дані про юридичних осіб – правонаступників";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб – правонаступників";

            carrentExcelCounter = excelCounter.GetStringCounter();
            sheet[carrentExcelCounter + "1"].Value = "Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб – правонаступників";


            i = 2;
            foreach (var item in subject.predecessors)
            {
                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue);
                sheet[carrentExcelCounter + $"{i}"].Value = item.name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 1);
                sheet[carrentExcelCounter + $"{i}"].Value = item.code;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 2);
                if (item.address != null)
                    sheet[carrentExcelCounter + $"{i}"].Value = item.address.address;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 3);
                sheet[carrentExcelCounter + $"{i}"].Value = item.last_name + " " + item.first_middle_name;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 4);
                sheet[carrentExcelCounter + $"{i}"].Value = item.role_text;

                carrentExcelCounter = excelCounter.GetGetStringForNumber(tempValue + 5);
                sheet[carrentExcelCounter + $"{i}"].Value = item.url;
            }
            // File(workbook.ToBinary(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"EDR-Subject{id}.xlsx");
            workbook.SaveAs($"C:\\app\\NewExcelFile {nameF} - {id}.xlsx"); 
        }
    }
}
