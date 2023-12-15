import crypto from 'crypto';

const BASE_URL = ''; 

class ApiService {
 
  static getEncryptedData = async () => {
    const response = await fetch(${BASE_URL}/api/EncryptedData);
    if (!response.ok) {
      throw new Error(Error fetching encrypted data: ${response.statusText});
    }
    return response.json();
  };


  static getEncryptedDataById = async (id) => {
    const response = await fetch(${BASE_URL}/api/EncryptedData/${id});
    if (!response.ok) {
      throw new Error(Error fetching encrypted data by ID: ${response.statusText});
    }
    return response.json();
  };

  
  static postEncryptedData = async (encryptedData) => {
    const response = await fetch(${BASE_URL}/api/EncryptedData, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(encryptedData),
    });

    if (!response.ok) {
      throw new Error(Error posting encrypted data: ${response.statusText});
    }
  };

  
}

export default ApiService;