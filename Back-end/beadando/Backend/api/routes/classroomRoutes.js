const express = require("express");

const router = express.Router();

const classroomController = require("../controllers/classroomController");

router.get("/", classroomController.getClassrooms);

router.post("/", classroomController.createClassroom);

module.exports = router;