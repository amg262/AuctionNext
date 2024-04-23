'use client';

import React from "react";
import {Coupon} from "@/types";
import {Label, TextInput} from "flowbite-react";

type Props = {
  onCreate: (event: React.FormEvent<HTMLFormElement>) => Promise<void>;
  isSubmitting: boolean;
  handleChange: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  handleKeyDown: (event: React.KeyboardEvent<HTMLTextAreaElement>) => void;
  handleSubmit: (event: React.FormEvent<HTMLFormElement>) => Promise<void>;
}
export default function CouponForm({onCreate, isSubmitting, handleChange, handleSubmit, handleKeyDown}: Props) {
  return (
      <div className="flex max-w-md flex-col gap-4 my-5">
        <form onSubmit={onCreate}>
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

          <button type="submit">Submit</button>
        </form>
      </div>
  );
}