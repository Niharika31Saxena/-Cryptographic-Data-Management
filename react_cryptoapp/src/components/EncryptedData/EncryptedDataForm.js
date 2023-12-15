import React, { useState } from 'react';
import cryptoRandomString from 'crypto-random-string';
import ApiService from '../../services/ApiService';

const EncryptedDataForm = () => {
  const [data, setData] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleInputChange = (e) => {
    setData(e.target.value);
    setErrorMessage('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      // Generate a random initialization vector
      const initializationVector = generateInitializationVector();

      // Encrypt the data using the API service
      const encryptedData = await ApiService.encryptData(data, initializationVector);

      // Send the encrypted data and initialization vector to the server
      await ApiService.postEncryptedData({ data: encryptedData, initializationVector });

      // Clear the form after successful submission
      setData('');
      setErrorMessage('');
    } catch (error) {
      setErrorMessage('Error submitting encrypted data.');
      console.error('Error:', error);
    }
  };

  const generateInitializationVector = () => {
    // Generate a random 16-byte (128-bit) initialization vector
    return cryptoRandomString({ length: 32, type: 'hex' }); // 32 characters represent 16 bytes in hex
  };

  return (
    <div>
      <h2>Encrypted Data Form</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Data:</label>
          <input type="text" value={data} onChange={handleInputChange} />
        </div>
        <div>
          <button type="submit">Submit</button>
        </div>
        {errorMessage && <div style={{ color: 'red' }}>{errorMessage}</div>}
      </form>
    </div>
  );
};

export default EncryptedDataForm;