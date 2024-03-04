import Heading from '@/app/components/Heading'
import React from 'react'
import AuctionForm from '../AuctionForm'

/**
 * `Create` component renders a user interface for submitting car details to create a new auction.
 * @returns The `Create` component returns a JSX element structure that combines the `Heading` and `AuctionForm` components
 * within a styled div to facilitate the creation of new auctions by users.
 */
export default function Create() {
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Sell your car!' subtitle='Please enter the details of your car'/>
        <AuctionForm/>
      </div>
  )
}
