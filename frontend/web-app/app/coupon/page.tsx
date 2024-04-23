import Heading from '@/app/components/Heading'
import React from 'react'
import {getCurrentUser} from "@/app/actions/authActions";
import {getCoupons} from "@/app/actions/auctionActions";
import {Coupon} from "@/types";

export default async function Page({params}: { params: { id: string } }) {
  const user = await getCurrentUser();
  const coupons = await getCoupons();
  // I'm trying to do something here cmon man
  // still doing it
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Coupons' subtitle='Manage coupons that sync with Stripe'/>

        {coupons && <pre>{JSON.stringify(coupons, null, 2)}</pre>}

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
