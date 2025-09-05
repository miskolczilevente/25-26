const express = require("express")

const router = express.Router();

const users = 
[
    {
        name: "KGergo02",
        age: 23
    },
    {
        name: "Kis Pista",
        age: 36
    },
    {
        name: "Bence",
        age: 12
    }
]


//localhost:3000/users
router.get("/", (req, res, next) => {
    res.status(200).json(users);

})