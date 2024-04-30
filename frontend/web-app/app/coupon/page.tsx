'use client';

import Heading from '@/app/components/Heading'
import React, {useEffect, useState} from 'react'
import {createCoupon, getCoupons} from "@/app/actions/auctionActions";
import {Coupon} from "@/types";
import Spinner from "@/app/components/Spinner";
import {Label, TextInput} from "flowbite-react";

type CouponForm = {
  couponCode: string;
  discountAmount: number;
  minAmount: number;
}

export default function Page({params}: { params: { id: string } }) {
  const [coupons, setCoupons] = useState<Coupon[]>([]);
  const [coupon, setCoupon] = useState<Coupon>({couponCode: '', discountAmount: 0, minAmount: 0} as Coupon);
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
  }, [coupons.length]);

  const handleCreateCoupon = async (couponData: Coupon) => {
    setIsLoading(true);
    try {
      const res = await createCoupon(couponData);
      // Re-fetch coupons or update local state to include the new coupon
    } catch (error) {
      setError(error instanceof Error ? error.message : "Failed to create coupon");
    }
    setIsLoading(false);
  };

  const handleFormSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent the default form submission behavior

    const target = event.target as typeof event.target & {
      couponCode: { value: string };
      discountAmount: { value: string };
      minAmount: { value: string };
    };

    const couponData: Coupon = {
      couponCode: target.couponCode.value,
      discountAmount: parseFloat(target.discountAmount.value),
      minAmount: parseFloat(target.minAmount.value)
    };

    setIsLoading(true);
    try {
      const res = await createCoupon(couponData);
      setCoupons(prev => [...prev, couponData]);
    } catch (error) {
      setError(error instanceof Error ? error.message : "Failed to create coupon");
    }
    setIsLoading(false);
  };


  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Coupons' subtitle='Manage coupons that sync with Stripe'/>

        {isLoading ? (
            <Spinner/>
        ) : (
            <div>
              {/*<CouponForm onCreate={handleCreateCoupon}/>*/}

              <div className="flex max-w-md flex-col gap-4 my-5">
                <form onSubmit={handleFormSubmit}>
                  <div>
                    <div className="mb-2 block">
                      <Label htmlFor="couponCode" value="Coupon Code"/>
                    </div>
                    <TextInput id="couponCode" type="text" sizing="md" name="couponCode"/>
                  </div>
                  <div>
                    <div className="mb-2 block">
                      <Label htmlFor="discountAmount" value="Discount Amount"/>
                    </div>
                    <TextInput id="discountAmount" type="text" sizing="md" name="discountAmount"/>
                  </div>
                  <div>
                    <div className="mb-2 block">
                      <Label htmlFor="minAmount" value="Min. Amount"/>
                    </div>
                    <TextInput id="minAmount" type="text" sizing="lg" name="minAmount"/>
                  </div>
                </form>
              </div>
              <button type="submit">Submit</button>
              <div className="flex flex-wrap gap-5 mt-5">
                {coupons && coupons.map((coupon: Coupon) => (
                    <div key={coupon.couponId} className="flex-1 min-w-[200px] shadow-md p-4 rounded-lg">
                      <h3 className="text-lg font-bold">{coupon.couponCode}</h3>
                      <p>Discount: {coupon.discountAmount}</p>
                      <p>Min Amount: {coupon.minAmount}</p>
                    </div>
                ))}
              </div>

            </div>
        )}
      </div>
  )
}
