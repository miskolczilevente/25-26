import { useQuery } from '@tanstack/react-query'
import { createFileRoute, useNavigate } from '@tanstack/react-router'
import axios, { Axios, type AxiosResponse } from 'axios'
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Button } from '@/components/ui/button'

export type Dog = {
    id: string
    attributes: {
      name: string
    }
  }

export const Route = createFileRoute('/animals/')({
  component: RouteComponent,
})

function RouteComponent() {

  const invoices = [
    {
      invoice: "INV001",
      paymentStatus: "Paid",
      totalAmount: "$250.00",
      paymentMethod: "Credit Card",
    },
    {
      invoice: "INV002",
      paymentStatus: "Pending",
      totalAmount: "$150.00",
      paymentMethod: "PayPal",
    },
    {
      invoice: "INV003",
      paymentStatus: "Unpaid",
      totalAmount: "$350.00",
      paymentMethod: "Bank Transfer",
    },
    {
      invoice: "INV004",
      paymentStatus: "Paid",
      totalAmount: "$450.00",
      paymentMethod: "Credit Card",
    },
    {
      invoice: "INV005",
      paymentStatus: "Paid",
      totalAmount: "$550.00",
      paymentMethod: "PayPal",
    },
    {
      invoice: "INV006",
      paymentStatus: "Pending",
      totalAmount: "$200.00",
      paymentMethod: "Bank Transfer",
    },
    {
      invoice: "INV007",
      paymentStatus: "Unpaid",
      totalAmount: "$300.00",
      paymentMethod: "Credit Card",
    },
  ]

  const navigate = useNavigate();
  


  const { data: dogs, isLoading: dogsIsLoading } = useQuery({
    queryKey: ["dogs"],
    staleTime: 5 * 1000,
    queryFn: () => axios.get<AxiosResponse<Dog[]>>("https://dogapi.dog/api/v2/breeds")
  })




  if (dogsIsLoading) {
    return <p>Töltés</p>
  }

  if (!dogs) {
    return <p>Nincs adat</p>
  }

  return (

    <Table>
      <TableCaption>A list of your recent invoices.</TableCaption>
      <TableHeader>
        <TableRow>
          <TableHead className="w-[100px]">ID</TableHead>
          <TableHead>Name</TableHead>
          <TableHead>Műveletek</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {dogs.data.data.map((dog) => (
          <TableRow key={dog.id}>
            <TableCell>{dog.id}</TableCell>
            <TableCell>{dog.attributes.name}</TableCell>
            <TableCell><Button onClick={() => {
              navigate({
                to: "/animals/$animalId/view",
                params: {
                  animalId: dog.id
                }
              })
            }}>Megtekintés</Button></TableCell>
          </TableRow>
        ))}
      </TableBody>


    </Table>

  )
}
