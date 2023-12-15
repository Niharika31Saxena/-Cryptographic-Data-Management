import React, { useState, useEffect } from 'react';
import ApiService from '../../services/ApiService';

const EncryptedDataDetail = ({ match }) => {
  const [encryptedData, setEncryptedData] = useState({});
  const [decryptedData, setDecryptedData] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
       
        const response = await ApiService.getEncryptedDataById(match.params.id);

       
        setEncryptedData(response.data);

       
        const decrypted = await ApiService.decryptData(response.data.data, response.data.initializationVector);

       
        setDecryptedData(decrypted);
      } catch (error) {
        setError('Error fetching or decrypting data.');
        console.error('Error:', error);
      }
    };

    fetchData();
  }, [match.params.id]);

  return (
    <div>
      <h2>Encrypted Data Detail</h2>
      {error ? (
        <div style={{ color: 'red' }}>{error}</div>
      ) : (
        <>
          <div>
            <strong>ID:</strong> {encryptedData.id}
          </div>
          <div>
            <strong>Data (Encrypted):</strong> {encryptedData.data}
          </div>
          <div>
            <strong>Initialization Vector:</strong> {encryptedData.initializationVector}
          </div>
          <div>
            <strong>Timestamp:</strong> {encryptedData.timestamp}
          </div>
          <div>
            <strong>Data (Decrypted):</strong> {decryptedData}
          </div>
        </>
      )}
    </div>
  );
};

export default EncryptedDataDetail;