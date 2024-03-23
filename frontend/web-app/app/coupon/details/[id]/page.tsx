import Heading from '@/app/components/Heading'
import React from 'react'
import {Payment} from "@/types";
import {getCurrentUser} from "@/app/actions/authActions";
import {completePayment} from "@/app/actions/auctionActions";

export default async function Update({params}: { params: { id: string } }) {
  const user = await getCurrentUser();
  let payment: Payment | null = null;
  let error = null;

  try {
    payment = await completePayment(params.id);
  } catch (err: any) {
    error = err;
  }

  if (error) return <div>Error: {error}</div>;

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Payment Succcessful' subtitle='bahhhh'/>
        <h1>Helllllllo!</h1>

        <h3>{user?.username}</h3>

        <h3>{params.id}</h3>

        {payment && <pre>{JSON.stringify(payment, null, 2)}</pre>}

      </div>
  )
}
