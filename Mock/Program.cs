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

namespace Mock
{

    public class StoreFinder
    {
        private readonly IService<StoreDTO> _store;
        public static List<StoreDTO> listOfStores = new List<StoreDTO>();
        public StoreFinder(IService<StoreDTO> store)
        {
            _store = store;
        }
        public async void init()
        {
            listOfStores = await _store.GetAllAsync();
        }
        //פונקציה שמקבלת רשימה של מוצרים ורשימה של חנויות ומחזירה רשימה של החנויות האפשריות
        public static List<StoreDTO> FindStoresWithProducts(List<ProductDTO> products)
        {
            StoreDTO s0 = new StoreDTO { StoreId = 0 };
            var result = new List<StoreDTO>();
            foreach (var store in TCP.listOfStores)
            {
                //bool hasAllProducs = true;
                int count = 0;

                foreach (var product in products)
                {
                    if (store.Products.Contains(product) || TCP.listOfStoresFromTheUser.Contains(store))
                    {
                        //hasAllProducs = true;
                        result.Add(store);
                        break;
                    }
                    else
                    {
                        count++;
                    }

                }
                if (count == TCP.listOfProducts.Count)
                {
                    result.Add(s0);

                }
            }
            return result;
        }
        //פונקציה שממירה את רשימת החנויות למסכה ומחזירה רשימה של 1 או 0 אם צריך את החנות או לא
        public static List<bool> StoresToBinaryList(List<StoreDTO> stores)
        {
            List<bool> binaryList = new List<bool>();
            foreach (StoreDTO store in TCP.listOfStores)
            {
                if (stores.Contains(store))
                {
                    binaryList.Add(true);
                }
                else
                {
                    binaryList.Add(false);
                }
            }
            return binaryList;
        }

        //פונקציה שממירה את הרשימה הבינארית למסכה
        public static long ReverseAndConvertToMask(List<bool> list)
        {
            //list.Reverse();
            // initialize bitmask and bit position
            long mask = 0;
            int bitPos = 0;

            // loop through list and set corresponding bit in mask
            foreach (bool bit in list)
            {
                if (bit == true)
                {
                    mask |= (1L << bitPos);
                }
                bitPos++;
            }

            return mask;
        }

        //פונקציה ש:
        //(מקבלת את רשימת המוצרים המעודכנת(שחלק מהמוצרים כבר לא נמצאים-מה שהשתמשו כבר
        //שולחת לפונקציה שמחזירה אילו חנויות צריך עכשיו- כלומר אחרי הורדה של מוצרים
        //שולחת לפונקציה נוספת שממירה את רשימת החנויות למסכה
        //מחזירה את המסכה כדי שנוכל להמשיך לעבוד עליה
        public static long removesDuplicates(List<ProductDTO> products)
        {
            var result = new List<StoreDTO>();
            result = FindStoresWithProducts(products);
            List<bool> binaryMask = StoresToBinaryList(result);
            TCP.listOfStores = result;
            long mask = ReverseAndConvertToMask(binaryMask);
            return mask;
        }
        //הפונקציה עוברת על רשימת הביטים-החנויות עבור כל חנות בודקת איזה מוצרים מהרשימה היא יכולה להוריד
        //ומורידה את כל המוצרים מכל החנויות על ידי הורדה מרשימת המוצרים של החנויות האחרות -מורידה את המוצרים וירטואלית כדי שלא ימחקו למסלול הבא
        //הפונקציה מחזירה מסכה חדשה בלי החנויות שהורדו
        public static long reduceProducts(StoreDTO store)
        {
            foreach (ProductDTO product in store.Products)
            {
                if (TCP.listOfProducts.Contains(product))
                {
                    TCP.listOfStores.Where(s => s.StoreId == store.StoreId).Last().ProductsUse.Add(product.ProductName);
                    foreach (StoreDTO store1 in TCP.listOfStores)
                    {
                        if (store.StoreId != store1.StoreId && !(TCP.listOfStoresFromTheUser.Contains(store1)))
                            if (TCP.listOfStores.Where(s => s.StoreId == store1.StoreId).Last().Products.Contains(product))
                                TCP.listOfStores.Where(s => s.StoreId == store1.StoreId).Last().Products.Remove(product);
                    }
                }
            }
            return removesDuplicates(TCP.listOfProducts);
        }
        //google-maps-places
        public async static Task<string> distByGoogleMaps(StoreDTO store1, StoreDTO store2)
        {
            var apiKey = "AIzaSyAnxSG6GhxSuLbkuQ3YTdY6iJuSB0IHFJk\r\n";
            var origin = store1.StreatName;
            var destination = store2.StreatName;

            using var httpClient = new HttpClient();
            var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&mode=walking&key={apiKey}";
            var response = await httpClient.GetAsync(url);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var json = JObject.Parse(responseContent);
            var distance = json["routes"][0]["legs"][0]["distance"]["text"].ToString();
            return distance;
        }

