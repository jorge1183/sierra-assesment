import { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';

import './custom.css'
import Home from './components/Home';
import ProductManager from './components/ProductManager';
import CustomerManager from './components/CustomerManager';
import OrderManager from './components/OrderManager';
import Login from './components/Login';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/product' component={ProductManager} />
        <Route path='/customer' component={CustomerManager} />
        <Route path='/order' component={OrderManager} />
        <Route path='/login' component={Login} />
      </Layout>
    );
  }
}
