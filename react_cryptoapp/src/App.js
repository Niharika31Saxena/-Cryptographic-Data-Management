import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Header from './components/common/Header';
import Footer from './components/common/Footer';
import Home from './pages/Home';
import EncryptedDataList from './pages/EncryptedData/EncryptedDataList';
import EncryptedDataDetail from './pages/EncryptedData/EncryptedDataDetail';
import EncryptedDataForm from './pages/EncryptedData/EncryptedDataForm';

function App() {
  return (
    <Router>
      <Header />
      <Switch>
        <Route path="/" exact component={Home} />
        <Route path="/encrypted-data" exact component={EncryptedDataList} />
        <Route path="/encrypted-data/:id" component={EncryptedDataDetail} />
        <Route path="/create-encrypted-data" component={EncryptedDataForm} />
      </Switch>
      <Footer />
    </Router>
  );
}

export default App;
