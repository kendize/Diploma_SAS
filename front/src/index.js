import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import NavigationBar from './components/NavigationBar/NavigationBar';
import {Provider} from 'react-redux'
import store from './store/store';
import { initFacebookSdk } from './utils/init-facebook-sdk';
import { QueryClient, QueryClientProvider } from "react-query";
initFacebookSdk().then(startApp())

function startApp () {
  const queryClient = new QueryClient();
  ReactDOM.render(
    <QueryClientProvider client={queryClient}>
    <Provider store={store}>
    <React.StrictMode>
      <App />
      <NavigationBar />
      
      
      
    </React.StrictMode>
    </Provider></QueryClientProvider>,
    document.getElementById('root')
  );
}


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
