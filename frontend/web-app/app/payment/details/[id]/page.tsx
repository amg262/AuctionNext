import Heading from '@/app/components/Heading'
import React from 'react'
import {getDetailedViewData} from '@/app/actions/auctionActions'

export default async function Update({params}: { params: { id: string } }) {
  const data = await getDetailedViewData(params.id);

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='laskjdfls' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{params.id}</h3>

      </div>
  )
}
