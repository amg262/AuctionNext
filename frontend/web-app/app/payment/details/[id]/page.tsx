import Heading from '@/app/components/Heading'
import React from 'react'
import {Payment} from "@/types";

export default async function Update({params}: { params: { id: string } }) {


  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Payment Succcessful' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{params.id}</h3>

        {/*<Button onClick={async () => GetPayment(params.id)}>Refresh Payment Data</Button>*/}

        {/*<ValidatePaymentButton paymentId={params.id}/>*/}

        {/*{payment && <pre>{JSON.stringify(payment, null, 2)}</pre>}*/}

      </div>
  )
}
