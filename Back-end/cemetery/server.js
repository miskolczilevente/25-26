require("dotenv").config();

const app = require("./app");

const PORT = process.env.PORT || 8000;

require("./api/db/db")

app.listen(PORT, () => 
{
    console.log(`http://localhost:${PORT}`);
});