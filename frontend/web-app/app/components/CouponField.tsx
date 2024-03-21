'use client';

import {Button} from "flowbite-react";

export default function CouponField() {
  return (
      <div className="flex items-center space-x-2">
        <input type="text" placeholder="Coupon code" className="border p-2 rounded"/>
        <Button outline type="submit">
          Apply
        </Button>
      </div>
  );
}