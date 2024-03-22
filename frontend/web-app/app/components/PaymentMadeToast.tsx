import {Payment} from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'
import {numberWithCommas} from "@/app/lib/numberWithComma";

type Props = {
  payment: Payment
}

export default function PaymentMadeToast({payment}: Props) {
  return (
      <Link href={`/payment/details/${payment.id}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src="payment.png"
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>SOLD: {payment.buyer} made a payment of ${numberWithCommas(payment.total)}</span>
        </div>
      </Link>
  )
}
