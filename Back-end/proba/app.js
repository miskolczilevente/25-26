const express = require("express")
const app = express()

app.use(express.json())
app.use(express.urlencoded({extended: true}))

const bookRoutes = require("./api/routes/bootRoutes")

app.get("/", (req, res, next) =>{
    res.status(302).redirect("/books")
})

app.use("/books", bookRoutes)

module.exports = app