using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialForm2 
{
    public static class Variable
    {
        public static string baseFolderPath = @"D:\DATASETS\RESULT";
        public static string LogPath = @"D:\DATASETS\LOG"; // 경로 설정

        //public static string Model1 = "Model 1 Title"; // 차트 레전드 제목
        public static double Model1_Chart_Minimum_X = 0D;
        public static double Model1_Chart_Maximum_X = Double.NaN;

        public static double Model1_Chart_Minimum_Y = 0D;
        public static double Model1_Chart_Maximum_Y = Double.NaN;

        public static string Model1_Sensor_Name1 = "S_001";
        public static string Model1_Sensor_Name2 = "S_002";
        public static string Model1_Sensor_Name3 = "S_003";
        public static string Model1_Sensor_Name4 = "S_004";
        public static string Model1_Sensor_Name5 = "S_005";
        public static string Model1_Sensor_Name6 = "S_006";
    }
}