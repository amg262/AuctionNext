'use client'

type Props = {
  auctionId: string;
  highBid: number;
}

import {placeBidForAuction} from '@/app/actions/auctionActions';
import {numberWithCommas} from '@/app/lib/numberWithComma';
import {useBidStore} from '@/hooks/useBidStore';
import React from 'react'
import {FieldValues, useForm} from 'react-hook-form';
import {toast} from 'react-hot-toast';

/**
 * Renders a form for placing a bid on an auction.
 *
 * This component allows users to enter a bid amount for a specific auction. It validates the bid amount
 * to ensure it is higher than the current highest bid. If the validation fails, it displays an error message.
 * On successful submission, the bid is added to the auction, and the form is reset.
 *
 * Props:
 * - `auctionId`: The unique identifier for the auction.
 * - `highBid`: The current highest bid for the auction.
 * @param {Props} props The props containing auctionId and highBid for the auction.
 * @returns A JSX element representing a form where users can submit their bids.
 */
export default function BidForm({auctionId, highBid}: Props) {
  const {register, handleSubmit, reset, formState: {errors}} = useForm();
  const addBid = useBidStore(state => state.addBid);

  /**
   * Handles the submission of the bid form.
   * Validates the bid amount to ensure it exceeds the current highest bid. If the bid is valid,
   * it attempts to place the bid for the auction. Displays an error message if the bid placement fails.
   *
   * @param {FieldValues} data The form data containing the bid amount.
   */
  function onSubmit(data: FieldValues) {
    if (data.amount <= highBid) {
      reset();
      return toast.error('Bid must be at least $' + numberWithCommas(highBid + 1))
    }

    placeBidForAuction(auctionId, +data.amount).then(bid => {
      if (bid.error) throw bid.error;
      addBid(bid);
      reset();
    }).catch(err => toast.error(err.message));
  }

  return (
      <form onSubmit={handleSubmit(onSubmit)} className='flex items-center border-2 rounded-lg py-2'>
        <input
            type="number"
            {...register('amount')}
            className='input-custom text-sm text-gray-600'
            placeholder={`Enter your bid (minimum bid is $${numberWithCommas(highBid + 1)})`}
        />
      </form>
  )
}
