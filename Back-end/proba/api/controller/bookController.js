const books = [
    {
        title: "book1",
        author: "Shakespeare" ,
        price: 3000
    },
    {
        title: "book2",
        author: "JK. Rowling" ,
        price: 5000
    },
    {
        title: "book3",
        author: "Nigga" ,
        price: 7000
    }
]

exports.getBooks = (req, res, next) => {
    const {title, price} = req.querry || {}

    let tempBooks = JSON.parse(JSON.stringify(books))

    if (title) 
    {
        tempBooks = tempBooks.filter(item => item.title === title)
    }

    if (price) 
    {
        tempBooks = tempBooks.filter(item => item.price <= price)   
    }

    res.status(200).json(tempBooks)

    
}

