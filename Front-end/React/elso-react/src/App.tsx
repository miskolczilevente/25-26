import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [barmi, setbarmi] = useState<number>(0)
  const [barmistr, setbarmistr] = useState<string>("")

  return (
    <>
      <button onClick={ () => setbarmi(10)}>nigga</button>
      
      <p>{barmi}</p>

      <input onChange={ (asd) => setbarmistr(asd.target.value)}></input>

      <p>{barmistr}</p>

    </>
  )
}

export default App