        //פונקציה שמכניסה את כל המיקומים מגוגל מפות לטבלת מרחקים בה נשתמש
        public async static Task CalculateTheDistToTable()
        {
            string distWithW;
            string km;
            double distance;

            for (int i = 0; i < TCP.listOfStores.Count; i++)
            {
                for (int j = i; j < TCP.listOfStores.Count; j++)
                {
                    if (i == j)
                        TCP.distances[i, j] = 0;
                    else
                    {
                        distWithW = await distByGoogleMaps(TCP.listOfStores[i], TCP.listOfStores[j]);
                        km = distWithW.Split(" ")[1];
                        if (km == "km")
                        {
                            distance = double.Parse(distWithW.Substring(0, distWithW.IndexOf(' '))) * 1000;
                        }
                        else
                            distance = (double.Parse(distWithW.Substring(0, distWithW.IndexOf(' '))));
                        TCP.distances[i, j] = distance;
                        TCP.distances[j, i] = distance;
                    }
                }
            }

        }
        //חישוב עדיפות לכל חנות
        public static void setPriority()
        {
            double count = 0;
            foreach (StoreDTO store in TCP.listOfStores)
            {
                if (store.priority == 0)
                {
                    count = 0;
                    foreach (ProductDTO product in TCP.listOfProducts)
                    {
                        if (store.Products.Contains(product))
                            count++;
                    }
                    if (count > 0)
                        store.priority = count / TCP.listOfProducts.Count;
                }
            }
        }
        public async static Task<int> findTheClosetStore()
        {
            //מציאת החנות הקרובה לנקודת ההתחלה
            //מקור היציאה נחשב כ"חנות" ראשונה ולכן נאתמול אותו בטבלת המרחקים להיות חנות מס אפס
            StoreDTO enter = new StoreDTO { StreatName = "מחנה יהודה & יפו, ירושלים" };

            double distance;

            int closestStoreIndex = 1;

            double minDistance = double.MaxValue;
            string distanceWithKm;
            string km;
            double priority = 0;
            foreach (StoreDTO st in listOfStores)
            {
                if (st.StoreId != 0)
                {
                    distanceWithKm = await distByGoogleMaps(enter, st);
                    km = distanceWithKm.Split(" ")[1];
                    if (km == "km")
                    {
                        distance = double.Parse(distanceWithKm.Substring(0, distanceWithKm.IndexOf(' '))) * 1000;
                    }
                    else
                        distance = (double.Parse(distanceWithKm.Substring(0, distanceWithKm.IndexOf(' '))));

                    if (st.priority > 1)
                        priority = 1;
                    if (st.priority < 0)
                        priority = 0;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestStoreIndex = (int)st.StoreId;
                        priority = st.priority;
                    }
                    if (distance == minDistance && st.priority > priority)
                    {
                        minDistance = distance;
                        closestStoreIndex = (int)st.StoreId;
                        priority = st.priority;
                    }

                }
            }
            return closestStoreIndex;
        }

        static public class TCP
        {

            // A function that takes a bit mask of stores with fiber masks   and returns the shortest route to visit all stores
            public static async Task<List<StoreDTO>> FindShortestRoute(long bitMask, int numStores)
            {
                // יצירת רשימה בה יהיה מאוחסן המסלול
                var route = new List<StoreDTO>();

                int closestStoreIndex = await findTheClosetStore();

                // הוספת החנות הראשונה למסלול
                route.Add(TCP.listOfStores[closestStoreIndex - 1]);

                // מחיקת החנות הראשונה מהמסכה
                bitMask &= ~(1L << closestStoreIndex - 1);
                // הורדת שאר החנויות הכפולות לחנות זו 
                bitMask &= reduceProducts(TCP.listOfStores[closestStoreIndex - 1]);

                // מציאת המסלול הקצר ביותר בין החנויות משתמש באלגוריתם הדינאמי
                while (bitMask > 0)
                {
                    // מציאת החנות הקרובה ביותר לחנות האחרונה שהצטרפה למסלול
                    int nearestStore = -1;
                    double minDistance = double.MaxValue;
                    string km;
                    double distance;
                    double priority = 0;
                    for (int i = 0; i < numStores; i++)
                    {
                        if ((bitMask & (1L << i)) != 0)
                        {
                            distance = TCP.distances[i, ((int)route.Last().StoreId) - 1];

                            if (TCP.listOfStores[i].priority > 1)
                            {
                                priority = 1;

                            }
                            if (TCP.listOfStores[i].priority < 0)
                                priority = 0;

                            if (distance < minDistance)
                            {
                                priority = TCP.listOfStores[i].priority;
                                minDistance = distance;
                                nearestStore = i;
                            }
                            if (distance == minDistance)
                            {
                                if (TCP.listOfStores[i].priority > priority)
                                {
                                    priority = TCP.listOfStores[i].priority;
                                    minDistance = distance;
                                    nearestStore = i;
                                }
                            }

                        }

                    }

                    // הוספת החנות שנמצאה למסלול
                    route.Add(TCP.listOfStores[nearestStore]);

                    // הורדת החנות הזאת מהמסכה
                    bitMask &= ~(1L << nearestStore);
                    //הורדת עוד חנויות שכבר לא יצטרכו בדיקה רק אם זו לא חנות מרשימת החנויות שאנחנו רוצים לבקר בהם בלי קשר למוצרים
                    if (!(TCP.listOfStoresFromTheUser.Contains(TCP.listOfStores[nearestStore])))
                    {
                        bitMask &= reduceProducts(TCP.listOfStores[nearestStore]);
                    }
                }
                // החזרת המסלול הקצר
                return route;
            }


