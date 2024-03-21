'use client';

import {Button} from "flowbite-react";
import {ChangeEvent, useState} from "react";
import {Coupon} from "@/types";
import {getCoupon} from "@/app/actions/auctionActions";


export default function CouponField() {

  const [couponCode, setCouponCode] = useState('10OFF');
  const [coupon, setCoupon] = useState<Coupon | null>(null);

  const handleCodeChange = (event: ChangeEvent<HTMLInputElement>) => {
    console.log('handleCodeChange');

    setCouponCode(event.target.value);
  };

  const handleCouponApply = async () => {
    console.log('handleCouponApply');
    try {
      const fetchedCoupon = await getCoupon(couponCode);
      console.log(fetchedCoupon);
      setCoupon(fetchedCoupon);
    } catch (error) {
      console.error('Error fetching coupon:', error);
      // Handle the error (e.g., display an error message)
    }
  };

  // useEffect(() => {
  //   console.log('useEffect');
  //
  //   handleCouponApply().then(r => console.log(r)).catch(e => console.error(e));
  // });

  return (
      <div className="flex items-center space-x-2">
        <input
            type="text"
            placeholder="Coupon code"
            className="border p-2 rounded"
            value={couponCode}
            onChange={handleCodeChange}
        />
        <Button outline onClick={handleCouponApply}>
          Apply
        </Button>
      </div>
  );
}