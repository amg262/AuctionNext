'use client';

import {FieldValues, useForm} from "react-hook-form";
import {Button} from "flowbite-react";
import Input from "@/app/components/Input";
import {useEffect} from "react";

export default function AuctionForm() {
  const {
    control, handleSubmit, setFocus,
    formState: {isSubmitting, isValid, isDirty, errors}
  } = useForm({
    mode: "onTouched",
  });

  useEffect(() => {
    setFocus("make");
  }, [setFocus]);

  function onSubmit(data: FieldValues) {
    console.log(data);
  }

  return (
      <form className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
        <div className="mb-3 block">
          <Input name="make" label="Make" control={control} rules={{required: "Make is required."}}/>
          <Input name="model" label="model" control={control} rules={{required: "Model is required."}}/>
          <Input name="color" label="Color" control={control} rules={{required: "Color is required."}}/>

          <div className="grid grid-cols-2 gap-3">
            <Input name="year" label="Year" control={control} rules={{required: "Year is required."}}/>
            <Input name="mileage" label="Mileage" control={control} rules={{required: "Mileage is required."}}/>
          </div>

          <Input name="Image URL" label="imageUrl" control={control} rules={{required: "Image URL is required."}}/>

          <div className="grid grid-cols-2 gap-3">
            <Input label="Reserve Price (enter 0 if no reserve)" name="reservePrice" control={control} type="number"
                   rules={{required: "Reserve price is required."}}/>
            <Input label="Auction end date/time" name="auctionEnd" type="date" control={control}
                   rules={{required: "Auction end date is required."}}/>
          </div>

          <div className="flex justify-between">
            <Button outline color="gray">Cancel</Button>
            <Button outline type="submit"
                    isProcessing={isSubmitting}
                // disabled={!isValid}
                    color="primary">Submit</Button>
          </div>
        </div>
      </form>
  )
}