            // memoization for top down recursion
            //static int[,] memo = new int[(n + 1), (1 << (n + 1))];
            static List<ProductDTO> listP = new List<ProductDTO>();
            static List<ProductDTO> listP2 = new List<ProductDTO>();
            static List<ProductDTO> listP3 = new List<ProductDTO>();
            static List<ProductDTO> listP4 = new List<ProductDTO>();
            static List<ProductDTO> listP5 = new List<ProductDTO>();
            static List<ProductDTO> listP6 = new List<ProductDTO>();
            static List<ProductDTO> listP7 = new List<ProductDTO>();
            static List<ProductDTO> listP8 = new List<ProductDTO>();
            public static List<ProductDTO> listOfProducts = new List<ProductDTO>();

            static List<StoreDTO> listS = new List<StoreDTO>();
            static List<StoreDTO> listS2 = new List<StoreDTO>();
            static List<StoreDTO> listS3 = new List<StoreDTO>();
            static List<StoreDTO> listS4 = new List<StoreDTO>();
            static List<StoreDTO> listS5 = new List<StoreDTO>();
            static List<StoreDTO> listS6 = new List<StoreDTO>();
            static List<StoreDTO> listS7 = new List<StoreDTO>();
            static List<StoreDTO> listS8 = new List<StoreDTO>();
            static List<StoreDTO> listS9 = new List<StoreDTO>();
            static List<StoreDTO> listS10 = new List<StoreDTO>();
            static List<StoreDTO> listS11 = new List<StoreDTO>();
            static List<StoreDTO> listS12 = new List<StoreDTO>();

            public static List<StoreDTO> listOfStores = new List<StoreDTO>();
            public static List<StoreDTO> listOfStoresFromTheUser = new List<StoreDTO>();

            public static int numStores;
            //מטריצת המרחקים בין חנות לחנות בתוך השוק
            public static double[,] distances;


