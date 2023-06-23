﻿
using System.Collections.Generic;

namespace DesARMA
{
    public class Reest
    {
        public static List<string> sNazyv = new List<string>();
        public static List<string> sRodov = new List<string>();
        public static List<string> sDav = new List<string>();
        public static List<int>    sGrupa = new List<int>();
        public static List<string> sub = new List<string>
        {
            "зазначених", "зазначеної", "зазначеним", "зазначеній"
        };
        public static List<string> sub2 = new List<string>
        {
            "осіб", "особи", "особам", "особі"
        };
        public static List<string> organs = new List<string>();
        public static List<string> organsName = new List<string>();
        public static List<string> abbreviatedName = new List<string>();
        public static List<string> namesField = new List<string>()
        {
            "Найменування юридичної особи",
            "Cкорочене найменування юридичної особи",
            "Ідентифікаційний код юридичної особи",
            "Актуальний стан на фактичну дату та час формування",
            "Організаційно-правова форма юридичної особи",
            "Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить юридична особа публічного права або який здійснює функції з управління корпоративними правами держави у відповідній юридичній особі",
            "Код ЄДРПОУ. Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить юридична особа публічного права або який здійснює функції з управління корпоративними правами держави у відповідній юридичній особі",
            "Місцезнаходження юридичної особи",
            "Основний вид економічної діяльності",
            "Інші види економічної діяльності",
            "Назви органів управління юридичної особи",
            "ПІБ засновників(учасників) юридичної особи",
            "Країна громадянства засновників(учасників) юридичної особи",
            "Місцезнаходження засновників(учасників) юридичної особи",
            "Розмір частки засновників(учасників) юридичної особи",
            "Вид установчого документа",
            "Дата запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Номер запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Місце зберігання реєстраційної справи в паперовій формі",
            "Дата взяття на облік",
            "Номер взяття на облік",
            "Назва органу(облік)",
            "Ідентифікаційний код органу",
            "Перелік контактних телефонів",
            "Електронна адреса",
            "Найменування відокремленого підрозділу",
            "Код ЄДРПОУ відокремленого підрозділу",
            "Роль по відношенню до пов’язаного суб’єкта",
            "Тип відокремленого підрозділу",
            "Дата запису про створення відокремленого підрозділу",
            "ПІБ керівника відокремленого підрозділу",
            "Дата призначення керівника відокремленого підрозділу",
            "Наявність обмежень щодо представництва від імені юридичної особи керівника відокремленого підрозділу",
            "Адреса відокремленого підрозділу юридичної особи",
            "Перелік контактних телефонів відокремленого підрозділу юридичної особи",
            "Електронна адреса відокремленого підрозділу юридичної особи",
            "Інтернет сайт відокремленого підрозділу юридичної особи",
            "Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)",
            "Підстава для внесення запису про державну реєстрацію припинення юридичної особи",
            "ПІБ. Відомості про комісію з припинення",
            "Адреса. Відомості про комісію з припинення",
            "Роль. Відомості про комісію з припинення",
            "Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог",
            "Номер провадження про банкрутство",
            "Дата провадження про банкрутство",
            "Дата набуття чинності",
            "Найменування суду",
            "Повна назва суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "Адреса. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "ПІБ. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "Повна назва суб’єкта. Дані про юридичних осіб – правонаступників",
            "ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб – правонаступників",
            "Адреса. Дані про юридичних осіб – правонаступників",
            "ПІБ. Дані про юридичних осіб – правонаступників",
            "Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб – правонаступників",
            "Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб – правонаступників",
            "Повне найменування юридичної особи, через яку здійснюється опосередкований вплив",
            "Код ЄДРПОУ або ідентифікаційний  код юридичної особи, через яку здійснюється опосередкований вплив",
            "Назва країни громадянства КБВ",
            "Адреса КБВ",
            "ПІБ КБВ",
            "Тип бенефіцарного володіння",
            "Роль по відношенню до пов’язаного суб’єкта",
            "Відсоток частки статутного капіталу або відсоток права голосу",
            "ПІБ. Керівник юридичної особи, а також відомості про інших осіб, які можуть вчиняти дії від імені юридичної особи, у тому числі підписувати договори, тощо",
            "Роль суб'єкта. Керівник юридичної особи, а також відомості про інших осіб, які можуть вчиняти дії від імені юридичної особи, у тому числі підписувати договори, тощо",
            "Розмір статутного (складеного) капіталу (пайового фонду)",

    };
        public static List<string> namesFieldPDF = new List<string>()
        {
            "Найменування юридичної особи",
            "Cкорочене найменування юридичної особи",
            "Ідентифікаційний код юридичної особи",
            "Актуальний стан на фактичну дату та час формування",
            "Організаційно-правова форма юридичної особи",
            "Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить юридична особа публічного права або який здійснює функції з управління корпоративними правами держави у відповідній юридичній особі",
            "Місцезнаходження юридичної особи",
            "Перелік контактних телефонів",
            "Електронна адреса",
            "Основний вид економічної діяльності",
            "Інші види економічної діяльності",
            "Назви органів управління юридичної особи",
            "Перелік засновників(учасників) юридичної особи",
            "Інформація про кінцевого бенефіціарного власника (контролера) юридичної особи, у тому числі відомості про юридичних осіб, через яких здійснюється опосередкований вплив на юридичну особу",
            //"Повне найменування юридичної особи, через яку здійснюється опосередкований вплив",
            //"Код ЄДРПОУ або ідентифікаційний  код юридичної особи, через яку здійснюється опосередкований вплив",
            //"Назва країни громадянства КБВ",
            //"Адреса КБВ",
            //"ПІБ КБВ",
            //"Тип бенефіцарного володіння",
            //"Роль по відношенню до пов’язаного суб’єкта",
            //"Відсоток частки статутного капіталу або відсоток права голосу",
            "Керівник юридичної особи, а також відомості про інших осіб, які можуть вчиняти дії від імені юридичної особи, у тому числі підписувати договори, тощо",
            //"ПІБ. Керівник юридичної особи, а також відомості про інших осіб, які можуть вчиняти дії від імені юридичної особи, у тому числі підписувати договори, тощо",
            //"Роль суб'єкта. Керівник юридичної особи, а також відомості про інших осіб, які можуть вчиняти дії від імені юридичної особи, у тому числі підписувати договори, тощо",
            "Розмір статутного (складеного) капіталу (пайового фонду)",
            "Вид установчого документа",
            "Дата запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Дані про відокремлені підрозділи юридичної особи",
            //"Найменування відокремленого підрозділу",
            //"Код ЄДРПОУ відокремленого підрозділу",
            //"Роль по відношенню до пов’язаного суб’єкта",
            //"Тип відокремленого підрозділу",
            //"Дата запису про створення відокремленого підрозділу",
            //"ПІБ керівника відокремленого підрозділу",
            //"Дата призначення керівника відокремленого підрозділу",
            //"Наявність обмежень щодо представництва від імені юридичної особи керівника відокремленого підрозділу",
            //"Адреса відокремленого підрозділу юридичної особи",
            //"Перелік контактних телефонів відокремленого підрозділу юридичної особи",
            //"Електронна адреса відокремленого підрозділу юридичної особи",
            //"Інтернет сайт відокремленого підрозділу юридичної особи",
            "Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)",
            "Підстава для внесення запису про державну реєстрацію припинення юридичної особи",
            "Відомості про комісію з припинення",
            //"ПІБ. Відомості про комісію з припинення",
            //"Адреса. Відомості про комісію з припинення",
            //"Роль. Відомості про комісію з припинення",
            "Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог",
            "Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"Повна назва суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"Адреса. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"ПІБ. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            //"Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб, правонаступником яких є зареєстрована юридична особа",
            "Дані про юридичних осіб – правонаступників",
            //"Повна назва суб’єкта. Дані про юридичних осіб – правонаступників",
            //"ЄДРПОУ код, якщо суб’єкт – юридична особа. Дані про юридичних осіб – правонаступників",
            //"Адреса. Дані про юридичних осіб – правонаступників",
            //"ПІБ. Дані про юридичних осіб – правонаступників",
            //"Роль по відношенню до пов’язаного суб’єкта. Дані про юридичних осіб – правонаступників",
            //"Посилання на сторінку з детальною інформацією про суб'єкт. Дані про юридичних осіб – правонаступників",
            "Номер провадження про банкрутство",
            "Дата провадження про банкрутство",
            "Дата набуття чинності",
            "Найменування суду",
            "Місце зберігання реєстраційної справи в паперовій формі",
            "Номер запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Відомості, отримані в порядку інформаційної взаємодії між Єдиним державним реєстром юридичних осіб, фізичних осіб - підприємців та громадських формувань та інформаційними системами державних органів",
            //"Дата взяття на облік",
            //"Номер взяття на облік",
            //"Назва органу(облік)",
            //"Ідентифікаційний код органу",
            
            
            
            

    };
        public static List<string> namesFieldPDFFiz = new List<string>()
        {
            "Прізвище, ім'я, по батькові фізичної особи-підприємця",
            "Актуальний стан на фактичну дату та час формування",
            "Країна громадянства фізичної особи-підприємця",
            "Місцезнаходження юридичної особи",
            "Перелік контактних телефонів",
            "Електронна адреса",
            "Основний вид економічної діяльності",
            "Інші види економічної діяльності",
            "Дата запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Місце зберігання реєстраційної справи в паперовій формі",
            "Номер запису в Єдиному державному реєстрі юридичних осіб, фізичних осіб - підприємців та громадських формувань",
            "Відомості, отримані в порядку інформаційної взаємодії між Єдиним державним реєстром юридичних осіб, фізичних осіб - підприємців та громадських формувань та інформаційними системами державних органів",
    };
        public static List<string> namesFieldRPS_PDF = new List<string>()
        {
            "Власник повітряного судна",
            "Експлуатант повітряного судна",
            "Тип/модель повітряного судна",
            "Державний і реєстраційний знак повітряного судна",
            "Заводський(серійний) номер повітряного судна",
            "Рік виготовлення повітряного судна",
            "Максимальна злітна маса повітряного судна, кг",
            "№ реєстраційного посвідчення повітряного судна",
            "Дата видачі реєстраційного посвідчення повітряного судна"
        };
        public static List<string> namesFieldSK_PDF = new List<string>()
        {
            "Власник юридична/фізична особа",
            "Судновий білет",
            "Порядковий номер у судновій книзі",
            "Серія та номер суднової книги",
            "Бортовий реєстраційний номер судна (присвоєний)",
            "Марка",
            "Тип",
            "Призначення судна",
            "Рік побудови",
            "Дата виключення",
            "Підстава виключення",
            "Прізвище Ім′я По-батькові",
            "Довжина найбільша (метрів)",
            "Ширина (метрів)",
            "Висота борту (метрів)",
            "Матеріал корпусу",
            "Модель 1-го двигуна",
            "Номер 1-го двигуна",
            "Потужність 1-го двигуна (е.к.с.)"
        };
        public static List<string> namesFieldSK_PDF_N = new List<string>()
        {
            "Судновий білет",
            "Порядковий номер у судновій книзі",
            "Серія та номер суднової книги",
            "Дата реєстрації",
            "Бортовий реєстраційний номер судна",
            "Бортовий реєстраційний номер судна (колишній)",
            "Марка",
            "Тип (старе значення)",
            "Тип",
            "Модель",
            "Призначення судна",
            "Рік побудови",
            "Країна побудови",
            "Місце побудови судна",
            "Заводський номер",
            "Орган реєстрації",
            "Новий орган реєстрації при перереєстрації",
            "Дата виключення",
            "Підстава виключення",
            "Дата тимчасового виключення",
            "Дата поновлення реєстрації",
            "Термін дії договору фрахтування",
            "Власник держава",
            "Дата народження",
            "Паспорт",
            "Прізвище Ім′я По-батькові",
            "Ідентифікаційний код",
            "Назва організації",
            "Код ЄДРПОУ",
            "Область",
            "Район",
            "Населений пункт",
            "Вулиця",
            "№ будівлі",
            "№ корпусу",
            "№ квартири (офісу)",
            "Прізвище Ім′я По-батькові (фрахтувальник)",
            "Ідентифікаційний код",
            "Назва організації",
            "Код ЄДРПОУ",
            "Область",
            "Район",
            "Населений пункт",
            "Вулиця",
            "№ будівлі",
            "№ корпусу",
            "№ квартири (офісу)",
            "Орган видачі",
            "Дата видачі",
            "Екіпаж (чоловік)",
            "Довжина найбільша (метрів)",
            "Ширина (метрів)",
            "Висота борту (метрів)",
            "Матеріал корпусу",
            "Швидкість ходу (вузлів)",
            "Вантажопідйомність (тонн)",
            "Модель 1-го двигуна",
            "Номер 1-го двигуна",
            "Потужність 1-го двигуна (кВт)"
        };
        public static List<string> listOPFG = new List<string>()
        {
            "Об'єднання підприємств",
            "Приватне акціонерне товариство",
            "Фермерське господарство",
            "Приватне підприємство",
            "Державне підприємство",
            "Казенне підприємство",
            "Комунальне підприємство",
            "Спільне комунальне підприємство",
            "Дочірнє підприємство",
            "Іноземне підприємство",
            "Підприємство об’єднання громадян (релігійної організації, профспілки)",
            "Підприємство споживчої кооперації",
            "Акціонерне товариство",
            "Відкрите акціонерне товариство",
            "Закрите акціонерне товариство",
            "Державна акціонерна компанія (товариство)",
            "Державна холдингова компанія",
            "Холдингова компанія",
            "Товариство з обмеженою відповідальністю",
            "Товариство з додатковою відповідальністю",
            "Повне товариство",
            "Командитне товариство",
            "Адвокатське об’єднання",
            "Адвокатське бюро",
            "Виробничий кооператив",
            "Обслуговуючий кооператив",
            "Житлово-будівельний кооператив",
            "Гаражний кооператив",
            "Споживчий кооператив",
            "Споживче товариство",
            "Сільськогосподарський виробничий кооператив",
            "Сільськогосподарський обслуговуючий кооператив",
            "Кооперативний банк",
            "Орган державної влади",
            "Орган місцевого самоврядування",
            "Державна організація (установа, заклад)",
            "Комунальна організація (установа, заклад)",
            "Приватна організація (установа, заклад)",
            "Організація (установа, заклад) об’єднання громадян (релігійної організації, профспілки, споживчої кооперації тощо)",
            "Спілка споживчих товариств",
            "Органи адвокатського самоврядування",
            "Органи суддівського самоврядування",
            "Вища кваліфікаційна комісія суддів України",
            "Фонди соціального страхування",
            "Саморегулівні організації",
            "Торгово-промислова палата",
            "Аудиторська палата України",
            "Політична партія",
            "Громадська організація",
            "Громадська спілка",
            "Релігійна організація",
            "Творча спілка (інша професійна організація)",
            "Благодійна організація",
            "Організація роботодавців",
            "Об’єднання співвласників багатоквартирного будинку",
            "Орган самоорганізації населення",
            "Підприємець-фізична особа",
            "Товарна біржа",
            "Фондова біржа",
            "Кредитна спілка",
            "Недержавний пенсійний фонд",
            "Садівниче товариство"
            
        };
        public static List<string> listOPFG_Abbreviation = new List<string>()
        {
            "ФГ",
            "ПП",
            "ДП",
            "КП",
            "КП",
            "СКП",
            "ДП",
            "ІП",
            "ПОГ",
            "ПСК",
            "АТ",
            "ВАТ",
            "ЗАТ",
            "ДАК",
            "ДХК",
            "ХК",
            "ТОВ",
            "ТДВ",
            "ПТ",
            "КТ",
            "АО",
            "АБ",
            "ВК",
            "ОК",
            "ЖБК",
            "ГК",
            "СК",
            "СТ",
            "СВК",
            "СОК",
            "КБ",
            "ОДС",
            "ОМС",
            "ДО",
            "КО",
            "ПО",
            "ООГ",
            "ССТ",
            "ОАС",
            "ОСС",
            "ВККСУ",
            "ФСС",
            "СО",
            "ТПП",
            "АПУ",
            "ПП",
            "ГО",
            "ГС",
            "РО",
            "ТС",
            "БО",
            "ОР",
            "ОСББ",
            "ОСН",
            "ФОП",
            "ТБ",
            "ФБ",
            "КС",
            "НПФ",
            "СТ"
        };
        public static List<string> namesFieldDSRU_PDF = new List<string>()
        {
            "Дата першої реєстрацій в ДРСУ (з 1.1.2011)",
            "Реєстраційний №",
            "Дата реєстрації",
            "Характер реєстрації",
            "Сучасна назва судна",
            "Модель судна",
            "Ідентифікаційний № корпусу",
            "Тип судна",
            "Призначення",
            "Порт припису",
            "Країна",
            "Район плавання",
            "Класифікаційна установа",
            "IMO",
            "INMARSAT",
            "Прізвище Ім'я По-Батькові",
            "Назва (юр)",
            "Назва юр. (фрахтувальник)",
            "Довжина, м",
            "Ширина, м",
            "Висота, м",
            "Валова місткість, од",
            "Матеріал корпусу",
            "Тип головних механізмів",
            "Потужність головних механізмів",
            "Кількість головних механізмів, од",
            "Тимчасова відмітка про виключення судна з Державного реєстру",
            "Постійна відмітка про виключення судна з Державного реєстру"
        };
        public static List<string> namesFieldDSRU_PDF_N = new List<string>()
        {
            "Дата першої реєстрацій в ДРСУ (з 1.1.2011)",
            "Реєстраційний №",
            "Дата реєстрації",
            "Характер реєстрації",
            "Документ - підстава для реєстрації",
            "Сучасна назва судна",
            "Колишня назва судна",
            "Модель судна",
            "Ідентифікаційний № корпусу",
            "Тип судна",
            "Призначення",
            "Порт припису",
            "Країна",
            "Рік побудови",
            "Район плавання",
            "Класифікаційна установа",
            "IMO",
            "INMARSAT",
            "Суднобудівна верф",
            "Конвенційна установа",
            "Прізвище Ім'я По-Батькові",
            "Ідентифікаційний код",
            "Паспорт",
            "Дата народження",
            "Назва (юр)",
            "Код ЄДРПОУ",
            "Юридична адреса",
            "Назва юр. (фрахтувальник)",
            "Код ЄДРПОУ (фрахтувальник)",
            "Прізвище Ім′я По-батькові (фрахтувальник)",
            "Дата народження (фрахтувальник)",
            "Паспорт (фрахтувальник)",
            "Ідентифікаційний код (фрахтувальник)",
            "Юридична адреса (фрахтувальник)",
            "Довжина, м",
            "Ширина, м",
            "Висота, м",
            "Валова місткість, од",
            "Матеріал корпусу",
            "Тип головних механізмів",
            "Потужність головних механізмів, Квт",
            "Кількість головних механізмів, од",
            "Екіпаж, ос",
            "Пасажири, ос",
            "Дедвейт, т",
            "Кількість щогл, од",
            "Тимчасова відмітка про виключення судна з Державного реєстру",
            "Постійна відмітка про виключення судна з Державного реєстру",
            "НОМЕР ЕNI  та ким присвоєний"
        };
        public static List<int> activeNumbReestr = new()
        {
            15,
            16,
            17,
            18,
            19,
            20,
            28,
            38,
            39,
            40
        };
    }
}
