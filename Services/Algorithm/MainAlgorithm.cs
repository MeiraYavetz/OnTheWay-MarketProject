using Common.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Algorithm
{
    public class MainAlgorithm:IAlgorithm
    {
        //הזרקדת התלויות לקבלת הנתונים מהדטהבייס
        private readonly IService<StoreDTO> _store;
        private readonly IService<ProductDTO> _product;
        private readonly IGetService<ProductStoreDTO> _productStore;

        public MainAlgorithm(IService<StoreDTO> store, IService<ProductDTO> product,
            IGetService<ProductStoreDTO> productStore)
        {
            _store = store;
            _product = product;
            _productStore = productStore;
        }

        //יצירת רשימות אליהן הכל יכנס
        public static List<ProductDTO> products = new List<ProductDTO>();
        public static List<ProductDTO> LeftProducts { get; set; } = new List<ProductDTO>();
        public static List<StoreDTO> listOfStoresFromTheUser = new List<StoreDTO>();
        public static List<ProductStoreDTO> productsStore = new List<ProductStoreDTO>();
        public static List<StoreDTO> Stores { get; set; } = new List<StoreDTO>();
        public static StoreDTO enter { get; set; }

        //הכנסת הנתונים לרשימות
        public async Task init()
        {
            Stores = await _store.GetAllAsync();
            products = await _product.GetAllAsync();
            productsStore = await _productStore.GetAllAsync();
        }
        //מטריצת המרחקים בין חנות לחנות בתוך השוק
        public static double[,] distances;
        public static DataMemoDTO[,] memo;

        //קישור רשימת קניות לרשימת חנויות
        public void initStoresWithProducts()
        {
            foreach (var s in Stores)
            {
                int storeId = (int)s.StoreId;
                List<int> lstProductsId = new List<int>();
                foreach (var ps in productsStore)
                {
                    if (ps.StoreId == storeId)
                        lstProductsId.Add((int)ps.ProductId);
                }
                foreach (int id in lstProductsId)
                {
                    s.Products.Add(products.First(x => x.ProductId == id));
                }
            }
        }
        //קישור רשימת חנויות לרשימת קניות
        public void initProductsWithStores()
        {
            foreach (var p in products)
            {
                int productId = p.ProductId;
                List<int> lstStoresId = new List<int>();
                foreach (var ps in productsStore)
                {
                    if (ps.ProductId == productId)
                        lstStoresId.Add(ps.StoreId);
                }
                foreach (int id in lstStoresId)
                {
                    p.Stores.Add(Stores.First(x => x.StoreId == id));
                }
            }
        }

        //פונקציה שמקבלת רשימה של STRING של שמות המוצרים והופסת לרשימה של מוצרים
        public void initProductsFromShoppingList(List<String> productstring)
        {
            foreach (var product in productstring)
            {
                foreach (var p in products)
                {
                    if (p.ProductName.Equals(product))
                    {
                        LeftProducts.Add(p);
                    }
                }
            }
        }
        //יצירת רשימה של חנויות שקיבלתי מהלקוח 
        public void initStoresFromTheShopingList(List<String> storestring)
        {
            foreach (var store in storestring)
            {
                foreach (var s in Stores)
                {
                    if (s.StoreName.Equals(store))
                    {
                        listOfStoresFromTheUser.Add(s);
                    }
                }
            }
        }
        //קבלת המיקום ההתחלתי של משתמש
        public StoreDTO StartingPlace(String starting)
        {
            StoreDTO store = new StoreDTO();
            store.StoreName = "enter";
            store.StreatName = starting;
            return store;
        }
        //פונקציה שמפעילה את כל האלגוריתם
        public async Task<List<StoreDTO>> MainAlg(DataDTO Lists)
        {
            await init();
            initStoresWithProducts();
            initProductsFromShoppingList(Lists.products);
            initStoresFromTheShopingList(Lists.stores);
            initProductsWithStores();
            //מציאת מקום ההתחלה
            enter = StartingPlace(Lists.startingPlace);
            //הכנסת מקום ההתחלה לרשימת החנויות בשביל יצירת המסלול
            Stores.Insert(0, enter);
            int numStores = Stores.Count;
            //אתחול מטריצת מרחקים
            distances= new double[numStores, numStores];
            //אתחול טבלת זיכרונות בגודל מספר החנויות-2 בחזקת מספר החנויות
            memo = new DataMemoDTO[(numStores), (1 << (numStores))];
            for (int i = 0; i < numStores; i++)
            {
                for (long j = 0; j < (1L << numStores); j++)
                {
                    memo[i, j] = new DataMemoDTO(0,-1, -1);
                }
            }
            //יצירת מסכה בינארית של חנויות שיש בהן את המוצרים שברשימת הקניות
            long bitMask = Calculations.removesDuplicates(LeftProducts,null);
            //חישוב מקדים אם נמצאו מוצרים יחידים 
            long mask= Calculations.singleProduct();
            if (mask != 0)
                bitMask &= mask;
            // קריאה לאלגוריתם מציאת המסלול הקצר ביותר
            var route = TSP.FindShortestRoute(bitMask);
            // הכנסת מוצרים נבחרים לכל חנות
            route = Calculations.setProductsUseInStores(route);
            //החזרת המסלול הנכון 
            return route.Result;
        }
    }
}
