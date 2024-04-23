'use client';

import Heading from '@/app/components/Heading'
import React, {useEffect, useState} from 'react'
import {getCoupons} from "@/app/actions/auctionActions";
import {Coupon} from "@/types";

export default function Page({params}: { params: { id: string } }) {
  const [coupons, setCoupons] = useState<Coupon[]>([]);
  const [error, setError] = useState<any>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    async function fetchCoupons() {
      setIsLoading(true);
      try {
        const coupons = await getCoupons();
        setCoupons(coupons);
        setIsLoading(false);
      } catch (error) {
        setError(error instanceof Error ? error.message : "An unknown error occurred");
        setIsLoading(false);
        console.error('Failed to fetch coupons', error);
      }
    }

    fetchCoupons().then(r => r).catch(e => e);
  }, [coupons]);


  if (error) {
    return <div>Error: {error}</div>;
  }

  // const coupons = await getCoupons();
  // I'm trying to do something here cmon man
  // still doing it
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Coupons' subtitle='Manage coupons that sync with Stripe'/>

        <div className="flex flex-wrap gap-5">
          {coupons && coupons.map((coupon: Coupon) => (
              <div key={coupon.couponId} className="flex-1 min-w-[200px] shadow-md p-4 rounded-lg">
                <h3 className="text-lg font-bold">{coupon.couponCode}</h3>
                <p>Discount: {coupon.discountAmount}</p>
                <p>Min Amount: {coupon.minAmount}</p>
              </div>
          ))}
        </div>
      </div>
  )
}
