'use client';

import {Payment} from "@/types";
import {User} from "next-auth";
import React, {useState} from "react";
import {AiOutlineDollarCircle} from 'react-icons/ai'; // Assuming you want to use a dollar icon for the refund button

type Props = {
  payment?: Payment | null;
  headers?: any;
  user?: User | null;
}

export default function RefundButton({payment, headers, user}: Props) {
  const [isLoading, setIsLoading] = React.useState(false);
  const [error, setError] = React.useState(null);
  const [refund, setRefund] = React.useState(null);
  const [refundError, setRefundError] = React.useState(null);
  const [refundStatus, setRefundStatus] = useState({success: false, error: null});
  const [buttonClicked, setButtonClicked] = useState(false); // New state to track if the button was ever clicked


  function refundPayment() {
    setIsLoading(true);
    setRefundStatus({success: false, error: null}); // Reset refund status
    setButtonClicked(true); // Set this to true as soon as the button is clicked

    console.log('Refunding payment');

    const requestOptions = {
      method: 'POST',
      headers: headers,
    }

    // console.log('user', user)
    // console.log('headers', headers)

    try {
      const response = fetch(`https://api.auctionnext.com/payment/refund/${payment?.paymentIntentId}`, requestOptions)
          .then(response => response.json())
          .then(data => {
            setRefund(data);
            setRefundStatus({success: true, error: null});
            setIsLoading(false);
          })
          .catch(err => {
            console.error(err);
            setRefundStatus({success: false, error: err.message || 'Refund failed'});
            setRefundError(err);
            setIsLoading(false);
          });
      // const response = fetchWrapper.post(`shipping/complete/${payment?.id}`, {});
      console.log(response);
    } catch (error) {
      console.error(error);
    }
  }

  return (
      <div>
        <button
            className={`flex items-center justify-center text-white font-bold py-2 px-4 rounded cursor-pointer
             ${buttonClicked || isLoading ? 'bg-red-400 hover:bg-red-400 opacity-75 pointer-events-none' : 'bg-red-500 hover:bg-red-700'}`}
            onClick={refundPayment}
            disabled={isLoading || buttonClicked}
        >
          <AiOutlineDollarCircle className="mr-2"/>
          {isLoading ? 'Processing...' : 'Refund'}
        </button>

        {refundStatus.success && (
            <div className="mt-4 p-4 bg-green-100 text-green-700 rounded-lg">Refund Processed Successfully!</div>
        )}
        {refundStatus.error && (
            <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-lg">{refundStatus.error}</div>
        )}
      </div>
  );
}