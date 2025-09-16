const express = require("express")
const router = express.Router()
const bookController = require("../controller/bookController")

router.get("/", bookController.getBooks)
router.get("/", bookController.getBookbyFilter)

module.exports = router