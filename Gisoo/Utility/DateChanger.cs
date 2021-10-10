using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gisoo.Utility
{
	public static class DateChanger
	{
		public static DateTime ToGeorgianDateTime(this string persianDate)
		{
			int year = Convert.ToInt32(persianDate.Substring(0, 4));
			int month = Convert.ToInt32(persianDate.Substring(5, 2));
			int day = Convert.ToInt32(persianDate.Substring(8, 2));
			DateTime georgianDateTime = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
			return georgianDateTime;
		}

		/// <summary>
		/// یک تاریخ میلادی را به معادل فارسی آن تبدیل میکند
		/// </summary>
		/// <param name="georgianDate">تاریخ میلادی</param>
		/// <returns>تاریخ شمسی</returns>
		public static string ToPersianDateString(this DateTime georgianDate)
		{
			System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();

			string year = persianCalendar.GetYear(georgianDate).ToString();
			string month = persianCalendar.GetMonth(georgianDate).ToString().PadLeft(2, '0');
			string day = persianCalendar.GetDayOfMonth(georgianDate).ToString().PadLeft(2, '0');
			string persianDateString = string.Format("{0}/{1}/{2}", year, month, day);
			return persianDateString;
		}

		/// <summary>
		/// یک تعداد روز را از یک تاریخ شمسی کم میکند یا به آن آضافه میکند
		/// </summary>
		/// <param name="georgianDate">تاریخ شمسی اول</param>
		/// <param name="days">تعداد روزی که میخواهیم اضافه یا کم کنیم</param>
		/// <returns>تاریخ شمسی به اضافه تعداد روز</returns>
		public static string AddDaysToShamsiDate(this string persianDate, int days)
		{
			DateTime dt = persianDate.ToGeorgianDateTime();
			dt = dt.AddDays(days);
			return dt.ToPersianDateString();
		}
		public static string PersianToEnglish(this string persianStr)
     {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',['۱'] = '1',['۲'] = '2',['۳'] = '3',['۴'] = '4',['۵'] = '5',['۶'] = '6',['۷'] = '7',['۸'] = '8',['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
				if (item.ToString() == "/")
					continue;
                persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            return persianStr;
     }
		public static string EnglishToPersian(this string englishStr)
     {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['0'] = '۰',['1'] = '۱',['2'] = '۲',['3'] = '۳',['4'] = '۴',['5'] = '۵',['6'] = '۶',['7'] = '۷',['8'] = '۸',['9'] = '۹'
            };
            foreach (var item in englishStr)
            {
				if (item.ToString() == "/")
					continue;
                englishStr = englishStr.Replace(item, LettersDictionary[item]);
            }
            return englishStr;
     }
		 public static string calculatDate(DateTime value)
        {
            DateTime dtNow = DateTime.Now;
            TimeSpan dt = (dtNow - value);
            string Text = "";
            if (dt.Days > 0)
            {
                Text += dt.Days + " روز ";
                Text += " قبل ";
                return Text;
            }
            if (dt.Hours > 0)
            {
                Text += dt.Hours + " ساعت ";
                Text += " قبل ";
                return Text;
            }
            if (dt.Minutes > 0)
            {
                Text += dt.Minutes + " دقیقه ";
                Text += " قبل ";
                return Text;
            }
            Text += " لحظاتی قبل ";
            return Text;
        }
	}
}