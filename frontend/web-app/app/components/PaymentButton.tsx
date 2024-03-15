'use client';

import React from 'react';
import {loadStripe} from '@stripe/stripe-js';
import {createPayment, getPayment} from "@/app/actions/auctionActions";
import {Button} from "flowbite-react";

type Props = {
  id: string;
  price: number;
  model: string;
  data: any;
  auctionId: string;
}


export default function PaymentButton({id, price, model, data}: Props) {
  const stripePromise = loadStripe(
      process.env.NEXTAUTH_STRIPE_SECRET || 'pk_test_51NKl9DJCh47a7Nh113trEHxzNk32tOgF5qtNOCfO2Jb5Hc7D8lC7kY4pVm6L7cQkaY5di4VNw0UPAuGrMSb4e9XB00NfTT04a5');

  const handleCheckout = async () => {
    const stripe = await stripePromise as any;

    // const response = await fetchWrapper.post("payments/create", JSON.stringify({
    //   soldAmount: price * 100, // Stripe expects amount in cents
    //   model: model,
    //   auctionId: id,
    // }));
    // const response = await fetch(process.env.'api/payments/create', {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': 'application/json',
    //   },
    //   body: JSON.stringify({
    //     soldAmount: price * 100, // Stripe expects amount in cents
    //     model: model,
    //     auctionId: id,
    //   }),
    // });

    // const response = await createPayment(JSON.stringify({
    //   soldAmount: price * 100,
    //   model: model,
    //   auctionId: id,
    // }));

    const res = await getPayment()
    console.log(res);

    const response = await createPayment(data);
    console.log(response);

    // const session = JSON.parse(response);

    // Redirect to Stripe Checkout
    const result = await stripe.redirectToCheckout({
      sessionId: response.session.id,
    });

    if (result.error) {
      alert(result.error.message);
    }
  };

  return (
      <Button onClick={handleCheckout}>
        Make Payment
      </Button>
  );
}