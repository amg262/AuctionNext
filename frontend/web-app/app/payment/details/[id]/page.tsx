import Heading from '@/app/components/Heading'
import React from 'react'
import {getPaymentById} from '@/app/actions/auctionActions'
import {Payment} from "@/types";

export default async function Update({params}: { params: { id: string } }) {
  const data = await getPaymentById(params.id) as Payment;

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Payment Succcessful' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{params.id}</h3>

        {/*<ValidatePaymentButton paymentId={params.id}/>*/}
        <pre>{JSON.stringify(data, null, 2)}</pre>

      </div>
  )
}
