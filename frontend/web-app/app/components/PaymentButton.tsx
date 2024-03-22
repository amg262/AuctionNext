'use client';

import React, {useEffect} from 'react';
import {loadStripe} from '@stripe/stripe-js';
import {createPayment, getPayment} from "@/app/actions/auctionActions";
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
  const [stripeError, setStripeError] = React.useState(null);
  const [stripe, setStripe] = React.useState<any>();
  const couponCode = React.useState<string>();
  const coupons = useCouponStore(state => state.coupons);
  const coupons2 = useCouponStore(initialState => initialState.coupons);
  // const stripePromise = loadStripe(
  //     process.env.NEXTAUTH_STRIPE_SECRET || 'pk_test_51NKl9DJCh47a7Nh113trEHxzNk32tOgF5qtNOCfO2Jb5Hc7D8lC7kY4pVm6L7cQkaY5di4VNw0UPAuGrMSb4e9XB00NfTT04a5');

  useEffect(() => {

    const key = process.env.NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY;

    // console.log("coupon", coupons);
    // console.log("coupon2", coupons2);
    // setCouponCode(coupon?.couponCode || '10OFF')
    // console.log('couponCode', coupon?.couponCode);
    const initializeStripe = async () => {
      if (!stripe) {
        const stripeTmp = await loadStripe(key as string);
        setStripe(stripeTmp);
      }
    };

    initializeStripe().then(r => console.log(r)).catch(e => console.error(e));
  }, [coupon?.couponCode, coupons, stripe]);
  const handleCheckout = async () => {
    setStripeError(null);
    console.log("coupon", coupons);
    console.log("coupon2", coupons2);

    let couponCode = coupons[0].couponCode

    // setCouponCode(coupon?.couponCode)
    // console.log('couponCode', coupon?.couponCode);

    // data.push({"couponCode": couponCode});
    // const stripe = await stripePromise as any;

    const label = "couponCode"
    console.log("couponCode", couponCode);

    const res = await getPayment()
    console.log("res", res);

    data["couponCode"] = couponCode;

    console.log("Updated data object with dynamic label and value:", data);

    // console.log("data", data);

    const response = await createPayment(data);
    console.log(response);

    // const session = JSON.parse(response);

    // Redirect to Stripe Checkout
    const result = await stripe.redirectToCheckout({
      sessionId: response.session.id,
    }).then(function (result: any) {
      if (result.error) {
        setStripeError(result.error.message);
      }
    });
  };

  return (
      // <Button onClick={handleCheckout}>
      //   Make Payment
      // </Button>
      <>
        {stripe ? (
            <button
                className="bg-indigo-600 text-white px-3 py-2 rounded text-base border-0"
                id="checkout-button-price_1Heree568gerg54rtretrt"
                role="link"
                type="button"
                onClick={handleCheckout}
            >
              Checkout
            </button>
        ) : "Loading..."
        }
        {stripeError ? <div id="error-message">{stripeError}</div> : null}
      </>
  );
}