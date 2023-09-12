using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.DTOs;
using Services.Interfaces;
using Repositories.Entities;
using Repositories.Repositories;
using Repositories.Interfaces;
using Services.Algorithm;
using System.Drawing;
using System.Net.Http;
using Newtonsoft.Json;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using Newtonsoft.Json.Linq;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using System.Diagnostics.Metrics;
using System.Net;



namespace Services.Algorithm
{
    public static class TSP
    {
        static StreamWriter writer;
        public static List<MaskDTO> masks = new List<MaskDTO>(); 
       
        static int MAX = 100000;
        static int numStores=MainAlgorithm.Stores.Count;
        
        public static async Task<List<StoreDTO>> FindShortestRoute(long bitMask)
        {
            // יצירת רשימה בה יאוחסן המסלול
            var route = new List<StoreDTO>();
            // הורדה של המיקום ההתחלתי מהמסכה
            bitMask &= (~(1 << 0));
            writer = new StreamWriter($@"c:\{DateTime.Now.ToString().Replace(@"\", "")
                .Replace(" ", "").Replace("/", "").Replace(":", "")}tt.txt");

            // יצירת מסכה בה ישמרו החנויות שנבחרו בכל שלב
            long maskWithRoute = 1;
            // שליחה לפונקציה רקורסיבית שמחזירה את עלותו של המסלול הטוב ביותר
            Task<double> routeCost = findShortestRouteRecorsive(0, bitMask, 0, maskWithRoute);
            // חילוץ מהמשימה
            double CurrentRouteCost = await routeCost;
            foreach (var m in masks)
            {
                writer.Write($"{DateTime.Now},,mask:{m.Mask}," 
                + $",cost:{m.Cost}" + Environment.NewLine);
            }
            for (int i = 0; i < numStores; i++)
            {
                for (int j = 0; j < numStores; j++)
                {
                    writer.Write($"distances[{i},{j}]:{MainAlgorithm.distances[i,j]}" + Environment.NewLine);
                }

            }
            double minCost =double.MaxValue;
            long minMask=0;
            foreach (var m in masks)
            {
                if(minCost>m.Cost)
                {
                    minCost = m.Cost;
                    minMask = m.Mask;
                }
            }
            writer.Write($"{DateTime.Now},,mask:{minMask},"
                + $",cost:{minCost}" + Environment.NewLine);

            bitMask = minMask;
           // הוספה של החנויות לפי הסדר אל המסלול 
            route = Calculations.GetShortestRouteFromMemo(numStores,bitMask);
            writer.Close();
            // החזרת המסלול
            return route;
        }
        static async Task<double> findShortestRouteRecorsive(int i, long mask,double cost,long maskWithRoute)
        {
            // תנאי עצירה
            if (mask == 0)
            {
                MaskDTO m= new MaskDTO(maskWithRoute,cost);
                masks.Add(m);
                return 0;
            }
            // במידה והחישוב של תת מסלול חושב כבר אין טעם לחשב אותו שוב והוא שמור
            if (MainAlgorithm.memo[i, maskWithRoute].Cost!=0)
                return MainAlgorithm.memo[i, maskWithRoute].Cost;
            int res = int.MaxValue;
            long minMask = mask;
            for (int j = 0; j < numStores; j++)
            {
                if ((mask & (1 << j)) != 0)
                {
                    // חישוב המרחק בין חנות לחנות או מגוגל מפות או ממטריצת המרחקים על פי הצורך
                    double dist = await CalculateDistancesByGoogleMaps
                        .CalculateDistance(MainAlgorithm.Stores[i], MainAlgorithm.Stores[j]);
                    // במידה והחנות כפולה לחנויות אחרות בשוק נצטרך לדלל אותם על ידי הפונקציה הבאה
                    long newMask = Calculations.reduceProducts(MainAlgorithm.Stores[j]);
                    // יצירת מסכה חדשה שתישלח לפעם הבאה ללא המיקום שנבחר למסלול זה
                    newMask &= mask & (~(1 << j));
                    // אל המסכה החדשה נוסיף את המיקום אותו בחרנו כדי שנוכל לשמור ביעילות במטריצת הזיכרונות
                    long newMaskWithRoute =maskWithRoute | (1L << j);
                     // חישוב פשוט של העלות עד כה למסלול זה
                    double currentCost = dist + cost;
                    // שליחה לפונקציה עצמה ברקורסיביות עם המיקום הבא והמסכות המעודכנות
                    Task<double> subCostTask = findShortestRouteRecorsive(j, newMask,currentCost, newMaskWithRoute);
                    // חילוץ מהמשימה
                    double subCost = await subCostTask;
                    // עדכון התוצאה עם המסלול המינימלי
                    if (res > currentCost )
                    {
                        res = (int)currentCost;
                        minMask = newMaskWithRoute ;
                        MainAlgorithm.memo[i, minMask].PrevStoreId = j;
                    }
                }
            }
            // הכנסת מיקום אופטימלי לטבלת הזכרונות
            MainAlgorithm.memo[i,minMask].Cost = res;
            MainAlgorithm.memo[i, minMask].LastStoreId = i;
            // החזרת התשובה
            return res;
        }
    }
}

