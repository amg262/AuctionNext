import {Auction} from "@/types";
import Link from "next/link";
import CarImage from "@/app/auctions/CarImage";
import CountdownTimer from "@/app/auctions/CountdownTimer";
import CurrentBid from "@/app/auctions/CurrentBid";


type Props = {
  auction: Auction
}

export default function AuctionCard({auction}: Props) {
  return (
      <Link href={`/auctions/details/${auction.id}`} className='group'>
        <div className='w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden'>
          <div>
            <CarImage imageUrl={auction.imageUrl}/>
            <div className='absolute bottom-2 left-2'>
              <CountdownTimer auctionEnd={auction.auctionEnd}/>
            </div>
            <div className='absolute top-2 right-2'>
              <CurrentBid
                  reservePrice={auction.reservePrice}
                  amount={auction.currentHighBid}/>
            </div>
          </div>
        </div>
        <div className='flex justify-between items-center mt-4'>
          <h3 className='text-gray-700'>{auction.make} {auction.model}</h3>
          <p className='font-semibold text-sm'>{auction.year}</p>
        </div>

      </Link>
  )
}
