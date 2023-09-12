import axios, { AxiosError } from "axios";
//עדיין אין לי
export const signIn = async (data) => {debugger
  try{debugger
    console.log(data);
     const response=await axios.post(`https://localhost:7028/api/SignIn`,data);
     console.log(response.data) ;
     return response.data ;
  }
  catch(error){
    throw error;
  }
   // return await (await axios.post(`https://localhost:7028/api/SignIn`,data)).data;

};
export const signUpApi = async (data) => {debugger
    try {
      const response = await axios.post(
        `https://localhost:7028/api/Customer`,data
      );
      
      console.log(response.data) ;
    } catch (error) {
      throw error;
    }
  };
  

export const getAllProductsAndShops = async () => {
    debugger
    return await  axios.get(`https://localhost:7028/api/TSP`);
}
export const getShops = async () => {
  debugger
  return await  axios.get(`https://localhost:7028/api/Store`);
}
//לטיפול
export const createShop = async (shop) => {debugger
  
    return await axios.post(`https://localhost:7028/api/Store`,shop);

}
//לטיפול
export const removeShop = async (nemeShop) => {
  
    return await axios.delete(`https://localhost:7028/api/Store/${nemeShop}`);

}
export const createRoute = async (route) => {
  try {
    const response = await axios.post(`https://localhost:7028/api/TSP`, route);
    debugger
    console.log(response.data); 
    
  }catch (error) {
    console.error(error);
  }
}
