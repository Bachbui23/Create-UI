using Create_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_UI.Services
{
    public class ValidationGroupConfig
    {
        public Dictionary<string, string> _dataYear = new Dictionary<string, string>();

        public Dictionary<string, string> _dataMoth = new Dictionary<string, string>();

        public Dictionary<string, string> _dataDay = new Dictionary<string, string>();

        public static ValidationGroupConfig CreateValiYear()
        {
            var config = new ValidationGroupConfig();
            config._dataYear.Add("2020", "B");
            config._dataYear.Add("2021", "C");
            config._dataYear.Add("2022", "D");
            config._dataYear.Add("2023", "F");
            config._dataYear.Add("2024", "G");
            config._dataYear.Add("2025", "H");
            config._dataYear.Add("2026", "J");
            config._dataYear.Add("2027", "K");
            config._dataYear.Add("2028", "L");
            config._dataYear.Add("2029", "M");
            config._dataYear.Add("2030", "N");
            config._dataYear.Add("2031", "P");
            config._dataYear.Add("2032", "Q");
            config._dataYear.Add("2033", "R");
            config._dataYear.Add("2034", "S");
            config._dataYear.Add("2035", "T");
            return config;
        }

        public static ValidationGroupConfig CreateValiMonth()
        {
            var config = new ValidationGroupConfig();
            config._dataMoth.Add("01", "0");
            config._dataMoth.Add("02", "1");
            config._dataMoth.Add("03", "2");
            config._dataMoth.Add("04", "3");
            config._dataMoth.Add("05", "4");
            config._dataMoth.Add("06", "5");
            config._dataMoth.Add("07", "6");
            config._dataMoth.Add("08", "7");
            config._dataMoth.Add("09", "8");
            config._dataMoth.Add("10", "9");
            config._dataMoth.Add("11", "B");
            config._dataMoth.Add("12", "C");
            return config;
        }

        public static ValidationGroupConfig CreateValiDay()
        {
            var config = new ValidationGroupConfig();
            config._dataDay.Add("01", "0");
            config._dataDay.Add("02", "1");
            config._dataDay.Add("03", "2");
            config._dataDay.Add("04", "3");
            config._dataDay.Add("05", "4");
            config._dataDay.Add("06", "5");
            config._dataDay.Add("07", "6");
            config._dataDay.Add("08", "7");
            config._dataDay.Add("09", "8");
            config._dataDay.Add("10", "9");
            config._dataDay.Add("11", "B");
            config._dataDay.Add("12", "C");
            config._dataDay.Add("13", "D");
            config._dataDay.Add("14", "F");
            config._dataDay.Add("15", "G");
            config._dataDay.Add("16", "H");
            config._dataDay.Add("17", "J");
            config._dataDay.Add("18", "K");
            config._dataDay.Add("19", "L");
            config._dataDay.Add("20", "M");
            config._dataDay.Add("21", "N");
            config._dataDay.Add("22", "P");
            config._dataDay.Add("23", "Q");
            config._dataDay.Add("24", "R");
            config._dataDay.Add("25", "S");
            config._dataDay.Add("26", "T");
            config._dataDay.Add("27", "V");
            config._dataDay.Add("28", "W");
            config._dataDay.Add("29", "X");
            config._dataDay.Add("30", "Y");
            config._dataDay.Add("31", "Z");
            return config;
        }
    }
}
