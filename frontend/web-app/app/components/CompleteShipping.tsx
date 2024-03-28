'use client';

import {AiOutlineCar} from "react-icons/ai";
import React from "react";
import {Payment} from "@/types";

type Props = {
  payment?: Payment | null
}

export default function CompleteShipping({payment}: Props) {
  function ship() {
    const baseUrl = process.env.NEXT_PUBLIC_API_URL;

    console.log('baseUrl', baseUrl);

    // console.log('Shipping completed');
    // console.log('shipping', payment);
    // fetch(`${baseUrl}/shipping/complete/${payment?.id}`, {}).then(r => r.json()).then(console.log).catch(console.error);
    //
    console.log('Shipping completed');
    console.log('shipping', payment);

    const requestOptions = {
      method: 'POST',
    }

    try {
      const response = fetch(`${baseUrl}shipping/complete/${payment?.id}`, requestOptions)
          .then(r => console.log(r.json()))
          .then(console.log)
          .catch(console.error);
      // const response = fetchWrapper.post(`shipping/complete/${payment?.id}`, {});
      console.log(response);
    } catch (error) {
      console.error(error);
    }


    try {
      const response = fetch(`https://api.auctionnext.com/shipping/complete/${payment?.id}`, requestOptions)
          .then(r => console.log(r.json()))
          .then(console.log)
          .catch(console.error);
      // const response = fetchWrapper.post(`shipping/complete/${payment?.id}`, {});
      console.log(response);
    } catch (error) {
      console.error(error);
    }
  }


  return (
      <button
          className="flex items-center justify-center bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded cursor-pointer"
          onClick={ship}
      >
        <AiOutlineCar className="mr-2"/> Complete Shipping
      </button>

  );
}
