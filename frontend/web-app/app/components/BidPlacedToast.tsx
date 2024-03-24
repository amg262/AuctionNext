import {Bid} from '@/types'
import Image from 'next/image'
import React from 'react'

type Props = {
  bid?: Bid
}

export default function BidPlacedToast({bid}: Props) {

  return (
      <div className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src="/bid.png"
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>Bid placed for ${bid?.amount} by {bid?.bidder}!</span>
        </div>
      </div>
  )
}
