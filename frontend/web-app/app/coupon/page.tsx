import Heading from '@/app/components/Heading'
import React from 'react'
import {getCurrentUser} from "@/app/actions/authActions";

export default async function Update({params}: { params: { id: string } }) {
  const user = await getCurrentUser();
  // I'm trying to do something here cmon man
  // still doing it
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='laskjdfls' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{params.id}</h3>
        <h3>{user?.username}</h3>

      </div>
  )
}
