import React, { useEffect, useState } from 'react';
import ApiService from '../../services/ApiService';

function EncryptedDataList() {
  const [dataList, setDataList] = useState([]);

  useEffect(() => {
    
    ApiService.getEncryptedData()
      .then(data => setDataList(data))
      .catch(error => console.error('Error fetching data:', error));
  }, []);

  return (
    <div>
      <h2>Encrypted Data List</h2>
      <ul>
        {dataList.map(item => (
          <li key={item.id}>
            <strong>ID:</strong> {item.id}<br />
            <strong>Data (Encrypted):</strong> {item.data}<br />
            <strong>Initialization Vector:</strong> {item.initializationVector}<br />
            <strong>Timestamp:</strong> {item.timestamp}<br />
           
          </li>
        ))}
      </ul>
    </div>
  );
}

export default EncryptedDataList;