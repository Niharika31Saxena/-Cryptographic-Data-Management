class EncryptionService {
    constructor(encryptionKey) {
      if (!encryptionKey || typeof encryptionKey !== 'string') {
        throw new Error('Encryption key must be a non-empty string.');
      }
  
      if (encryptionKey.length !== 32) {
        throw new Error('Encryption key must be 32 characters long.');
      }
  
      this.encryptionKey = new TextEncoder().encode(encryptionKey);
    }
  
    async encrypt(data, initializationVector) {
      const iv = new TextEncoder().encode(initializationVector);
      const encodedData = new TextEncoder().encode(data);
  
      const algorithm = {
        name: 'AES-GCM',
        iv: iv,
      };
  
      const key = await crypto.subtle.importKey('raw', this.encryptionKey, algorithm, false, ['encrypt']);
  
      const encrypted = await crypto.subtle.encrypt(algorithm, key, encodedData);
  
      return btoa(String.fromCharCode(...new Uint8Array(encrypted)));
    }
  
    async decrypt(encryptedData, initializationVector) {
      const iv = new TextEncoder().encode(initializationVector);
      const encodedEncryptedData = new Uint8Array(atob(encryptedData).split('').map(char => char.charCodeAt(0)));
  
      const algorithm = {
        name: 'AES-GCM',
        iv: iv,
      };
  
      const key = await crypto.subtle.importKey('raw', this.encryptionKey, algorithm, false, ['decrypt']);
  
      const decrypted = await crypto.subtle.decrypt(algorithm, key, encodedEncryptedData);
  
      return new TextDecoder().decode(decrypted);
    }
  }
  
  export default EncryptionService;