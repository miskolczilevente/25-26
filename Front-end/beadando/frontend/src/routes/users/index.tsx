import { createFileRoute } from '@tanstack/react-router'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'

import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"

type User = {
  id: number
  name: string
  email: string
}

export const Route = createFileRoute('/users/')({
  component: RouteComponent,
})

async function getUsers(): Promise<User[]> {
  const response = await axios.get("http://localhost:3001/api/users")
  return response.data
}

function RouteComponent() {
  const queryClient = useQueryClient()

  const { data: users, isLoading, isError } = useQuery<User[]>({
    queryKey: ['users'],
    queryFn: getUsers,
  })

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault()

    const form = e.currentTarget
    const username = (form.elements.namedItem("username") as HTMLInputElement).value
    const email = (form.elements.namedItem("email") as HTMLInputElement).value
    const password = (form.elements.namedItem("password") as HTMLInputElement).value

    try {
      await axios.post("http://localhost:3001/api/users", {
        username,
        email,
        password,
      })

      await queryClient.invalidateQueries({ queryKey: ['users'] })
      alert("Successfully!")
      form.reset()
    } catch (error) {
      alert("Failed")
      console.error(error)
    }
  }

  if (isLoading) return <p className="p-4">Loading...</p>
  if (isError) return <p className="p-4 text-red-500">Error loading users.</p>

  return (
    <div className="p-6">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Username</TableHead>
            <TableHead>Email</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {users?.map((user) => (
            <TableRow key={user.id}>
              <TableCell className="font-medium">{user.name}</TableCell>
              <TableCell>{user.email}</TableCell>
            </TableRow>
          ))}
          <TableRow>
            <TableCell colSpan={3}>
              <Dialog>
                <DialogTrigger asChild>
                  <Button variant="outline">Create User</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                  <DialogHeader>
                    <DialogTitle>Create user</DialogTitle>
                    <DialogDescription>
                      Fill out the form to create a new user.
                    </DialogDescription>
                  </DialogHeader>

                  <form className="grid gap-4" onSubmit={handleSubmit}>
                    <div className="grid gap-3">
                      <Label htmlFor="username">Username</Label>
                      <Input id="username" name="username" placeholder="JohnDoe" />
                    </div>
                    <div className="grid gap-3">
                      <Label htmlFor="email">Email</Label>
                      <Input id="email" name="email" placeholder="john@example.com" />
                    </div>
                    <div className="grid gap-3">
                      <Label htmlFor="password">Password</Label>
                      <Input id="password" name="password" type="password" placeholder="********" />
                    </div>

                    <DialogFooter>
                      <DialogClose asChild>
                        <Button variant="outline" type="button">
                          Cancel
                        </Button>
                      </DialogClose>
                      <Button type="submit">Create</Button>
                    </DialogFooter>
                  </form>
                </DialogContent>
              </Dialog>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  )
}
