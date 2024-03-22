import {Auction, Bid} from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
  auction: Auction
  bid?: Bid
}

export default function BidPlacedToast({auction, bid}: Props) {
  return (
      <Link href={`/auctions/details/${auction.id}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src={auction.imageUrl}
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>Bid placed for {bid?.amount} by {bid?.bidder}!</span>
        </div>
      </Link>
  )
}
