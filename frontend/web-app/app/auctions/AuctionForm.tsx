'use client';

import {FieldValues, useForm} from "react-hook-form";
import {Button} from "flowbite-react";
import Input from "@/app/components/Input";
import {useEffect} from "react";
import DateInput from "@/app/components/DateInput";
import {createAuction, updateAuction} from "@/app/actions/auctionActions";
import {usePathname, useRouter} from "next/navigation";
import toast from "react-hot-toast";
import {Auction} from "@/types";

type Props = {
  auction?: Auction
}
export default function AuctionForm({auction}: Props) {
  const router = useRouter();
  const pathname = usePathname();
  const {
    control, handleSubmit, setFocus, reset,
    formState: {isSubmitting, isValid}
  } = useForm({
    mode: "onTouched",
  });

  useEffect(() => {
    if (auction) {
      const {make, model, color, year, mileage, imageUrl, reservePrice, auctionEnd} = auction;
      reset({make, model, color, year, mileage, imageUrl, reservePrice, auctionEnd});
    }
    setFocus("make");
  }, [setFocus]);

  async function onSubmit(data: FieldValues) {
    try {
      let id = '';
      let res;
      if (pathname === '/auctions/create') {
        res = await createAuction(data);
        id = res.id;
      } else {
        if (auction) {
          res = await updateAuction(data, auction.id);
          id = auction.id;
        }
      }
      if (res.error) {
        throw res.error;
      }
      router.push(`/auctions/details/${id}`)
    } catch (error: any) {
      toast.error(error.status + ' ' + error.message)
    }
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

          {pathname === "/auctions/create" && (
              <>
                <Input label="Image URL" name="imageUrl" control={control}
                       rules={{required: "Image URL is required."}}/>

                <div className="grid grid-cols-2 gap-3">
                  <Input label="Reserve Price (enter 0 if no reserve)" name="reservePrice" control={control}
                         type="number"
                         rules={{required: "Reserve price is required."}}/>
                  <DateInput label="Auction end date/time" name="auctionEnd" control={control}
                             dateFormat="MMMM dd yyyy h:mm a" showTimeSelect/>
                </div>
              </>)}
          <div className="flex justify-between">
            <Button outline color="gray">Cancel</Button>
            <Button outline type="submit"
                    isProcessing={isSubmitting}
                    disabled={!isValid}
                    color="primary">Submit</Button>
          </div>
        </div>
      </form>
  )
}