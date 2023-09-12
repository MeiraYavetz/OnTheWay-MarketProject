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
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage;
using System.Numerics;

namespace Services.Algorithm
{

    public class Calculations
    {
        //פונקציה שמקבלת רשימה של מוצרים ומחזירה רשימה של החנויות האפשריות
        public static List<StoreDTO> FindStoresWithProducts(List<ProductDTO> products,StoreDTO storeToCheck)
        {
            StoreDTO s0 = new StoreDTO { StoreId = 0 };
            var result = new List<StoreDTO>();
            foreach (var store in MainAlgorithm.Stores)
            {
                int count = 0;
                foreach (var product in products)
                {
                    if (store.Products.Contains(product) || MainAlgorithm.listOfStoresFromTheUser.Contains(store))
                    {
                        result.Add(store);
                        break;
                    }

                    else
                        count++;
                }
                if (store.StoreName.Equals(MainAlgorithm.enter.StoreName))
                {
                    result.Add(store);
                    count = -1;
                }
                if (products.Count == 0 && MainAlgorithm.listOfStoresFromTheUser.Contains(store))
                {
                    result.Add(store);
                    count = -1;
                }
                if (storeToCheck != null && storeToCheck.StoreId == store.StoreId)
                { 
                    result.Add(store);
                    count = -1;
                }
                if (count == products.Count)
                        result.Add(s0);
            }
            return result;
        }
        //פונקציה שממירה את רשימת החנויות למסכה ומחזירה רשימה של 1 או 0 אם צריך את החנות או לא
        public static List<bool> StoresToBinaryList(List<StoreDTO> stores)
        {
            List<bool> binaryList = new List<bool>();
            
            foreach (StoreDTO store in MainAlgorithm.Stores)
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
            return mask;//110 0110 0011 
        }
        //פונקציה ש:
        //(מקבלת את רשימת המוצרים המעודכנת(שחלק מהמוצרים כבר לא נמצאים-מה שהשתמשו כבר
        //שולחת לפונקציה שמחזירה אילו חנויות צריך עכשיו- כלומר אחרי הורדה של מוצרים
        //שולחת לפונקציה נוספת שממירה את רשימת החנויות למסכה
        //מחזירה את המסכה כדי שנוכל להמשיך לעבוד עליה
        public static long removesDuplicates(List<ProductDTO> products,StoreDTO storeToCheck)
        {
            var result=new List<StoreDTO>();
            result = FindStoresWithProducts(products,storeToCheck);
            List<bool> binaryMask = StoresToBinaryList(result);
            //MainAlgorithm.Stores=result;
            long mask = ReverseAndConvertToMask(binaryMask);
            return mask;
        }
        //הפונקציה עוברת על רשימת הביטים-החנויות עבור כל חנות בודקת איזה מוצרים מהרשימה היא יכולה להוריד
        //ומורידה את כל המוצרים מכל החנויות על ידי הורדה מרשימת המוצרים של החנויות האחרות
        //הפונקציה מחזירה מסכה חדשה בלי החנויות שהורדו
        public static long reduceProducts(StoreDTO store)
        {
            List<ProductDTO> currentLeftProducts = new List<ProductDTO>(MainAlgorithm.LeftProducts);

            var contains = store.Products.Where(c => currentLeftProducts.Contains(c));
            contains.ToList().ForEach(x => currentLeftProducts.Remove(x));
            return removesDuplicates(currentLeftProducts,store);
        }
       
        //פונקציה שבודקת האם יש מוצר יחיד שיש אותו רק בחנות אחרת
        //ומעלה את העדיפות לחנות על ידי שמשמיטה משאר החנויות את המוצרים
        public static long singleProduct()
        {
            long bitMask = 0;
            foreach (var p in MainAlgorithm.LeftProducts)
            {
                if (p.Stores.Count() == 1)
                {
                    bitMask = reduceProducts(p.Stores.First());
                }
            }
            return bitMask;
        }
        //יצירת רשימה לכל חנות של איזה מוצרים השתמשו בה
        public static Task<List<StoreDTO>> setProductsUseInStores(Task<List<StoreDTO>> stores)
        {
            foreach(var store in stores.Result)
            {
                foreach (var product in MainAlgorithm.LeftProducts)
                {
                    if (store.Products.Contains(product))
                    {
                        store.ProductsUse.Add(product.ProductName);
                        
                    }
                }
            }
            return stores;
        }
        //הוספה של החנויות אל המסלול לפי הסדר
        public static List<StoreDTO> GetShortestRouteFromMemo(int numStores, long bitMask)
        {
            int n = MainAlgorithm.memo.GetLength(0);
            int i = -1;
            for (int k = 0; k < numStores; k++)
            {
                DataMemoDTO memoEntry = MainAlgorithm.memo[k, bitMask];
                foreach (var m in TSP.masks)
                {
                    if (memoEntry.Cost == m.Cost)
                    {
                        i = k;
                        break;
                    }
                }
            }
            List<StoreDTO> route = new List<StoreDTO>();
            if (i != -1)
            {
                
                int lastStoreId = MainAlgorithm.memo[i, bitMask].LastStoreId; // get the ID of the last store visited in the shortest route
                while (lastStoreId != -1)
                {
                    route.Add(MainAlgorithm.Stores[lastStoreId]); // add the current store to the route
                    int prevStoreId = MainAlgorithm.memo[lastStoreId, bitMask].PrevStoreId; // get the ID of the previous store in the shortest route
                    bitMask &= (~(1L << lastStoreId));
                    lastStoreId = prevStoreId; // update the last store ID to the previous store ID
                }
                long mask = (1 << numStores )- 1;
                bitMask &=(bitMask-1);
                if (bitMask!=0)
                {
                    mask &= bitMask;
                    int position = BitOperations.TrailingZeroCount(bitMask);
                    if ((bitMask & mask) != 0)
                        {
                            route.Add(MainAlgorithm.Stores[position]);
                            bitMask&= ~(1L << position);
                        }
                    
                }
                route.Add(MainAlgorithm.enter);
                route.Reverse(); // reverse the order of stores to get the correct route
            }
            return route;

        }

    }
}

