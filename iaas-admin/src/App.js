import logo from './logo.svg';
import './App.css';
import { Button } from 'antd';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn Create React App
        </a>
        <Button type='primary' size='small' onClick={envGet}>测试按钮</Button>
      </header>
    </div>
  );
}
function envGet(){
  fetch("/api/host").then(res=>res.json()).then(data=>console.log(data));
  fetch("/api").then(res=>res.json()).then(data=>console.log(data));
}

export default App;