            public async static Task Main(string[] args)
            {
                ProductDTO p1 = new ProductDTO { ProductId = 1, ProductName = "milk", Stores = listS };
                ProductDTO p2 = new ProductDTO { ProductId = 2, ProductName = "bread", Stores = listS2 };
                ProductDTO p3 = new ProductDTO { ProductId = 3, ProductName = "tuna", Stores = listS3 };
                ProductDTO p4 = new ProductDTO { ProductId = 4, ProductName = "egs", Stores = listS4 };
                ProductDTO p5 = new ProductDTO { ProductId = 5, ProductName = "cheese", Stores = listS5 };
                ProductDTO p6 = new ProductDTO { ProductId = 6, ProductName = "avokado", Stores = listS6 };
                ProductDTO p7 = new ProductDTO { ProductId = 7, ProductName = "afarsek", Stores = listS7 };
                ProductDTO p8 = new ProductDTO { ProductId = 8, ProductName = "table", Stores = listS8 };
                ProductDTO p9 = new ProductDTO { ProductId = 9, ProductName = "cheir", Stores = listS9 };
                ProductDTO p10 = new ProductDTO { ProductId = 10, ProductName = "closet", Stores = listS10 };
                ProductDTO p11 = new ProductDTO { ProductId = 11, ProductName = "shoes", Stores = listS11 };
                ProductDTO p12 = new ProductDTO { ProductId = 12, ProductName = "boots", Stores = listS12 };

                listP.Add(p1);
                listP.Add(p2);
                listP.Add(p3);
                listP.Add(p4);
                listP.Add(p5);
                listP2.Add(p6);
                listP2.Add(p7);
                listP2.Add(p4);
                listP3.Add(p8);
                listP3.Add(p9);
                listP3.Add(p10);
                listP4.Add(p11);
                listP4.Add(p12);
                listP5.Add(p1);
                listP5.Add(p2);
                listP6.Add(p11);
                listP6.Add(p12);
                listP7.Add(p8);
                listP7.Add(p11);
                listP8.Add(p6);
                listP8.Add(p7);

                StoreDTO s1 = new StoreDTO { StoreId = 1, StoreName = "s1", StreatName = "התפוח 12, ירושלים", Products = listP };
                StoreDTO s2 = new StoreDTO { StoreId = 2, StoreName = "s2", StreatName = "השקד 4, ירושלים", Products = listP8 };
                StoreDTO s3 = new StoreDTO { StoreId = 3, StoreName = "s3", StreatName = "האגוז 8, ירושלים", Products = listP2 };
                StoreDTO s4 = new StoreDTO { StoreId = 4, StoreName = "s4", StreatName = "עץ חיים 2, ירושלים", Products = listP3 };
                StoreDTO s5 = new StoreDTO { StoreId = 5, StoreName = "s5", StreatName = "התפוח 12, ירושלים", Products = listP8 };
                StoreDTO s6 = new StoreDTO { StoreId = 6, StoreName = "s6", StreatName = "התות 5, ירושלים", Products = listP4 };
                StoreDTO s7 = new StoreDTO { StoreId = 7, StoreName = "s7", StreatName = "השזיף 3, ירושלים", Products = listP5 };
                StoreDTO s8 = new StoreDTO { StoreId = 8, StoreName = "s8", StreatName = "האשכול 7, ירושלים", Products = listP8 };
                StoreDTO s9 = new StoreDTO { StoreId = 9, StoreName = "s9", StreatName = "השקד 6, ירושלים", Products = listP6 };
                StoreDTO s10 = new StoreDTO { StoreId = 10, StoreName = "s10", StreatName = "התפוח 12, ירושלים", Products = listP7 };

                listS.Add(s1);
                listS.Add(s7);
                listS.Add(s9);
                listS2.Add(s1);
                listS2.Add(s7);
                listS3.Add(s1);
                listS3.Add(s9);
                listS4.Add(s1);
                listS4.Add(s3);
                listS5.Add(s1);
                listS6.Add(s2);
                listS6.Add(s3);
                listS6.Add(s5);
                listS6.Add(s8);
                listS7.Add(s2);
                listS7.Add(s3);
                listS7.Add(s5);
                listS7.Add(s8);
                listS8.Add(s4);
                listS8.Add(s10);
                listS9.Add(s4);
                listS10.Add(s4);
                listS11.Add(s6);
                listS11.Add(s10);
                listS12.Add(s6);

                listOfStores.Add(s1);
                listOfStores.Add(s2);
                listOfStores.Add(s3);
                listOfStores.Add(s4);
                listOfStores.Add(s5);
                listOfStores.Add(s6);
                listOfStores.Add(s7);
                listOfStores.Add(s8);
                listOfStores.Add(s9);
                listOfStores.Add(s10);


                listOfProducts.Add(p1);
                listOfProducts.Add(p2);
                listOfProducts.Add(p3);
                listOfProducts.Add(p4);
                listOfProducts.Add(p8);
                listOfProducts.Add(p9);
                listOfProducts.Add(p11);

                listOfStoresFromTheUser.Add(s5);

                numStores = listOfStores.Count;

                distances = new double[numStores, numStores];


                //חישוב מרחקים בין חנות לחנות והכנסת הנתונים למטריצת המרחקים על ידי גוגל מפות
                await CalculateTheDistToTable();

                //חישוב עדיפות לחנות
                setPriority();

                long bitMask = 0;

                // יצירת מסכה בינארית של חנויות שיש בהן את המוצרים שברשימת הקניות
                bitMask = removesDuplicates(TCP.listOfProducts);

                //הוספת רשימת החנויות שהמשתמש הקיש
                foreach (StoreDTO store in listOfStoresFromTheUser)
                {
                    // TCP.listOfStores.Where(r => r.StoreId == store.StoreId).First().priority = 1;

                    int bit = (int)listOfStoresFromTheUser.First().StoreId;
                    bitMask |= (1L << (bit - 1));
                }





                // Find the shortest route to visit all stores with fiber masks
                var route = FindShortestRoute(bitMask, TCP.numStores);

                // Print the route of the stores according to their ID
                foreach (var store in route.Result)
                {
                    Console.WriteLine(store.StoreName);
                    foreach (var product in store.ProductsUse)
                    {
                        Console.WriteLine(product);

                    }
                }

            }

        }
    }
}
