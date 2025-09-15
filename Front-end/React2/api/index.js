const express = require('express')
const app = express()
const port = 5000

app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.get('/api/users', (req, res) => {
  const users = [
    { id: '1', name: 'Janos', age: 10 },
    { id: '2', name: 'Anna', age: 22 },
    { id: '3', name: 'Peter', age: 35 },
    { id: '4', name: 'Maria', age: 28 },
    { id: '5', name: 'John', age: 45 },
    { id: '6', name: 'Sophia', age: 31 },
    { id: '7', name: 'Michael', age: 50 },
    { id: '8', name: 'Emma', age: 27 },
    { id: '9', name: 'David', age: 33 },
    { id: '10', name: 'Laura', age: 29 }
  ]
  res.json(users)
})

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`)
})
