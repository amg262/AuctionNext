import Heading from '@/app/components/Heading'
import React from 'react'
import {Payment} from "@/types";
import {getCurrentUser} from "@/app/actions/authActions";
import {completePayment} from "@/app/actions/auctionActions";
import {numberWithCommas} from "@/app/lib/numberWithComma";
import {AiOutlineCheckCircle, AiOutlineClockCircle} from "react-icons/ai";

export default async function Update({params}: { params: { id: string } }) {
  const user = await getCurrentUser();
  let payment: Payment | null = null;
  let error = null;

  try {
    const {payment: paymentData} = await completePayment(params.id);
    payment = paymentData;
  } catch (err: any) {
    error = err;
  }

  console.log('payment', payment)


  if (error) return <div>Error: {error}</div>;

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Payment Succcessful!' subtitle=''>
          <AiOutlineCheckCircle className="text-green-500 ml-2" />
        </Heading>
          <div className="mt-6">
          <h2 className="text-2xl font-semibold">Transaction Details</h2>
          <div className="mt-4 bg-gray-100 p-4 rounded-lg">
            {payment && (
                <div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Transaction ID:</span>
                    <span>{payment.id}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Date & Time:</span>
                    <span>{new Date(payment.updatedAt).toLocaleString()}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Status:</span>
                    <span className="text-green-500">{payment.status}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Amount:</span>
                    <span>${payment.total}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Discount:</span>
                    <span>${payment.discount}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Buyer:</span>
                    <span>{payment.userId}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Seller:</span>
                    <span>{payment.seller}</span>
                  </div>
                  <div className="flex justify-between items-center py-2">
                    <span className="font-medium">Coupon Code:</span>
                    <span>{payment.couponCode || 'N/A'}</span>
                  </div>
                </div>
            )}
          </div>
        </div>
      </div>
  )
}
