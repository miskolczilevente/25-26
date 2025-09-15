import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import { useQuery } from '@tanstack/react-query';
import axios, { type AxiosAdapter, type AxiosResponse } from 'axios';
import type { Dog } from '..';


export const Route = createFileRoute('/animals/$animalId/view')({
  component: RouteComponent,
})

function RouteComponent() {
    const params = Route.useParams();
    const navigate = useNavigate();


    const {data: dogs , isLoading: dogsIsLoading} = useQuery({
      queryKey: ["dogs", params.animalId],
      queryFn: () => axios.get<AxiosResponse<Dog>>(`https://dogapi.dog/api/v2/breeds/${params.animalId}`)
    })

     if (dogsIsLoading) {
    return <p>Töltés</p>
  }

  if (!dogs) {
    navigate({
      to: "/animals"
    });
    return null;
  }


  return (
    <>
    <p>{dogs.data.data.attributes.name}</p>

    <Button onClick={() => navigate({
      to: "/animals"
      
    })}>Vissza</Button>
    </>
  )
  
  
}

