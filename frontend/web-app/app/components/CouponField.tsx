'use client';

import {Button} from "flowbite-react";
import {ChangeEvent, useState} from "react";
import {Coupon} from "@/types";
import {getCoupon} from "@/app/actions/auctionActions";
import {useCouponStore} from "@/hooks/useCouponStore";


export default function CouponField() {

  const [couponCode, setCouponCode] = useState('');
  const [coupon, setCoupon] = useState<Coupon | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const setCoupons = useCouponStore(state => state.setCoupons);
  const coupons = useCouponStore(state => state.coupons);

  const handleCodeChange = (event: ChangeEvent<HTMLInputElement>) => {
    console.log('handleCodeChange');
    setCouponCode(event.target.value);
  };

  const handleCouponApply = async () => {
    console.log('handleCouponApply');
    setIsLoading(true);
    try {
      setIsLoading(true);
      let coupon = await getCoupon(couponCode);
      coupons.push(coupon);
      console.log("coupon", coupon);
      console.log("coupons", coupons);

    } catch (error) {
      console.error('Error fetching coupon:', error);
      // Handle the error (e.g., display an error message)
    } finally {
      setIsLoading(false);
    }
  };

  return (
      <div className="flex items-center space-x-2">
        <input
            type="text"
            placeholder="Coupon code"
            className="border p-2 rounded"
            value={couponCode}
            onChange={handleCodeChange}
            disabled={isLoading}
        />
        <Button outline onClick={handleCouponApply} disabled={isLoading}>
          {isLoading ? 'Applying...' : 'Apply'}
        </Button>
      </div>
  );
}