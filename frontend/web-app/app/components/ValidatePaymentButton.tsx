'use client';

import {Button} from "flowbite-react";
import {getPaymentById} from "@/app/actions/auctionActions";
import {Payment} from "@/types";
import {useEffect, useState} from "react";

type Props = {
  paymentId: string;
}

export default async function ValidatePaymentButton({paymentId}: Props) {
  const [payment, setPayment] = useState<Payment | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const data = await getPaymentById(paymentId) as Payment;

  const handleClick = async () => {
    setIsLoading(true);

    try {
      const paymentData = await getPaymentById(paymentId) as Payment;
      setPayment(paymentData);
    } catch (error) {
      console.error('Error fetching payment', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    handleClick();
  }, [handleClick]);

  return (
      <div>
        <Button onClick={handleClick}>
          {isLoading ? 'Loading...' : 'Refresh Payment'}
        </Button>

        {payment && (
            // Display payment data here...
            <div>
              <p>Payment Status: {payment.status}</p>
              <pre>{JSON.stringify(payment, null, 2)}</pre>
            </div>
        )}
      </div>
  );
}