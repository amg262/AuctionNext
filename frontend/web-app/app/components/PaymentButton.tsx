'use client';

import React, {useEffect, useState} from 'react';
import {loadStripe} from '@stripe/stripe-js';
import {createPayment} from "@/app/actions/auctionActions";
import {Coupon} from "@/types";
import {useCouponStore} from "@/hooks/useCouponStore";

type Props = {
  // id: string;
  // price: number;
  // model: string;
  data: any;
  coupon?: Coupon;
  // auctionId: string;
  // dataObject: any;
}


export default function PaymentButton({data, coupon}: Props) {
  const [stripeError, setStripeError] = React.useState('');
  const [stripe, setStripe] = React.useState<any>();
  const [loading, setLoading] = useState(false); // Loading state
  const coupons = useCouponStore(state => state.coupons);
  const coupons2 = useCouponStore(initialState => initialState.coupons);

  useEffect(() => {

    const key = process.env.NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY;
    const initializeStripe = async () => {
      if (!stripe) {
        const stripeTmp = await loadStripe(key as string);
        setStripe(stripeTmp);
      }
    };

    initializeStripe().then(r => console.log(r)).catch(e => console.error(e));
  }, [coupon?.couponCode, coupons, stripe]);
  const handleCheckout = async () => {
    setLoading(true);
    setStripeError('');

    // Check if there are any coupons and assign the first coupon's code to couponCode
    if (coupons.length > 0) {
      data["couponCode"] = coupons[0].couponCode;
      data["coupons"] = coupons;
    }
    try {
      const response = await createPayment(data);
      console.log(response);

      const result = await stripe.redirectToCheckout({
        sessionId: response.session.id,
      });

      if (result.error) {
        setStripeError(result.error.message);
        setLoading(false); // Stop loading in case of error
      }
    } catch (error) {
      console.error(error);
      setStripeError("An error occurred. Please try again.");
      setLoading(false); // Stop loading in case of error
    }
  };

  return (
      // <Button onClick={handleCheckout}>
      //   Make Payment
      // </Button>
      <>
        {stripe ? (
            <button
                className={`bg-indigo-600 text-white px-3 py-2 rounded text-base border-0 ${loading ? 'opacity-50 cursor-not-allowed' : ''}`}
                id="checkout-button"
                role="link"
                type="button"
                onClick={handleCheckout}
                disabled={loading} // Disable button when loading
            >
              {loading ? 'Processing...' : 'Checkout'}
            </button>
        ) : "Loading Stripe..."}
        {stripeError && <div id="error-message">{stripeError}</div>}
      </>
  );
}